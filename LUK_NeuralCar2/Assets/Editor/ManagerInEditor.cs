using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(Manager))]
public class ManagerInEditor : Editor
{
    //Ajustable properties
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

        //Title
        EditorGUILayout.LabelField("CUSTOM EDITOR");

        //Show custom GUI controls
        //Population
        if (populationProperty.intValue <= 20)
            EditorGUILayout.LabelField("(This amount of cars won't probably be enough)");
        else if (populationProperty.intValue >= 150)
            EditorGUILayout.LabelField("(This amount of cars will probably make you lag)");
        else
            EditorGUILayout.LabelField("(This amount of cars is fine !)");

        EditorGUILayout.IntSlider(populationProperty, 0, 200, new GUIContent("Population"));
        ProgressBar(populationProperty.intValue / 200f, "Population");

        //Duration
        int durationMinutes = durationProperty.intValue / 60;
        int durationSeconds = durationProperty.intValue - durationMinutes * 60;
        EditorGUILayout.LabelField("(= " + durationMinutes.ToString() + " minutes and " +
            durationSeconds.ToString() + " seconds)");

        if (durationProperty.intValue <= 30)
            EditorGUILayout.LabelField("(This training won't be very long...)");

        EditorGUILayout.IntSlider(durationProperty, 0, 300, new GUIContent("Duration"));
        ProgressBar(durationProperty.intValue / 300f, "Duration");

        //Mutation
        if (mutationProperty.intValue == 0)
            EditorGUILayout.LabelField("(This is definitely not enough)");
        else if (mutationProperty.intValue >= 75)
            EditorGUILayout.LabelField("(This is probably too much)");
        else
            EditorGUILayout.LabelField("(This mutation rate is fine !)");

        EditorGUILayout.IntSlider(mutationProperty, 0, 100, new GUIContent("Mutation"));
        ProgressBar(mutationProperty.intValue / 100f, "Mutation");

        Manager manager = (Manager)target;
        if (GUILayout.Button("RandomizeAll"))
            manager.RandomizeAll();

        //Application
        serializedObject.ApplyModifiedProperties();
    }

    void ProgressBar(float value, string label)
    {
        Rect rect = GUILayoutUtility.GetRect(18, 18, "TextField");
        EditorGUI.ProgressBar(rect, value, label);
        EditorGUILayout.Space();
    }
}