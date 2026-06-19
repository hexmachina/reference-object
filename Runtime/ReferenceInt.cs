using Unity.Properties;
using UnityEngine;

namespace TW.ReferenceObjects
{

	[CreateAssetMenu(menuName = "Data/Reference/Int"), GeneratePropertyBag]
	public partial class ReferenceInt : ReferenceIntBase
	{
		public virtual void Increment()
		{
			Value++;
		}
		public virtual void Decrement() { Value--; }

		public virtual void Toggle(bool toggle)
		{
			if (toggle)
			{
				Value++;
			}
			else
			{
				Value--;
			}
		}

		[CreateProperty]
		public bool IsPositive => Value >= 0;
		[CreateProperty]
		public bool IsNegative => Value < 0;
	}

	public abstract class ReferenceIntBase : ReferenceBase<int>
	{
		public override string ToString()
		{
			return Value.ToString();
		}
	}
}

