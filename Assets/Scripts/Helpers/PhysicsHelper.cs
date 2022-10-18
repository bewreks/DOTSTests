using System;
using Unity.Physics;
using Ray = UnityEngine.Ray;

namespace Helpers
{
	public static class PhysicsHelper
	{
		public static bool CastRay(this Ray ray, float distance, ref CollisionWorld collisionWorld, out RaycastHit hit)
		{
			var input = new RaycastInput
			            {
				            Start = ray.origin,
				            End   = ray.GetPoint(distance),
				            Filter = new CollisionFilter
				                     {
					                     BelongsTo    = (uint)PhysicsLayers.Selection,
					                     CollidesWith = (uint)PhysicsLayers.Ground
				                     }
			            };
			return collisionWorld.CastRay(input, out hit);
		}
	}

	[Flags]
	public enum PhysicsLayers
	{
		Selection = 1 << 0,
		Ground    = 1 << 1,
		Unit      = 1 << 2
	}
}