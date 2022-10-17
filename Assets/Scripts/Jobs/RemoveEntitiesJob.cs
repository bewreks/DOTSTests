using Unity.Burst;
using Unity.Entities;

namespace Jobs
{
	[BurstCompile]
	public struct RemoveEntitiesJob : IJobEntity
	{
		public EntityCommandBuffer.ParallelWriter Ecb;

		public void Execute(Entity e, [EntityInQueryIndex] int index)
		{
			Ecb.DestroyEntity(index, e);
		}
	}
}