using Components;
using Unity.Burst;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;

namespace Jobs
{
	[BurstCompile]
	public partial struct AddFadeOutComponentsJob : IJobEntity
	{
		public EntityCommandBuffer.ParallelWriter Ecb;

		public float Speed;

		void Execute(Entity e, [EntityInQueryIndex] int index)
		{
			Ecb.AddComponent(index, e, FadeOutComponent.New(Speed));
			Ecb.AddComponent(index, e, new NonUniformScale
			                           {
				                           Value = float3.zero
			                           });
		}
	}
}