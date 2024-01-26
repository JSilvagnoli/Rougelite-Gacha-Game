using UnityEditor;
using UnityEngine;

[CreateAssetMenu(menuName = "Dialogue Controls")]
public class DialogueControls : ScriptableObject
{
    [SerializeField]
    private DialogueNode[] nodes;
    public DialogueNode[] Nodes => nodes;
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
public class DialogueNode
{
    public ActionType action = ActionType.Nothing;
    public DialogueInformation dialogueInformation;
}


[System.Serializable]
public class DialogueInformation
{
    [SerializeField]
    public string characterName;

    [SerializeField]
    public string dialogueLine;
}
