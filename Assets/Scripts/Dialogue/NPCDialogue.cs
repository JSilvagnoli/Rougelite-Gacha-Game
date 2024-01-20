using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "NPC Dialogue")]
public class NPCDialogue : ScriptableObject
{
    public string npcName;
    [TextArea]
    public List<string> lines;
    public int index;
    public GameObject npcGameObject;
}
