using Components;
using Unity.Burst;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;

namespace Jobs
{
	[BurstCompile]
	public partial struct StartRotateToJob : IJobEntity
	{
		public EntityCommandBuffer.ParallelWriter Ecb;

		public float3 Position;
		public float  Speed;

		private void Execute(Entity e, [EntityInQueryIndex] int index, in Rotation rotation, in Translation translation)
		{
			Ecb.AddComponent(index, e, RotateToComponent.New(rotation.Value, translation.Value, Position, Speed));
		}
	}
}