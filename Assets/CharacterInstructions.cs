using UnityEditor;
using UnityEngine;

[CreateAssetMenu(menuName = "Instructions")]
public class CharacterInstructions : ScriptableObject
{
    [SerializeField]
    private InstructionNode[] nodes;
    public InstructionNode[] Nodes => nodes;
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

[System.Serializable]
public class InstructionNode
{
    public ActionType action = ActionType.Nothing;

    public string characterToPerformInstruction;
    public DialogueInformation dialogueInformation;
    public WalkInformation walkInformation;
}


[System.Serializable]
public class DialogueInformation
{
    [SerializeField]
    public string characterName;

    [SerializeField]
    public string dialogueLine;
}

[System.Serializable]
public class WalkInformation
{
    [SerializeField]
    public string waypoint;

    [SerializeField]
    public float walkSpeed;
}
