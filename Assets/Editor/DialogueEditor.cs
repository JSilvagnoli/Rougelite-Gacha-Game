using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(CharacterInstructions))]
public class DialogueEditor : Editor
{
    private SerializedProperty nodes;
    private SerializedProperty alternateDialogue;

    private void OnEnable()
    {
        nodes = serializedObject.FindProperty("nodes");
        alternateDialogue = serializedObject.FindProperty("alternateDialogue");
    }

    public override void OnInspectorGUI()
    {
        EditorGUILayout.PropertyField(nodes, new GUIContent("Nodes"), true);

        EditorGUILayout.PropertyField(alternateDialogue, new GUIContent("Alternate Dialogue"), true);

        serializedObject.ApplyModifiedProperties();
    }
}

[CustomPropertyDrawer(typeof(InstructionNode))]
public class InstructionNodeDrawer : PropertyDrawer
{
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        EditorGUI.BeginProperty(position, label, property);

        // Draw the foldout for the InstructionNode
        Rect foldOutBox = new Rect(position.min.x, position.min.y,
            position.size.x, EditorGUIUtility.singleLineHeight);
        property.isExpanded = EditorGUI.Foldout(foldOutBox, property.isExpanded, label);

        // Draw the rest of the properties if expanded
        if (property.isExpanded)
        {
            SerializedProperty action = property.FindPropertyRelative("action");

            SerializedProperty characterToPerformInstruction = property.FindPropertyRelative("characterToPerformInstruction");

            // Draw the ActionType enum field
            EditorGUI.PropertyField(new Rect(position.x, position.y + EditorGUIUtility.singleLineHeight, position.width, EditorGUIUtility.singleLineHeight), action);

            switch ((ActionType)action.enumValueIndex)
            {
                case ActionType.Nothing:

                    
                    break;
                case ActionType.Dialogue:
                    GUI.backgroundColor = Color.red;

                    EditorGUI.PropertyField(new Rect(position.x, position.y + EditorGUIUtility.singleLineHeight * 2, position.width, EditorGUIUtility.singleLineHeight), characterToPerformInstruction, new GUIContent("Character To Perform Instruction"));
                    // Find the dialogue related variables
                    SerializedProperty dialogueInformation = property.FindPropertyRelative("dialogueInformation");
                    SerializedProperty dialogueLine = dialogueInformation.FindPropertyRelative("dialogueLine");

                    // Draw the rest of the properties
                    EditorGUI.PropertyField(new Rect(position.x, position.y + EditorGUIUtility.singleLineHeight * 3, position.width, EditorGUIUtility.singleLineHeight), dialogueLine, new GUIContent("Dialogue Line"));
                    break;
                case ActionType.Walk:
                    GUI.backgroundColor = Color.blue;

                    // Find the walk related variables
                    SerializedProperty walkInformation = property.FindPropertyRelative("walkInformation");
                    SerializedProperty charactersToMove = walkInformation.FindPropertyRelative("charactersToMove");
                    SerializedProperty waypoints = walkInformation.FindPropertyRelative("waypoints");
                    SerializedProperty walkSpeed = walkInformation.FindPropertyRelative("walkSpeed");

                    // Draw the rest of the properties
                    EditorGUI.PropertyField(new Rect(position.x, position.y + EditorGUIUtility.singleLineHeight * 2, position.width, EditorGUIUtility.singleLineHeight), walkSpeed, new GUIContent("Walk Speed"));
                    EditorGUI.PropertyField(new Rect(position.x, position.y + EditorGUIUtility.singleLineHeight * 3, position.width, EditorGUIUtility.singleLineHeight), waypoints, new GUIContent("Waypoints"));
                    EditorGUI.PropertyField(new Rect(position.x, position.y + EditorGUIUtility.singleLineHeight * 8, position.width, EditorGUIUtility.singleLineHeight), charactersToMove, new GUIContent("Characters to Move"));
                    break;
            }
        }        

        EditorGUI.EndProperty();
    }

    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        if (property.isExpanded)
        {
            return EditorGUIUtility.singleLineHeight * 20; // Adjust as needed
        }

        return EditorGUIUtility.singleLineHeight;
    }
}