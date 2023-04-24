using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

[CustomEditor(typeof(StaticWeaponCreator))]
public class MyObjectEditor : Editor
{
    private SerializedProperty myObjectsProperty;

    private void OnEnable()
    {
        myObjectsProperty = serializedObject.FindProperty("weapons");
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        EditorGUILayout.BeginVertical();

        EditorGUILayout.LabelField("Genotypes", EditorStyles.boldLabel);

        for (int i = 0; i < myObjectsProperty.arraySize; i++)
        {
            SerializedProperty myObjectProperty = myObjectsProperty.GetArrayElementAtIndex(i);
            SerializedProperty myArrayProperty = myObjectProperty.FindPropertyRelative("genotype");

            EditorGUILayout.BeginHorizontal();

            for (int j = 0; j < myArrayProperty.arraySize; j++)
            {
                SerializedProperty elementProperty = myArrayProperty.GetArrayElementAtIndex(j);
                elementProperty.intValue = EditorGUILayout.IntField(elementProperty.intValue);
            }

            // Add and remove Genes
            if (GUILayout.Button("+"))
            {
                myArrayProperty.InsertArrayElementAtIndex(myArrayProperty.arraySize);
            }
            if (GUILayout.Button("-"))
            {
                myArrayProperty.DeleteArrayElementAtIndex(myArrayProperty.arraySize - 1);
            }

            EditorGUILayout.EndHorizontal();
        }

        if (GUILayout.Button("Add Object"))
        {
            serializedObject.Update();
            StaticWeaponCreator myObject = (StaticWeaponCreator)target;
            myObject.weapons.Add(new StaticWeaponCreator.StaticWeapon());
            serializedObject.ApplyModifiedProperties();
        }

        if (GUILayout.Button("Remove Object"))
        {
            serializedObject.Update();
            StaticWeaponCreator myObject = (StaticWeaponCreator)target;
            myObject.weapons.RemoveAt(myObject.weapons.Count - 1);
            serializedObject.ApplyModifiedProperties();
        }

        EditorGUILayout.Space();
        EditorGUILayout.HelpBox("On Start, all weapons listed here will generate at once", MessageType.Info);

        EditorGUILayout.EndVertical();

        serializedObject.ApplyModifiedProperties();
    }
}
