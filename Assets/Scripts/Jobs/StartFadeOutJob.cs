using Components;
using Unity.Burst;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;

namespace Jobs
{
	[BurstCompile]
	public partial struct StartFadeOutJob : IJobEntity
	{
		public float Speed;

		public EntityCommandBuffer.ParallelWriter Ecb;

		void Execute(Entity e, [EntityInQueryIndex] int index)
		{
			Ecb.AddComponent(index, e, FadeOutComponent.New(Speed));
			Ecb.AddComponent(index, e, new NonUniformScale
			                           {
				                           Value = float3.zero
			                           });
		}
	}
}