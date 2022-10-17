using Unity.Entities;
using Unity.Mathematics;

namespace Events
{
	public struct UserClickEvent : IComponentData
	{
		public float3 Position;
	}
}