using Components;
using Unity.Burst;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;

namespace Jobs
{
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