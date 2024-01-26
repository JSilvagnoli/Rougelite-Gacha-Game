using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(DialogueControls))]
public class DialogueEditor : Editor
{
    private SerializedProperty nodes;

    private void OnEnable()
    {
        nodes = serializedObject.FindProperty("nodes");
    }

    public override void OnInspectorGUI()
    {
        EditorGUILayout.PropertyField(nodes, new GUIContent("Nodes"), true);

        serializedObject.ApplyModifiedProperties();
    }
}

[CustomPropertyDrawer(typeof(DialogueNode))]
public class DialogueNodeDrawer : PropertyDrawer
{
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        EditorGUI.BeginProperty(position, label, property);

        // Draw the foldout for the DialogueNode
        Rect foldOutBox = new Rect(position.min.x, position.min.y,
            position.size.x, EditorGUIUtility.singleLineHeight);
        property.isExpanded = EditorGUI.Foldout(foldOutBox, property.isExpanded, label);

        // Draw the rest of the properties if expanded
        if (property.isExpanded)
        {
            SerializedProperty action = property.FindPropertyRelative("action");

            SerializedProperty dialogueInformation = property.FindPropertyRelative("dialogueInformation");
            SerializedProperty characterName = dialogueInformation.FindPropertyRelative("characterName");
            SerializedProperty dialogueLine = dialogueInformation.FindPropertyRelative("dialogueLine");

            // Draw the ActionType enum field
            EditorGUI.PropertyField(new Rect(position.x, position.y + EditorGUIUtility.singleLineHeight, position.width, EditorGUIUtility.singleLineHeight), action);

            switch ((ActionType)action.enumValueIndex)
            {
                case ActionType.Nothing:

                    
                    break;
                case ActionType.Dialogue:
                    GUI.backgroundColor = Color.red;

                    // Draw the rest of the properties
                    EditorGUI.PropertyField(new Rect(position.x, position.y + EditorGUIUtility.singleLineHeight * 2, position.width, EditorGUIUtility.singleLineHeight), characterName, new GUIContent("Character Name"));
                    EditorGUI.PropertyField(new Rect(position.x, position.y + EditorGUIUtility.singleLineHeight * 3, position.width, EditorGUIUtility.singleLineHeight), dialogueLine, new GUIContent("Dialogue Line"));
                    break;
                case ActionType.Walk:
                    GUI.backgroundColor = Color.blue;


                    break;
            }
        }

        EditorGUI.EndProperty();
    }

    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        if (property.isExpanded)
        {
            return EditorGUIUtility.singleLineHeight * 4; // Adjust as needed
        }

        return EditorGUIUtility.singleLineHeight;
    }
}