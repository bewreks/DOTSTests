using Unity.Entities;
using Unity.Mathematics;

namespace Components
{
	public struct RotateToComponent : IComponentData
	{
		public quaternion From;
		public quaternion To;
		public float      TotalTime;
		public float      CurrentTime;

		public static RotateToComponent New(quaternion from, float3 position, float3 rotateTo, float speed)
		{
			var direction     = math.normalize(rotateTo.xz - position.xz);
			var finalRotation = quaternion.LookRotation(new float3(direction.x, 0, direction.y), math.up());

			var acos = math.acos(math.clamp(math.dot(from, finalRotation), 0, 1));

			return new RotateToComponent
			       {
				       From      = from,
				       To        = finalRotation,
				       TotalTime = speed * acos
			       };
		}
	}
}