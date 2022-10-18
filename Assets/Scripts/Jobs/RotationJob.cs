using Components;
using Unity.Burst;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;

namespace Jobs
{
	[BurstCompile]
	public partial struct RotationJob : IJobEntity
	{
		public float DeltaTime;

		public EntityCommandBuffer.ParallelWriter Ecb;

		private void Execute(Entity e, [EntityInQueryIndex] int index, ref Rotation rotation, ref RotateToComponent rotateTo)
		{
			rotateTo.CurrentTime += DeltaTime;

			if (rotateTo.CurrentTime >= rotateTo.TotalTime)
			{
				rotation.Value = rotateTo.To;
				Ecb.RemoveComponent<RotateToComponent>(index, e);
			}
			else
			{
				rotation.Value = math.slerp(rotateTo.From, rotateTo.To, rotateTo.CurrentTime / rotateTo.TotalTime);
			}
		}
	}
}