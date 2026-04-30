using UnityEngine;
namespace TW.ReferenceObjects
{

	[CreateAssetMenu(menuName = "Data/Reference/Bool")]
	public class ReferenceBool : ReferenceBase<bool>
	{
		public void Toggle()
		{
			Value = !Value;
		}
	}
}