using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(Recipe))]
public class RecipeScriptableObjectEditor : Editor
{
    public override void OnInspectorGUI() {
        serializedObject.Update();

        Recipe recipeScriptableObject = (Recipe)target;

        EditorGUILayout.BeginHorizontal();
        GUILayout.FlexibleSpace();
        GUILayout.Label("OUTPUT", new GUIStyle { fontStyle = FontStyle.Bold });
        GUILayout.FlexibleSpace();
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.BeginHorizontal();
        GUILayout.FlexibleSpace();

        EditorGUILayout.BeginVertical();


        EditorGUILayout.PropertyField(serializedObject.FindProperty("outputItem"), GUIContent.none, true, GUILayout.Width(150));
        EditorGUILayout.PropertyField(serializedObject.FindProperty("outputItemMuch"), GUIContent.none, true, GUILayout.Width(25));
        EditorGUILayout.EndVertical();

        GUILayout.FlexibleSpace();
        EditorGUILayout.EndHorizontal();



        EditorGUILayout.Space();



        GUILayout.Label("RECIPE", new GUIStyle { fontStyle = FontStyle.Bold });
        EditorGUILayout.BeginHorizontal();

        EditorGUILayout.BeginVertical();

        EditorGUILayout.PropertyField(serializedObject.FindProperty("item_02"), GUIContent.none, true, GUILayout.Width(100));
        EditorGUILayout.PropertyField(serializedObject.FindProperty("item_02Much"), GUIContent.none, true, GUILayout.Width(25));
        EditorGUILayout.EndVertical();

        EditorGUILayout.BeginVertical();

        EditorGUILayout.PropertyField(serializedObject.FindProperty("item_12"), GUIContent.none, true, GUILayout.Width(100));
        EditorGUILayout.PropertyField(serializedObject.FindProperty("item_12Much"), GUIContent.none, true, GUILayout.Width(25));
        EditorGUILayout.EndVertical();

        EditorGUILayout.BeginVertical();

        EditorGUILayout.PropertyField(serializedObject.FindProperty("item_22"), GUIContent.none, true, GUILayout.Width(100));
        EditorGUILayout.PropertyField(serializedObject.FindProperty("item_22Much"), GUIContent.none, true, GUILayout.Width(25));
        EditorGUILayout.EndVertical();

        EditorGUILayout.EndHorizontal();



        EditorGUILayout.BeginHorizontal();

        EditorGUILayout.BeginVertical();

        EditorGUILayout.PropertyField(serializedObject.FindProperty("item_01"), GUIContent.none, true, GUILayout.Width(100));
        EditorGUILayout.PropertyField(serializedObject.FindProperty("item_01Much"), GUIContent.none, true, GUILayout.Width(25));
        EditorGUILayout.EndVertical();

        EditorGUILayout.BeginVertical();

        EditorGUILayout.PropertyField(serializedObject.FindProperty("item_11"), GUIContent.none, true, GUILayout.Width(100));
        EditorGUILayout.PropertyField(serializedObject.FindProperty("item_11Much"), GUIContent.none, true, GUILayout.Width(25));
        EditorGUILayout.EndVertical();

        EditorGUILayout.BeginVertical();

        EditorGUILayout.PropertyField(serializedObject.FindProperty("item_21"), GUIContent.none, true, GUILayout.Width(100));
        EditorGUILayout.PropertyField(serializedObject.FindProperty("item_21Much"), GUIContent.none, true, GUILayout.Width(25));
        EditorGUILayout.EndVertical();

        EditorGUILayout.EndHorizontal();




        EditorGUILayout.BeginHorizontal();

        EditorGUILayout.BeginVertical();

        EditorGUILayout.PropertyField(serializedObject.FindProperty("item_00"), GUIContent.none, true, GUILayout.Width(100));
        EditorGUILayout.PropertyField(serializedObject.FindProperty("item_00Much"), GUIContent.none, true, GUILayout.Width(25));
        EditorGUILayout.EndVertical();

        EditorGUILayout.BeginVertical();

        EditorGUILayout.PropertyField(serializedObject.FindProperty("item_10"), GUIContent.none, true, GUILayout.Width(100));
        EditorGUILayout.PropertyField(serializedObject.FindProperty("item_10Much"), GUIContent.none, true, GUILayout.Width(25));
        EditorGUILayout.EndVertical();

        EditorGUILayout.BeginVertical();

        EditorGUILayout.PropertyField(serializedObject.FindProperty("item_20"), GUIContent.none, true, GUILayout.Width(100));
        EditorGUILayout.PropertyField(serializedObject.FindProperty("item_20Much"), GUIContent.none, true, GUILayout.Width(25));
        EditorGUILayout.EndVertical();

        EditorGUILayout.EndHorizontal();


        serializedObject.ApplyModifiedProperties();
    }


}