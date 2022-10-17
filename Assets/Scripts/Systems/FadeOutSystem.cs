using Components;
using Unity.Burst;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;

namespace Systems
{
	[UpdateAfter(typeof(UserInputSystem))]
	public partial class FadeOutSystem : SystemBase
	{
		private EntityQuery _fadeOutQuery;

		private EndSimulationEntityCommandBufferSystem _endSimulationEntityCommandBufferSystem;
		private UserInputSystem                        _userInputSystem;

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
			_userInputSystem                        = World.GetOrCreateSystem<UserInputSystem>();
			RequireForUpdate(_fadeOutQuery);
		}

		protected override void OnUpdate()
		{
			var buffer = _endSimulationEntityCommandBufferSystem.CreateCommandBuffer();
			Dependency = new FadeOutJob
			             {
				             Ecb       = buffer.AsParallelWriter(),
				             DeltaTime = Time.DeltaTime
			             }.ScheduleParallel(_fadeOutQuery, Dependency);
			_endSimulationEntityCommandBufferSystem.AddJobHandleForProducer(Dependency);
		}
	}

	[BurstCompile]
	public partial struct FadeOutJob : IJobEntity
	{
		public float                              DeltaTime;
		public EntityCommandBuffer.ParallelWriter Ecb;

		void Execute(Entity                                e,
		             [EntityInQueryIndex] int              index,
		             ref                  NonUniformScale  scale,
		             ref                  FadeOutComponent component)
		{
			component.CurTime += DeltaTime;
			var ratio = component.CurTime / component.Time;

			scale.Value = math.lerp(component.From, component.To, math.clamp(ratio, 0, 1));

			if (ratio >= 1)
			{
				Ecb.RemoveComponent<FadeOutComponent>(index, e);
			}
		}
	}
}