using Components;
using Groups;
using Unity.Entities;
using Unity.Transforms;

namespace Systems
{
	[UpdateInGroup(typeof(UpdateGroup))]
	public partial class FadeOutSystem : SystemBase
	{
		private EntityQuery _fadeOutQuery;

		private EndSimulationEntityCommandBufferSystem _endSimulationEntityCommandBufferSystem;

		protected override void OnCreate()
		{
			_fadeOutQuery = GetEntityQuery(new EntityQueryDesc
			                               {
				                               All = new ComponentType[]
				                                     {
					                                     typeof(NonUniformScale),
					                                     typeof(FadeOutComponent)
				                                     }
			                               });

			_endSimulationEntityCommandBufferSystem = World.GetOrCreateSystem<EndSimulationEntityCommandBufferSystem>();
			RequireForUpdate(_fadeOutQuery);
		}

		protected override void OnUpdate()
		{
			var buffer = _endSimulationEntityCommandBufferSystem.CreateCommandBuffer();
			Dependency = new Jobs.FadeOutJob
			             {
				             Ecb       = buffer.AsParallelWriter(),
				             DeltaTime = Time.DeltaTime
			             }.ScheduleParallel(_fadeOutQuery, Dependency);
			_endSimulationEntityCommandBufferSystem.AddJobHandleForProducer(Dependency);
		}
	}
}