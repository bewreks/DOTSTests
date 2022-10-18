using Unity.Entities;

namespace Groups
{
	
	[UpdateInGroup(typeof(SimulationSystemGroup))]
	public class UpdateGroup : ComponentSystemGroup
	{
		
	}
}