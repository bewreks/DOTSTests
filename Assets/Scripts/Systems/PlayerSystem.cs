using Components;
using Events;
using Unity.Burst;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;

namespace Systems
{
	[UpdateInGroup(typeof(InitializationSystemGroup))]
	[UpdateAfter(typeof(PointerSystem))]
	public partial class PlayerSystem : SystemBase
	{
		private EntityQuery _userClickEvent;
		private EntityQuery _movedEntityQuery;
		private EntityQuery _notMovedEntityQuery;

		private EndInitializationEntityCommandBufferSystem _commandBufferSystem;

		protected override void OnCreate()
		{
			_commandBufferSystem = World.GetOrCreateSystem<EndInitializationEntityCommandBufferSystem>();

			_movedEntityQuery = GetEntityQuery(new EntityQueryDesc
			                                   {
				                                   All = new ComponentType[]
				                                         {
					                                         typeof(PlayerMarker),
					                                         typeof(Translation),
					                                         typeof(MoveToComponent)
				                                         }
			                                   });
			
			_notMovedEntityQuery = GetEntityQuery(new EntityQueryDesc
			                                      {
				                                      All = new ComponentType[]
				                                            {
					                                            typeof(PlayerMarker),
					                                            typeof(Translation)
				                                            },
				                                      None = new ComponentType[]
				                                             {
					                                             typeof(MoveToComponent)
				                                             }
			                                      });
			
			RequireForUpdate(_userClickEvent);
		}

		protected override void OnUpdate()
		{
			var position = float3.zero;
			var ecb      = _commandBufferSystem.CreateCommandBuffer();

			Entities
				.WithAll<UserClickEvent>()
				.WithStoreEntityQueryInField(ref _userClickEvent)
				.ForEach((ref UserClickEvent click) => { position = click.Position; }).Run();

			if (!_notMovedEntityQuery.IsEmpty)
			{
				var startMoveToJob = new StartMoveToJob
				                     {
					                     Ecb      = ecb.AsParallelWriter(),
					                     Position = position,
					                     Speed    = 0.1f
				                     };
				
				Dependency = startMoveToJob.ScheduleParallel(_notMovedEntityQuery, Dependency);
			}

			if (!_movedEntityQuery.IsEmpty)
			{
				Dependency = new RestartMoveToJob
				             {
					             Ecb      = ecb.AsParallelWriter(),
					             Position = position,
					             Speed    = 0.1f
				             }.ScheduleParallel(_movedEntityQuery, Dependency);
			}

			_commandBufferSystem.AddJobHandleForProducer(Dependency);
		}
	}

	[BurstCompile]
	public partial struct StartMoveToJob : IJobEntity
	{
		public float                              Speed;
		public float3                             Position;
		public EntityCommandBuffer.ParallelWriter Ecb;

		private void Execute(Entity e, [EntityInQueryIndex] int index, in Translation translation)
		{
			var targetVector = Position - translation.Value;
			var time         = math.length(targetVector) * Speed;

			Ecb.AddComponent(index, e, new MoveToComponent
			                           {
				                           From        = translation.Value,
				                           To          = Position,
				                           TotalTime   = time
			                           });
		}
	}

	[BurstCompile]
	public partial struct RestartMoveToJob : IJobEntity
	{
		public float                              Speed;
		public float3                             Position;
		public EntityCommandBuffer.ParallelWriter Ecb;

		private void Execute(Entity e, [EntityInQueryIndex] int index, in Translation translation)
		{
			var targetVector = Position - translation.Value;
			var time         = math.length(targetVector) * Speed;

			Ecb.SetComponent(index, e, new MoveToComponent
			                           {
				                           From        = translation.Value,
				                           To          = Position,
				                           TotalTime   = time
			                           });
		}
	}
}