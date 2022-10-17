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
		public FadeOutComponent                   Prototype;
		public EntityCommandBuffer.ParallelWriter Ecb;

		void Execute(Entity e, [EntityInQueryIndex] int index)
		{
			Ecb.AddComponent(index, e, Prototype);
			Ecb.AddComponent(index, e, new NonUniformScale
			                           {
				                           Value = float3.zero
			                           });
		}
	}
}