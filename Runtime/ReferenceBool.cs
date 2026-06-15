using Unity.Properties;
using UnityEngine;
namespace TW.ReferenceObjects
{

	[CreateAssetMenu(menuName = "Data/Reference/Bool"), GeneratePropertyBag]
	public partial class ReferenceBool : ReferenceBase<bool>
	{
		[CreateProperty]
		public bool BindBoolValue => _value;
		[CreateProperty]
		public bool Sample { get; set; }
		public void Toggle()
		{
			Value = !Value;
		}
	}
}