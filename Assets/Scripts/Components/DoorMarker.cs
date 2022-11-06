using Unity.Entities;
using Unity.Mathematics;

namespace Components
{
	
	[GenerateAuthoringComponent]
	public struct DoorMarker : IComponentData
	{
		public float3 Bottom;
	}
}