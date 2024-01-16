using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Character Information")]
public class Characters : ScriptableObject
{
    public GameObject model;

    public string characterName;

    [TextArea]
    public string description;

    // Reference to the base class for abilities
    public CharacterAbility abilityScript;
}
