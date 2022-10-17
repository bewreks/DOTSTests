using Events;
using Unity.Collections;
using Unity.Entities;

namespace Systems
{
	[UpdateInGroup(typeof(PresentationSystemGroup), OrderLast = true)]
	[UpdateAfter(typeof(PointerSystem))]
	public partial class EventsSystem : SystemBase
	{
		private EntityQuery _eventsQuery;

		protected override void OnCreate()
		{
			RequireForUpdate(_eventsQuery);
		}

		public Entity CreateEventEntity<T>(ref T component)
			where T : struct, IComponentData
		{
			var eventEntity = EntityManager.CreateEntity();
			EntityManager.AddComponentData(eventEntity, component);
			return eventEntity;
		}

		protected override void OnUpdate()
		{
			var buffer = new EntityCommandBuffer(Allocator.Temp);
			Entities
				.WithAny<UserClickEvent>()
				.WithStoreEntityQueryInField(ref _eventsQuery)
				.ForEach((Entity e) => { buffer.DestroyEntity(e); }).Run();
			buffer.Playback(EntityManager);
			buffer.Dispose();
		}
	}
}