using System.Collections.Generic;
using TW.ReferenceObjects;
using UnityEditor;
using UnityEditorInternal;
using UnityEngine;

namespace TW.Editors.ReferenceObjects
{

	[CustomEditor(typeof(StatementEvaluator))]
	public class StatementEvaluatorEditor : Editor
	{
		ReorderableList statementList;
		ReorderableList evaluateList;

		SerializedProperty conditions;
		SerializedProperty statements;
		SerializedProperty bits;
		SerializedProperty events;

		private string[] scriptableBoolNames;

		private string[] conditionDrops = new string[] { "Equals", "Contains" };

		readonly List<ReferenceBool> statementAssets = new List<ReferenceBool>();


		private void OnEnable()
		{
			statements = serializedObject.FindProperty("statements");
			conditions = serializedObject.FindProperty("conditions");
			bits = serializedObject.FindProperty("bits");
			events = serializedObject.FindProperty("events");

			statementList = new ReorderableList(serializedObject, statements, false, false, true, true);
			statementList.drawElementCallback = OnDrawStatementElement;
			statementList.elementHeight = EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;
			statementList.onAddDropdownCallback = OnStatementAddDropdown;
			statementList.onRemoveCallback = RemoveStatementCallback;
			statementList.drawHeaderCallback = OnDrawStatementHeader;

			evaluateList = new ReorderableList(serializedObject, bits, false, false, true, true);
			evaluateList.drawElementCallback = OnDrawEvalElement;
			evaluateList.onAddCallback = AddEvalCallback;
			evaluateList.onRemoveCallback = RemoveEvalCallback;
			evaluateList.elementHeightCallback = ElementHeightEvalCallback;
			evaluateList.drawHeaderCallback = OnDrawEvalHeader;
			//reorderableList.elementHeight = (EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing) * 3;
			UpdateStatementNames();
			Refresh();
		}
		public override void OnInspectorGUI()
		{
			serializedObject.Update();
			//EditorGUILayout.PropertyField(bools);
			EditorGUI.BeginChangeCheck();
			statementList.DoLayoutList();
			if (EditorGUI.EndChangeCheck())
			{
				UpdateStatementNames();
			}
			evaluateList.DoLayoutList();
			serializedObject.ApplyModifiedProperties();
		}

		private void UpdateStatementNames()
		{
			scriptableBoolNames = new string[statements.arraySize];
			for (int i = 0; i < statements.arraySize; i++)
			{
				var el = statements.GetArrayElementAtIndex(i);
				scriptableBoolNames[i] = el.objectReferenceValue.name;
			}
		}

		private void OnDrawStatementHeader(Rect rect)
		{
			EditorGUI.LabelField(rect, "Statements");
		}

		private void OnDrawStatementElement(Rect rect, int index, bool isActive, bool isFocused)
		{

			rect.height = EditorGUIUtility.singleLineHeight;
			var el = statements.GetArrayElementAtIndex(index);
			if (el.objectReferenceValue)
			{
				var stat = el.objectReferenceValue as ReferenceBool;
				//var so = new SerializedObject(el.objectReferenceValue);
				//var val = so.FindProperty("_value");
				if (stat)
				{
					//var x = rect.x;
					//var width = rect.width;
					//rect.x += EditorGUIUtility.labelWidth + 2f;
					EditorGUI.BeginDisabledGroup(true);
					EditorGUI.Toggle(rect, stat.Value);
					EditorGUI.EndDisabledGroup();
					//rect.x = x;
					rect.x += EditorGUIUtility.singleLineHeight + 2;
				}
				EditorGUI.LabelField(rect, new GUIContent(el.objectReferenceValue.name));
			}
		}

		private void RemoveStatementCallback(ReorderableList list)
		{
			var index = list.index;

			statements.DeleteArrayElementAtIndex(index);
			statements.DeleteArrayElementAtIndex(index);
			UnsetBits((int)Mathf.Pow(2, index));
			UpdateStatementNames();

		}

		private void OnStatementAddDropdown(Rect buttonRect, ReorderableList list)
		{
			GenericMenu menu = new GenericMenu();
			for (int i = 0; i < statementAssets.Count; i++)
			{
				if (!ContainsStatement(statementAssets[i]))
				{
					int index = i;
					menu.AddItem(new GUIContent(statementAssets[i].name), false, () => AddStatement(statementAssets[index]));
				}
			}
			menu.ShowAsContext();
		}

		private void AddStatement(ReferenceBool boolEvent)
		{
			var last = statements.arraySize;
			statements.arraySize++;
			var el = statements.GetArrayElementAtIndex(last);
			el.objectReferenceValue = boolEvent;
			serializedObject.ApplyModifiedProperties();
			UpdateStatementNames();

		}

		private bool ContainsStatement(ReferenceBool boolEvent)
		{
			for (int i = 0; i < statements.arraySize; i++)
			{
				var el = statements.GetArrayElementAtIndex(i);
				if (el.objectReferenceValue == boolEvent)
				{
					return true;
				}
			}
			return false;
		}

		private void OnDrawEvalHeader(Rect rect)
		{
			EditorGUI.LabelField(rect, "Conditions");
		}

		private void OnDrawEvalElement(Rect rect, int index, bool isActive, bool isFocused)
		{
			rect.height = EditorGUIUtility.singleLineHeight;
			var bit = bits.GetArrayElementAtIndex(index);
			EditorGUI.BeginChangeCheck();
			bit.intValue = EditorGUI.MaskField(rect, new GUIContent($"Flags ({bit.intValue})"), bit.intValue, scriptableBoolNames);
			if (EditorGUI.EndChangeCheck())
			{
				if (bit.intValue < 0)
				{
					bit.intValue = CleanUpBits(bit.intValue);
				}
			}
			rect.y += EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;
			var condition = conditions.GetArrayElementAtIndex(index);
			condition.intValue = EditorGUI.Popup(rect, "Condition", condition.intValue, conditionDrops);
			rect.y += EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;
			var ev = events.GetArrayElementAtIndex(index);
			rect.height = EditorGUI.GetPropertyHeight(ev);
			EditorGUI.PropertyField(rect, ev, new GUIContent("On Success"), false);
		}

		private void RemoveEvalCallback(ReorderableList list)
		{
			var index = list.index;
			conditions.DeleteArrayElementAtIndex(index);
			bits.DeleteArrayElementAtIndex(index);
			events.DeleteArrayElementAtIndex(index);
		}

		private void AddEvalCallback(ReorderableList list)
		{
			conditions.arraySize++;
			bits.arraySize++;
			events.arraySize++;
		}

		private float ElementHeightEvalCallback(int index)
		{
			var height = EditorGUIUtility.standardVerticalSpacing * 3;
			if (conditions.arraySize > index)
			{
				var condition = conditions.GetArrayElementAtIndex(index);
				height += EditorGUI.GetPropertyHeight(condition);

			}
			if (events.arraySize > index)
			{
				var ev = events.GetArrayElementAtIndex(index);
				height += EditorGUI.GetPropertyHeight(ev);

			}
			if (bits.arraySize > index)
			{
				var bit = bits.GetArrayElementAtIndex(index);
				height += EditorGUI.GetPropertyHeight(bit);

			}
			return height;
		}

		private int CleanUpBits(int value)
		{
			int bit = 0;

			for (int i = 0; i < statements.arraySize; i++)
			{
				int enumValue = (int)Mathf.Pow(2, i);
				int checkBit = value & enumValue;
				if (checkBit != 0)
					bit |= enumValue;
			}

			return bit;
		}

		private void UnsetBits(int bit)
		{
			for (int i = 0; i < bits.arraySize; i++)
			{
				var el = bits.GetArrayElementAtIndex(i);
				if (HasFlag(el.intValue, bit))
				{
					el.intValue &= ~bit;
				}
			}
		}

		public bool HasFlag(int a, int b)
		{
			return (a & b) == b;
		}

		void Refresh()
		{
			statementAssets.Clear();
			var guids = AssetDatabase.FindAssets($"t:{nameof(ReferenceBool)}");
			foreach (var g in guids)
			{
				var path = AssetDatabase.GUIDToAssetPath(g);
				var asset = AssetDatabase.LoadAssetAtPath<ReferenceBool>(path);
				statementAssets.Add(asset);
			}
		}
	}
}

