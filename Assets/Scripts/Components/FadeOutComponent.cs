using Unity.Entities;
using Unity.Mathematics;

namespace Components
{
	public struct FadeOutComponent : IComponentData
	{
		public float3 From;
		public float3 To;
		public float  Time;
		public float  CurTime;
	}
}