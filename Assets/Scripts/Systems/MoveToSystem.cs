using Components;
using Unity.Burst;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;

namespace Systems
{
	[UpdateInGroup(typeof(InitializationSystemGroup))]
	public partial class MoveToSystem : SystemBase
	{
		private EntityQuery _movables;

		private EndInitializationEntityCommandBufferSystem _commandBufferSystem;

		protected override void OnCreate()
		{
			_movables = GetEntityQuery(new EntityQueryDesc
			                           {
				                           All = new ComponentType[]
				                                 {
					                                 typeof(MoveToComponent),
					                                 typeof(Translation)
				                                 }
			                           });

			_commandBufferSystem = World.GetOrCreateSystem<EndInitializationEntityCommandBufferSystem>();

			RequireForUpdate(_movables);
		}

		protected override void OnUpdate()
		{
			var buffer = _commandBufferSystem.CreateCommandBuffer();
			Dependency = new MovingJob
			             {
				             Ecb = buffer.AsParallelWriter(),
				             DeltaTime = Time.DeltaTime
			             }.ScheduleParallel(_movables, Dependency);
			_commandBufferSystem.AddJobHandleForProducer(Dependency);
		}
	}

	[BurstCompile]
	public partial struct MovingJob : IJobEntity
	{
		public float DeltaTime;

		public EntityCommandBuffer.ParallelWriter Ecb;

		private void Execute(Entity e, [EntityInQueryIndex] int index, ref Translation translation, ref MoveToComponent moveTo)
		{
			moveTo.CurrentTime += DeltaTime;

			if (moveTo.CurrentTime >= moveTo.TotalTime)
			{
				translation.Value = moveTo.To;
				Ecb.RemoveComponent<MoveToComponent>(index, e);
			}
			else
			{
				translation.Value = math.lerp(moveTo.From, moveTo.To, moveTo.CurrentTime / moveTo.TotalTime);
			}
		}
	}
}