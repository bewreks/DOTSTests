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

		public static FadeOutComponent New(float time)
		{
			return new FadeOutComponent
			       {
				       From    = new float3(1),
				       To      = float3.zero,
				       Time    = time,
				       CurTime = 0
			       };
		}
	}
}