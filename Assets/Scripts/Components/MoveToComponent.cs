using Unity.Entities;
using Unity.Mathematics;

namespace Components
{
	public struct MoveToComponent : IComponentData
	{
		public float3 From;
		public float3 To;
		public float  TotalTime;
		public float  CurrentTime;

		public static MoveToComponent New(float3 from, float3 to, float speed)
		{
			var targetVector = to - from;
			var time         = math.length(targetVector) * speed;
			return new MoveToComponent
			       {
				       From        = from,
				       To          = to,
				       TotalTime   = time,
				       CurrentTime = 0
			       };
		}
	}
}