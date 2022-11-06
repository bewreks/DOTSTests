using Components;
using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;

namespace Jobs
{
	[BurstCompile]
	public partial struct CheckMoreDistanceJob : IJobEntity
	{
		public NativeArray<Translation>           Positions;
		public float                              Distance;
		public EntityCommandBuffer.ParallelWriter Ecb;

		private void Execute([EntityInQueryIndex] int index, Entity e, in Translation translation)
		{
			var distance = math.distance(Positions[index].Value, translation.Value);

			if (distance > Distance)
			{
				Ecb.RemoveComponent<ButtonPressedMarker>(index, e);
			}
		}
	}
}