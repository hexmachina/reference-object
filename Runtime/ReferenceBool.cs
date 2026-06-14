using Unity.Properties;
using UnityEngine;
namespace TW.ReferenceObjects
{

	[CreateAssetMenu(menuName = "Data/Reference/Bool"), GeneratePropertyBag]
	public class ReferenceBool : ReferenceBase<bool>
	{
		[CreateProperty]
		public bool BindBoolValue => _value;
		public void Toggle()
		{
			Value = !Value;
		}
	}
}