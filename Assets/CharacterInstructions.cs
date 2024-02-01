using JetBrains.Annotations;
using UnityEditor;
using UnityEngine;
using System.ComponentModel;
using System;

[CreateAssetMenu(menuName = "Instructions")]
public class CharacterInstructions : ScriptableObject
{
    [SerializeField]
    private InstructionNode[] nodes;
    public InstructionNode[] Nodes => nodes;

    [SerializeField]
    private AlternateDialogueLine[] alternateDialogue;
    public AlternateDialogueLine[] AlternateDialogue => alternateDialogue;

}

public enum ActionType
{
    Nothing,
    Dialogue,
    Walk,
    ChangeCamera,
    PlaySong,
    StopSong,
    Cutscene,
    Animation,
    ScreenShake
}

public enum AlternateDialogueCharacter
{
    [Description("Ethan Magus")]
    EthanMagus,

    [Description("Jessica Night")]
    JessicaNight,

    [Description("Damien Mortis")]
    DamienMortis
}

// Function to get the description of an enum value
public static class EnumHelper
{
    public static string GetEnumDescription(Enum value)
    {
        var fieldInfo = value.GetType().GetField(value.ToString());

        var attribute = (DescriptionAttribute)Attribute.GetCustomAttribute(fieldInfo, typeof(DescriptionAttribute));

        return attribute == null ? value.ToString() : attribute.Description;
    }
}

[System.Serializable]
public class InstructionNode
{
    public ActionType action = ActionType.Nothing;

    public string characterToPerformInstruction;
    public DialogueInformation dialogueInformation;
    public WalkInformation walkInformation;
}

[System.Serializable]
public class AlternateDialogueLine
{
    public AlternateDialogueCharacter characterToPerformDialogue = AlternateDialogueCharacter.EthanMagus;

    public string[] alternateDialogueLines;
}

[System.Serializable]
public class DialogueInformation
{
    [SerializeField]
    public string dialogueLine;
}

[System.Serializable]
public class WalkInformation
{
    [SerializeField]
    public string[] charactersToMove;

    [SerializeField]
    public string[] waypoints;

    [SerializeField]
    public float walkSpeed;
}
