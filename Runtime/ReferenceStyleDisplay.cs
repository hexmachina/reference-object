using Unity.Properties;
using UnityEngine;
using UnityEngine.UIElements;

namespace TW.ReferenceObjects
{

	[CreateAssetMenu(menuName = "Data/Reference/Style Display"), GeneratePropertyBag]
	public partial class ReferenceStyleDisplay : ReferenceBase<StyleEnum<DisplayStyle>>
	{
		public void Toggle()
		{
			_value.value = _value.value == DisplayStyle.Flex ? DisplayStyle.None : DisplayStyle.Flex;
		}

		public void SetDisplay(bool value)
		{
			_value.value = value ? DisplayStyle.Flex : DisplayStyle.None;

		}
	}
}
