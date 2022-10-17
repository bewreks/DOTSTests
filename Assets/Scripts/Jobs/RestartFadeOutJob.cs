using Components;
using Unity.Burst;
using Unity.Entities;

namespace Jobs
{
	[BurstCompile]
	public partial struct RestartFadeOutJob : IJobEntity
	{
		public FadeOutComponent                   Prototype;
		public EntityCommandBuffer.ParallelWriter Ecb;

		void Execute(Entity e, [EntityInQueryIndex] int index)
		{
			Ecb.SetComponent(index, e, Prototype);
		}
	}
}