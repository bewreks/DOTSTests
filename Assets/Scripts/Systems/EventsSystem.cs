using Events;
using Jobs;
using Unity.Entities;

namespace Systems
{
	[UpdateInGroup(typeof(PresentationSystemGroup), OrderLast = true)]
	public partial class EventsSystem : SystemBase
	{
		private EntityQuery                                  _eventsQuery;
		private BeginInitializationEntityCommandBufferSystem _commandBufferSystem;

		protected override void OnCreate()
		{
			_eventsQuery = GetEntityQuery(new EntityQueryDesc
			                              {
				                              Any = new ComponentType[]
				                                    {
					                                    typeof(UserClickEvent)
				                                    }
			                              });

			_commandBufferSystem = World.GetOrCreateSystem<BeginInitializationEntityCommandBufferSystem>();

			RequireForUpdate(_eventsQuery);
		}

		public Entity CreateEventEntity<T>(ref T component)
			where T : struct, IComponentData
		{
			var eventEntity = EntityManager.CreateEntity();
			EntityManager.AddComponentData(eventEntity, component);
			SetSingleton(component);
			return eventEntity;
		}

		protected override void OnUpdate()
		{
			var buffer = _commandBufferSystem.CreateCommandBuffer();
			Dependency = new RemoveEntitiesJob
			{
				Ecb = buffer.AsParallelWriter()
			}.ScheduleParallel(_eventsQuery, Dependency);
			_commandBufferSystem.AddJobHandleForProducer(Dependency);
		}
	}
}