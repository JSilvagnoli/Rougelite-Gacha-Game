using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Character Rarities")]
public class CharacterRarity : ScriptableObject
{
    public string tier;

    public int rarity;

    public List<Characters> characters = new List<Characters>();
}
