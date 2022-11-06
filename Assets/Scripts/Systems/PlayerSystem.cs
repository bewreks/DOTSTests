using Components;
using Events;
using Groups;
using Jobs;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;

namespace Systems
{
	[UpdateInGroup(typeof(InputGroup))]
	[UpdateAfter(typeof(PointerSystem))]
	public partial class PlayerSystem : SystemBase
	{
		private EntityQuery _playerQuery;

		private EndInitializationEntityCommandBufferSystem _bufferSystem;

		protected override void OnCreate()
		{
			_playerQuery = GetEntityQuery(new EntityQueryDesc
			                              {
				                              All = new ComponentType[]
				                                    {
					                                    typeof(PlayerMarker),
					                                    typeof(Translation),
					                                    typeof(Rotation)
				                                    },
			                              });

			_bufferSystem = World.GetOrCreateSystem<EndInitializationEntityCommandBufferSystem>();

			RequireSingletonForUpdate<UserClickEvent>();
		}

		protected override void OnUpdate()
		{
			var position = GetSingleton<UserClickEvent>().Position;

			Dependency = new StartRotateToJob
			             {
				             Ecb      = _bufferSystem.CreateCommandBuffer().AsParallelWriter(),
				             Position = position,
				             Speed    = .01f
			             }.ScheduleParallel(_playerQuery, Dependency);


			Dependency = new StartMoveToJob
			             {
				             Ecb      = _bufferSystem.CreateCommandBuffer().AsParallelWriter(),
				             Position = position,
				             Speed    = 0.1f
			             }.ScheduleParallel(_playerQuery, Dependency);
			_bufferSystem.AddJobHandleForProducer(Dependency);
		}
	}
}