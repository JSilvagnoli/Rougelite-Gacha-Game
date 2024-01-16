using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterCollectionManager : MonoBehaviour
{
    public Dictionary<Characters, int> characterCounts = new Dictionary<Characters, int>();

    private void Update()
    {
        if (Input.GetKeyDown("space"))
        {
            ShowCollectedCharacters();
        }
    }

    // Call this method when the player obtains a new character
    public void AddCharacterToCollection(Characters character)
    {
        if (characterCounts.ContainsKey(character))
        {
            // Character already exists in the collection, update the count
            characterCounts[character]++;
        }
        else
        {
            // Character is not in the collection, add it with a count of 1
            characterCounts.Add(character, 1);
        }
    }

    // Overload for handling a list of characters
    public void AddCharacterToCollection(List<Characters> characters)
    {
        foreach (var character in characters) 
        {
            AddCharacterToCollection(character);
        }
    }

    public void ShowCollectedCharacters()
    {
        foreach (var character in characterCounts)
        {
            Debug.Log($"Character: {character.Key}, Count: {character.Value}");
        }
    }
}
