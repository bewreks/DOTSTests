using Unity.Mathematics;

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
		
		// public static float AngleSigned(float3 from, float3 to, float3 axis)
		// {
		// 	float num1 = Angle(from, to);
		// 	float num2 = from.y * to.z - from.z * to.y;
		// 	float num3 = from.z * to.x - from.x * to.z;
		// 	float num4 = from.x * to.y - from.y * to.x;
		// 	float num5 = math.sign(axis.x * num2 +
		// 	                       axis.y * num3 +
		// 	                       axis.z * num4);
		// 	return num1 * num5;
		// }
		//
		// public static float Angle(float3 from, float3 to)
		// {
		// 	var num = math.sqrt(math.length(from) * math.length(to));
		// 	return num < 1.0000000036274937E-15
		// 		       ? 0.0f
		// 		       : math.acos(math.clamp(math.dot(from, to) / num, -1f, 1f)) * 57.29578f;
		// }
	}
}