using Components;
using Unity.Entities;
using UnityEngine;

namespace Authorings
{
	public class ButtonAuthoring : MonoBehaviour, IConvertGameObjectToEntity
	{
		public void Convert(Entity entity, EntityManager dstManager, GameObjectConversionSystem conversionSystem)
		{
			dstManager.AddComponent<ButtonMarker>(entity);
		}
	}
}