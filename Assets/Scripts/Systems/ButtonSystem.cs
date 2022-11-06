using Components;
using Groups;
using Unity.Entities;
using UnityEngine;

namespace Systems
{
	
	[UpdateInGroup(typeof(UpdateGroup))]
	[UpdateAfter(typeof(ButtonPressSystem))]
	[UpdateBefore(typeof(ButtonPressSystem))]
	[DisableAutoCreation]
	public partial class ButtonSystem : SystemBase
	{
		private EntityQuery _buttonsQuery;

		protected override void OnCreate()
		{
			_buttonsQuery = GetEntityQuery(new EntityQueryDesc
			                               {
				                               All = new ComponentType[]
				                                     {
					                                     typeof(ButtonMarker),
					                                     typeof(ButtonPressedMarker)
				                                     }
			                               });
			
			RequireForUpdate(_buttonsQuery);
		}

		protected override void OnUpdate()
		{
			Dependency = new ButtonsPressedJob().ScheduleParallel(_buttonsQuery, Dependency);
		}
	}
	
	public partial struct ButtonsPressedJob : IJobEntity
	{
		private void Execute(in ButtonMarker marker)
		{
			Debug.Log($"button pressed");
		}
	}
}