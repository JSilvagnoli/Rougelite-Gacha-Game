using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CharacterDisplay : MonoBehaviour
{
    public Characters character;

    public GameObject characterModel;

    public TextMeshProUGUI nameText;
    public TextMeshProUGUI descriptionText;

    public void InitializeCharacterDisplay()
    {
        nameText.text = character.name;
        descriptionText.text = character.description;

        characterModel = character.model;
        Vector3 characterPosition = new Vector3(70, -45, -797);
        GameObject newCharacter = Instantiate(characterModel, characterPosition, Quaternion.identity);
        newCharacter.transform.localScale = new Vector3(4, 4, 4);
    }

    public void ActivateCharacterAbility()
    {
        // Leave this commented out for now. Until you decide what to do with the abilities
        /*if (character.abilityScript != null)
        {
            character.abilityScript.ActivateAbility();
        }*/
    }
}
