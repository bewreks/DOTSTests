using Events;
using Helpers;
using Unity.Entities;
using Unity.Physics.Systems;
using UnityEngine;

namespace Systems
{
	[UpdateInGroup(typeof(InitializationSystemGroup), OrderFirst = true)]
	[UpdateBefore(typeof(PointerSystem))]
	public partial class UserInputSystem : SystemBase
	{
		private BuildPhysicsWorld _buildPhysicsWorld;

		private EventsSystem _eventsSystem;

		protected override void OnCreate()
		{
			_eventsSystem      = World.GetOrCreateSystem<EventsSystem>();
			_buildPhysicsWorld = World.GetOrCreateSystem<BuildPhysicsWorld>();
		}

		protected override void OnUpdate()
		{
			if (Input.GetMouseButtonDown(0))
			{
				var ray = Camera.main.ScreenPointToRay(Input.mousePosition);

				if (ray.CastRay(100f, ref _buildPhysicsWorld.PhysicsWorld.CollisionWorld, out var hit))
				{
					var userClickEvent = new UserClickEvent
					                     {
						                     Position = hit.Position
					                     };
					_eventsSystem.CreateEventEntity(ref userClickEvent);
				}
			}
		}
	}
}