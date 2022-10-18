using Components;
using Groups;
using Unity.Entities;
using Unity.Transforms;

namespace Systems
{
	
	[UpdateInGroup(typeof(UpdateGroup))]
	public partial class RotateToSystem : SystemBase
	{
		private EntityQuery _rotates;

		private EndInitializationEntityCommandBufferSystem _commandBufferSystem;

		protected override void OnCreate()
		{
			_rotates = GetEntityQuery(new EntityQueryDesc
			                           {
				                           All = new ComponentType[]
				                                 {
					                                 typeof(RotateToComponent),
					                                 typeof(Translation)
				                                 }
			                           });

			_commandBufferSystem = World.GetOrCreateSystem<EndInitializationEntityCommandBufferSystem>();

			RequireForUpdate(_rotates);
		}

		protected override void OnUpdate()
		{
			var buffer = _commandBufferSystem.CreateCommandBuffer();
			Dependency = new Jobs.RotationJob
			             {
				             Ecb       = buffer.AsParallelWriter(),
				             DeltaTime = Time.DeltaTime
			             }.ScheduleParallel(_rotates, Dependency);
			_commandBufferSystem.AddJobHandleForProducer(Dependency);
		}
	}
}