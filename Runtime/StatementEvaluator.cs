using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace TW.ReferenceObjects
{

	public class StatementEvaluator : MonoBehaviour
	{

		public List<ReferenceBool> statements = new List<ReferenceBool>();

		public List<bool> conditions = new List<bool>();
		public List<int> bits = new List<int>();

		public List<UnityEvent> events = new List<UnityEvent>();

		private void Awake()
		{
			for (int i = 0; i < statements.Count; i++)
			{
				statements[i].OnValueChanged += OnValueChanged;
			}
		}

		private void OnDestroy()
		{
			for (int i = 0; i < statements.Count; i++)
			{
				statements[i].OnValueChanged -= OnValueChanged;
			}
		}

		private void OnValueChanged(bool obj)
		{
			Evaluate();
		}

		public void Evaluate()
		{
			var statement = GetStatementMask();
			for (int i = 0; i < bits.Count; i++)
			{
				if (!conditions[i])
				{
					if (statement == bits[i])
					{
						events[i].Invoke();
					}
				}
				else
				{
					if (HasFlag(statement, bits[i]))
					{
						events[i].Invoke();
					}
				}
			}
		}

		public int GetStatementMask()
		{
			int bits = 0;
			for (int i = 0; i < statements.Count; i++)
			{
				if (statements[i].Value)
				{
					bits += (int)Mathf.Pow(2, i);
				}
			}
			return bits;
		}

		//private bool CheckBools()
		//{
		//	for (int i = 0; i < bools.Count; i++)
		//	{
		//		if (!bools[i])
		//		{
		//			return false;
		//		}
		//	}
		//	return true;
		//}

		public bool HasFlag(int a, int b)
		{
			return (a & b) == b;
		}
	}
}
