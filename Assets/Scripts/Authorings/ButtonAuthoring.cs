using Components;
using Unity.Entities;
using UnityEngine;

namespace Authorings
{
	public class ButtonAuthoring : MonoBehaviour, IConvertGameObjectToEntity
	{
		[SerializeField] private GameObject door;
		
		public void Convert(Entity entity, EntityManager dstManager, GameObjectConversionSystem conversionSystem)
		{
			var buttonMarker = new ButtonMarker
			                   {
				                   Door = conversionSystem.GetPrimaryEntity(door)
			                   };
			dstManager.AddComponentData(entity, buttonMarker);
		}
	}
}