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
	}
}