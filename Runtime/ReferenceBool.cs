using Unity.Properties;
using UnityEngine;
namespace TW.ReferenceObjects
{

	[CreateAssetMenu(menuName = "Data/Reference/Bool"), GeneratePropertyBag]
	public partial class ReferenceBool : ReferenceBase<bool>
	{
		[CreateProperty]
		public bool InvertValue => !Value;
		public void Toggle()
		{
			Value = !Value;
		}
	}
}