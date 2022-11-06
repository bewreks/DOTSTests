using System;
using Components;
using Events;
using Groups;
using Jobs;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;

namespace Systems
{
	[UpdateInGroup(typeof(InputGroup))]
	[UpdateAfter(typeof(UserInputSystem))]
	public partial class PointerSystem : SystemBase
	{
		private EntityQuery _markersQuery;

		private EndInitializationEntityCommandBufferSystem _bufferSystem;

		protected override void OnCreate()
		{
			_markersQuery = GetEntityQuery(new EntityQueryDesc
			                               {
				                               All = new ComponentType[]
				                                     {
					                                     typeof(PointerMarker),
					                                     typeof(Translation)
				                                     }
			                               });

			_bufferSystem = World.GetOrCreateSystem<EndInitializationEntityCommandBufferSystem>();

			RequireSingletonForUpdate<UserClickEvent>();
		}

		protected override void OnUpdate()
		{
			var userClickEvent = GetSingleton<UserClickEvent>();
			var position = userClickEvent.Position;;

			Dependency = new SetEntityPositionJob
			             {
				             Position = position
			             }.ScheduleParallel(_markersQuery, Dependency);

			Dependency = new AddFadeOutComponentsJob
			             {
				             Speed = 0.2f,
				             Ecb   = _bufferSystem.CreateCommandBuffer().AsParallelWriter()
			             }.ScheduleParallel(_markersQuery, Dependency);
			_bufferSystem.AddJobHandleForProducer(Dependency);
		}
	}
}