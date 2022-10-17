using Unity.Burst;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;

namespace Jobs
{
	[BurstCompile]
	public partial struct SetEntityPositionJob : IJobEntity
	{
		public float3 Position;

		void Execute(ref Translation translation)
		{
			translation.Value = Position;
		}
	}
}