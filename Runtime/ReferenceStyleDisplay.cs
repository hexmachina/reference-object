using Unity.Properties;
using UnityEngine;
using UnityEngine.UIElements;

namespace TW.ReferenceObjects
{

	[CreateAssetMenu(menuName = "Data/Reference/Style Display"), GeneratePropertyBag]
	public partial class ReferenceStyleDisplay : ReferenceBase<DisplayStyle>
	{
	}
}
