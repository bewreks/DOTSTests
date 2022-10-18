using Unity.Entities;

namespace Groups
{
	[UpdateInGroup(typeof(InitializationSystemGroup))]
	public class InputGroup : ComponentSystemGroup
	{
		
	}
}