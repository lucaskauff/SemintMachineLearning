using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(Manager))]
public class ManagerInEditor : Editor
{
    SerializedProperty populationProperty;
    SerializedProperty durationProperty;
    SerializedProperty mutationProperty;

    private void OnEnable()
    {
        //Binding of properties
        populationProperty = serializedObject.FindProperty("populationSize");
        durationProperty = serializedObject.FindProperty("trainingDuration");
        mutationProperty = serializedObject.FindProperty("mutationRate");
    }

    public override void OnInspectorGUI()
    {
        //Update
        serializedObject.Update();

        //Show custom GUI controls
        //Population
        EditorGUILayout.IntSlider(populationProperty, 0, 250, new GUIContent("Population"));
        ProgressBar(populationProperty.intValue / 250, "Population");

        //Duration
        EditorGUILayout.IntSlider(durationProperty, 0, 300, new GUIContent("Duration"));
        ProgressBar(durationProperty.intValue / 300, "Duration");

        //Mutation
        EditorGUILayout.IntSlider(mutationProperty, 0, 100, new GUIContent("Mutation"));
        ProgressBar(mutationProperty.intValue / 100, "Mutation");

        //Application
        serializedObject.ApplyModifiedProperties();
    }

    void ProgressBar(int value, string label)
    {
        Rect rect = GUILayoutUtility.GetRect(18, 18, "TextField");
        EditorGUI.ProgressBar(rect, value, label);
        EditorGUILayout.Space();
    }
}