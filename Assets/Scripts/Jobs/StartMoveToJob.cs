using Components;
using Unity.Burst;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;

namespace Jobs
{
	[BurstCompile]
	public partial struct StartMoveToJob : IJobEntity
	{
		public float                              Speed;
		public float3                             Position;
		public EntityCommandBuffer.ParallelWriter Ecb;

		private void Execute(Entity e, [EntityInQueryIndex] int index, in Translation translation)
		{
			Ecb.AddComponent(index, e, MoveToComponent.New(translation.Value, Position, Speed));
		}
	}
}