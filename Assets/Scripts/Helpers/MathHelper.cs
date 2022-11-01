using Unity.Mathematics;
using UnityEngine;

namespace Helpers
{
	public static class MathHelper
	{
		public const float HalfPI = math.PI / 2; 
		
		public static float AngleSigned(float3 from, float3 to, float3 axis)
		{
			var angle = math.acos(math.dot(math.normalize(from), math.normalize(to)));
			var sign  = math.sign(math.dot(axis,                 math.cross(from, to)));
			return angle * sign;
		}
		
		public static float3 MoveTowards(float3 current, float3 target, float maxDistanceDelta)
		{
			var delta  = target - current;
			var sqdist = math.lengthsq(delta);
			if (sqdist == 0 || sqdist <= maxDistanceDelta * maxDistanceDelta)
				return target;
			var dist = math.sqrt(sqdist);
			return current + delta * (maxDistanceDelta / dist);
		}
	}
}