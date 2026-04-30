using UnityEngine;

namespace TW.ReferenceObjects
{

	[CreateAssetMenu(menuName = "Data/Reference/Int")]
	public class ReferenceInt : ReferenceIntBase
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
	}

	public abstract class ReferenceIntBase : ReferenceBase<int>
	{
		public override string ToString()
		{
			return Value.ToString();
		}
	}
}

