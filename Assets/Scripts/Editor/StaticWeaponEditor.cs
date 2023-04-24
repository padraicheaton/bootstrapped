using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(StaticWeaponCreator.StaticWeapon))]
public class StaticWeaponEditor : Editor
{
    private SerializedProperty myIntsProperty;

    private void OnEnable()
    {
        myIntsProperty = serializedObject.FindProperty("genotype");
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        EditorGUILayout.BeginHorizontal();

        int[] myInts = new int[myIntsProperty.arraySize];
        for (int i = 0; i < myIntsProperty.arraySize; i++)
        {
            myInts[i] = myIntsProperty.GetArrayElementAtIndex(i).intValue;
        }

        EditorGUI.BeginChangeCheck();

        for (int i = 0; i < myInts.Length; i++)
        {
            myInts[i] = EditorGUILayout.IntField(myInts[i], GUILayout.Width(30));
        }

        if (EditorGUI.EndChangeCheck())
        {
            for (int i = 0; i < myInts.Length; i++)
            {
                myIntsProperty.GetArrayElementAtIndex(i).intValue = myInts[i];
            }
        }

        EditorGUILayout.EndHorizontal();

        serializedObject.ApplyModifiedProperties();
    }
}
