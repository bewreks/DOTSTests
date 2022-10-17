using Components;
using Unity.Burst;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;

namespace Jobs
{
	[BurstCompile]
	public partial struct MovingJob : IJobEntity
	{
		public float DeltaTime;

		public EntityCommandBuffer.ParallelWriter Ecb;

		private void Execute(Entity e, [EntityInQueryIndex] int index, ref Translation translation, ref MoveToComponent moveTo)
		{
			moveTo.CurrentTime += DeltaTime;

			if (moveTo.CurrentTime >= moveTo.TotalTime)
			{
				translation.Value = moveTo.To;
				Ecb.RemoveComponent<MoveToComponent>(index, e);
			}
			else
			{
				translation.Value = math.lerp(moveTo.From, moveTo.To, moveTo.CurrentTime / moveTo.TotalTime);
			}
		}
	}
}