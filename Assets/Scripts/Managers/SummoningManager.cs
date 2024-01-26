using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SummoningManager : MonoBehaviour
{
    public TextMeshProUGUI nameText;
    public TextMeshProUGUI descriptionText;

    public Button singleSummonButton;
    public Button multiSummonButton;
    public Button nextButton;
    public Button exitButton;
    public Button skipButton;

    public GameObject scrollView;
    public GameObject newCharacter;

    public List<CharacterRarity> rarities;

    public CharacterDisplay characterDisplay;
    public CharacterCollectionManager characterCollectionManager;

    private bool singleSummon;
    private bool multiSummon;
    private bool nextcharacter;
    private bool skipCharacters;
    public bool firstCharacterSummoned;
    public bool finishedInspectingCharacter;

    public int i;

    private void Start()
    {
        nameText.gameObject.SetActive(false);
        descriptionText.gameObject.SetActive(false);
        nextButton.gameObject.SetActive(false);
        exitButton.gameObject.SetActive(false);
        skipButton.gameObject.SetActive(false);
        scrollView.SetActive(false);
    }

    public void SingleSummon()
    {
        singleSummon = true;
        SummonCharacters();
        firstCharacterSummoned = true;
    }

    public void MultiSummon()
    {
        multiSummon = true;

        SummonCharacters();
    }

    public void SummonCharacters()
    {
        // Hide the buttons and show the character
        singleSummonButton.gameObject.SetActive(false);
        multiSummonButton.gameObject.SetActive(false);

        nameText.gameObject.SetActive(true);
        descriptionText.gameObject.SetActive(true);

        scrollView.SetActive(true);

        if (singleSummon)
        {
            Characters summonedCharacters = GetRandomCharacter();
            Debug.Log("Summoned Character " + summonedCharacters.characterName);

            // Sets the summoned character's information
            characterDisplay.character = summonedCharacters;

            // Saves the summoned character
            characterCollectionManager.AddCharacterToCollection(summonedCharacters);

            // Displays the summoned character's information on the screen
            characterDisplay.InitializeCharacterDisplay();

            // Prints the summoned character's ability to the console
            characterDisplay.ActivateCharacterAbility();

            exitButton.gameObject.SetActive(true);
        }
        else if (multiSummon)
        {
            StartCoroutine(MultiSummonCoroutine());
        }
    }

    IEnumerator MultiSummonCoroutine()
    {
        for (i = 0;  i < 10; i++)
        {
            Characters summonedCharacters = GetRandomCharacter();
            Debug.Log("Summoned Character " + summonedCharacters.characterName);

            // Sets the summoned character's information
            characterDisplay.character = summonedCharacters;

            // Saves the summoned character
            characterCollectionManager.AddCharacterToCollection(summonedCharacters);

            // Displays the summoned character's information on the screen
            characterDisplay.InitializeCharacterDisplay();

            // Prints the summoned character's ability to the console
            characterDisplay.ActivateCharacterAbility();

            skipButton.gameObject.SetActive(true);

            if (!skipCharacters)
            {
                if (i < 9)
                {
                    nextButton.gameObject.SetActive(true);
                }
            }
            else if (skipCharacters)
            {
                if (i < 9)
                {
                    GameObject newCharacter = GameObject.FindGameObjectWithTag("Character");
                    newCharacter.SetActive(false);
                }
            }

            exitButton.gameObject.SetActive(true);

            if (!skipCharacters)
            {
                // Wait until the player presses the next button
                yield return new WaitUntil(() => nextcharacter);

                // If the player pressed the next button, show the next character
                nextcharacter = false;
                nextButton.gameObject.SetActive(false);
            }
        }

        exitButton.gameObject.SetActive(true);
        skipButton.gameObject.SetActive(false);
        skipCharacters = false;
    }

    public void NextButton()
    {
        nextcharacter = true;
        if (i < 9)
        {
            GameObject newCharacter = GameObject.FindGameObjectWithTag("Character");
            newCharacter.SetActive(false);
            scrollView.SetActive(false);
        }
    }

    public void ExitButton()
    {
        singleSummon = false;
        multiSummon = false;

        exitButton.gameObject.SetActive(false);

        // Hide the character and show the buttons
        singleSummonButton.gameObject.SetActive(true);
        multiSummonButton.gameObject.SetActive(true);

        nameText.gameObject.SetActive(false);
        descriptionText.gameObject.SetActive(false);

        newCharacter = GameObject.FindGameObjectWithTag("DisplayCharacter");
        scrollView.SetActive(false);

        finishedInspectingCharacter = true;
    }

    public void SkipButton()
    {
        skipCharacters = true;
        nextcharacter = true;

        GameObject newCharacter = GameObject.FindGameObjectWithTag("Character");
        newCharacter.SetActive(false);
    }

    private Characters GetRandomCharacter()
    {
        // Determine total weight based on rarities
        int totalWeight = rarities.Sum(r => r.rarity);

        // Generate a random index based on total weight
        int randomIndex = Random.Range(0, totalWeight);

        // Find the rarity that corresponds to the random index
        foreach(var rarity in rarities)
        {
            if (randomIndex < rarity.rarity)
            {
                // If the random index is within the range of this rarity, return a random character from that rarity
                int characterIndex = Random.Range(0, rarity.characters.Count);
                return rarity.characters[characterIndex];
            }

            // Adjust the random index to move to the next rarity
            randomIndex -= rarity.rarity;
        }

        return null;
    }
}
