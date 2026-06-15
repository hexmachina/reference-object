using Unity.Properties;
using UnityEngine;
namespace TW.ReferenceObjects
{

	[CreateAssetMenu(menuName = "Data/Reference/Bool"), GeneratePropertyBag]
	public partial class ReferenceBool : ReferenceBase<bool>
	{
		[field: SerializeField, CreateProperty]
		public bool BindBoolValue
		{
			get;
			set;
		}
		[field: SerializeField, CreateProperty]
		public bool Sample { get; set; }
		public void Toggle()
		{
			Value = !Value;
		}
	}
}