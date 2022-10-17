using Components;
using Unity.Entities;
using Unity.Transforms;

namespace Systems
{
	[UpdateInGroup(typeof(SimulationSystemGroup))]
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
			Dependency = new Jobs.MovingJob
			             {
				             Ecb = buffer.AsParallelWriter(),
				             DeltaTime = Time.DeltaTime
			             }.ScheduleParallel(_movables, Dependency);
			_commandBufferSystem.AddJobHandleForProducer(Dependency);
		}
	}
}