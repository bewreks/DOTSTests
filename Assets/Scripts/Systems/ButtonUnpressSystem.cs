using Components;
using Groups;
using Unity.Collections;
using Unity.Entities;
using Unity.Transforms;

namespace Systems
{
	[UpdateInGroup(typeof(UpdateGroup))]
	[UpdateAfter(typeof(ButtonSystem))]
	public partial class ButtonUnpressSystem : SystemBase
	{
		private EntityQuery _playerQuery;
		private EntityQuery _buttonsQuery;

		private EndInitializationEntityCommandBufferSystem _commandBufferSystem;

		protected override void OnCreate()
		{
			_playerQuery = GetEntityQuery(new EntityQueryDesc
			                              {
				                              All = new ComponentType[]
				                                    {
					                                    typeof(PlayerMarker),
					                                    typeof(Translation)
				                                    }
			                              });

			_buttonsQuery = GetEntityQuery(new EntityQueryDesc
			                               {
				                               All = new ComponentType[]
				                                     {
					                                     typeof(ButtonMarker),
					                                     typeof(Translation),
					                                     typeof(ButtonPressedMarker)
				                                     }
			                               });

			_commandBufferSystem = World.GetOrCreateSystem<EndInitializationEntityCommandBufferSystem>();

			RequireForUpdate(_buttonsQuery);
		}

		protected override void OnUpdate()
		{
			var playerPositions = _playerQuery.ToComponentDataArray<Translation>(Allocator.TempJob);

			Dependency = new Jobs.CheckMoreDistanceJob
			             {
				             Positions = playerPositions,
				             Distance  = 1,
				             Ecb       = _commandBufferSystem.CreateCommandBuffer().AsParallelWriter()
			             }.ScheduleParallel(_buttonsQuery, Dependency);

			Dependency = playerPositions.Dispose(Dependency);
		}
	}
}