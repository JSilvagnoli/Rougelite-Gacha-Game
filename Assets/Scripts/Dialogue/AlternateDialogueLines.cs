using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[CreateAssetMenu(menuName = "Alternate Dialogue")]
public class AlternateDialogueLines : ScriptableObject
{
    public string characterName;
    public List<string> dialogueLines = new List<string>();
}
