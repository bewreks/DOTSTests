using Components;
using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;

namespace Jobs
{
	[BurstCompile]
	public partial struct CheckLessDistanceJob : IJobEntity
	{
		public            NativeArray<Translation>             Positions;
		public            float                                Distance;
		public            EntityCommandBuffer.ParallelWriter   Ecb;
		[ReadOnly] public ComponentDataFromEntity<DoorMarker>  DoorMarker;
		[ReadOnly] public ComponentDataFromEntity<Translation> DoorPositions;

		private void Execute([EntityInQueryIndex] int index,
		                     Entity                   e,
		                     in Translation           translation,
		                     in ButtonMarker          button)
		{
			var distance = math.distance(Positions[index].Value, translation.Value);

			if (distance <= Distance)
			{
				Ecb.AddComponent<ButtonPressedMarker>(index, e);
				Ecb.AddComponent(index, button.Door, MoveToComponent.New(DoorPositions[button.Door].Value,
				                                                         DoorMarker[button.Door].Bottom,
				                                                         1));
			}
		}
	}
}