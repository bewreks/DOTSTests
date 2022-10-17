using Components;
using Events;
using Unity.Burst;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;

namespace Systems
{
	[UpdateInGroup(typeof(InitializationSystemGroup))]
	[UpdateAfter(typeof(UserInputSystem))]
	public partial class PointerSystem : SystemBase
	{
		private EntityQuery _userClickEvent;
		private EntityQuery _markersQuery;
		private EntityQuery _animatedMarkersQuery;
		private EntityQuery _notAnimatedMarkersQuery;

		private EndSimulationEntityCommandBufferSystem _endSimulationEntityCommandBufferSystem;

		protected override void OnCreate()
		{
			_userClickEvent = GetEntityQuery(new EntityQueryDesc
			                                 {
				                                 All = new ComponentType[] { typeof(UserClickEvent) }
			                                 });

			_endSimulationEntityCommandBufferSystem = World.GetOrCreateSystem<EndSimulationEntityCommandBufferSystem>();

			_markersQuery = GetEntityQuery(new EntityQueryDesc
			                               {
				                               All = new ComponentType[]
				                                     {
					                                     typeof(MarkerComponent),
					                                     typeof(Translation)
				                                     }
			                               });

			_animatedMarkersQuery = GetEntityQuery(new EntityQueryDesc
			                                       {
				                                       All = new ComponentType[]
				                                             {
					                                             typeof(MarkerComponent),
					                                             typeof(FadeOutComponent)
				                                             }
			                                       });

			_notAnimatedMarkersQuery = GetEntityQuery(new EntityQueryDesc
			                                          {
				                                          All  = new ComponentType[] { typeof(MarkerComponent) },
				                                          None = new ComponentType[] { typeof(FadeOutComponent) }
			                                          });

			RequireForUpdate(_userClickEvent);
		}

		protected override void OnUpdate()
		{
			var position = float3.zero;
			var ecb      = _endSimulationEntityCommandBufferSystem.CreateCommandBuffer();

			Entities
				.WithAll<UserClickEvent>()
				.ForEach((ref UserClickEvent click) => { position = click.Position; }).Run();

			var updateMarkersPositionJob = new UpdateMarkersPositionJob
			                               {
				                               Position = position
			                               };
			var startMarkerAnimationJob = new StartMarkerAnimationJob
			                              {
				                              Ecb       = ecb.AsParallelWriter(),
				                              Prototype = NewFadeComponent()
			                              };
			var restartMarkerAnimationJob = new RestartMarkerAnimationJob
			                                {
				                                Ecb       = ecb.AsParallelWriter(),
				                                Prototype = NewFadeComponent()
			                                };

			Dependency = updateMarkersPositionJob.ScheduleParallel(_markersQuery, Dependency);

			if (!_notAnimatedMarkersQuery.IsEmpty)
			{
				Dependency = startMarkerAnimationJob.ScheduleParallel(_notAnimatedMarkersQuery, Dependency);
				_endSimulationEntityCommandBufferSystem.AddJobHandleForProducer(Dependency);
			}

			if (!_animatedMarkersQuery.IsEmpty)
			{
				Dependency = restartMarkerAnimationJob.ScheduleParallel(_animatedMarkersQuery, Dependency);
				_endSimulationEntityCommandBufferSystem.AddJobHandleForProducer(Dependency);
			}

			FadeOutComponent NewFadeComponent()
			{
				return new FadeOutComponent
				       {
					       From    = new float3(1),
					       To      = float3.zero,
					       CurTime = 0,
					       Time    = 0.2f
				       };
			}
		}
	}

	[BurstCompile]
	public partial struct UpdateMarkersPositionJob : IJobEntity
	{
		public float3 Position;

		void Execute(ref Translation translation)
		{
			translation.Value = Position;
		}
	}

	[BurstCompile]
	public partial struct StartMarkerAnimationJob : IJobEntity
	{
		public FadeOutComponent                   Prototype;
		public EntityCommandBuffer.ParallelWriter Ecb;

		void Execute(Entity e, [EntityInQueryIndex] int index)
		{
			Ecb.AddComponent(index, e, Prototype);
			Ecb.AddComponent(index, e, new NonUniformScale
			                           {
				                           Value = float3.zero
			                           });
		}
	}

	[BurstCompile]
	public partial struct RestartMarkerAnimationJob : IJobEntity
	{
		public FadeOutComponent                   Prototype;
		public EntityCommandBuffer.ParallelWriter Ecb;

		void Execute(Entity e, [EntityInQueryIndex] int index)
		{
			Ecb.SetComponent(index, e, Prototype);
		}
	}
}