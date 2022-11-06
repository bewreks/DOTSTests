using Components;
using Groups;
using Unity.Collections;
using Unity.Entities;
using Unity.Transforms;
using UnityEngine;

namespace Systems
{
	[UpdateInGroup(typeof(UpdateGroup))]
	[UpdateAfter(typeof(MoveToSystem))]
	public partial class ButtonPressSystem : SystemBase
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
					                                     typeof(Translation)
				                                     },
				                               None = new ComponentType[]
				                                      {
					                                      typeof(ButtonPressedMarker),
					                                      typeof(DoorOpenedMarker)
				                                      }
			                               });

			_commandBufferSystem = World.GetOrCreateSystem<EndInitializationEntityCommandBufferSystem>();

			RequireForUpdate(_buttonsQuery);
		}

		protected override void OnUpdate()
		{
			var playerPositions = _playerQuery.ToComponentDataArray<Translation>(Allocator.TempJob);

			var componentDataFromEntity = GetComponentDataFromEntity<DoorMarker>();
			Dependency = new Jobs.CheckLessDistanceJob
			             {
				             Positions     = playerPositions,
				             Distance      = 1,
				             Ecb           = _commandBufferSystem.CreateCommandBuffer().AsParallelWriter(),
				             DoorMarker    = componentDataFromEntity,
				             DoorPositions = GetComponentDataFromEntity<Translation>()
			             }.ScheduleParallel(_buttonsQuery, Dependency);

			Dependency = playerPositions.Dispose(Dependency);
		}
	}
}