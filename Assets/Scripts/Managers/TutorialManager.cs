using System.Collections;
using System.Collections.Generic;
using UnityEditor.UIElements;
using UnityEngine;

public class TutorialManager : MonoBehaviour
{
    public GameObject ethanMagusNPC;
    public GameObject summoningBox;
    public GameObject dialogueBox;
    public GameObject startingArea;
    public GameObject introBox;
    public GameObject roomManager;

    private string characterName;

    public bool introCompleted;
    public bool skipIntro;
    public bool characterSpawned;
    public bool ethanSpawned;
    public bool tutorialTower;
    public bool ethanUpdated;

    public SummoningManager summoningManager;
    public CharacterCollectionManager characterCollectionManager;
    private Dialogue dialogue;

    public Camera introCamera;
    public Camera summoningSceneCamera;
    public Camera mainCharacterCamera;

    public List<GameObject> availableCharactersToStart;

    private void Awake()
    {
        introBox.SetActive(false);
        introCamera.gameObject.SetActive(false);
        dialogueBox.SetActive(false);
        summoningBox.SetActive(false);
        summoningSceneCamera.gameObject.SetActive(false);
        mainCharacterCamera.gameObject.SetActive(false);
        startingArea.SetActive(false);
        dialogue = dialogueBox.GetComponent<Dialogue>();
        roomManager.SetActive(false);
        dialogue.npcDialogueData[dialogue.npcIndex].spokenTo = false;
    }

    private void Update()
    {
        if (!summoningManager.firstCharacterSummoned)
        {
            summoningSceneCamera.gameObject.SetActive(true);

            // Show the intro cutscene and make the player summon their first character
            SummonFirstCharacter();
        }

        if (!introCompleted && summoningManager.finishedInspectingCharacter)
        {
            summoningSceneCamera.gameObject.SetActive(false);
            introBox.SetActive(true);
            introCamera.gameObject.SetActive(true);
            RectTransform singleSummonButtonRectTransform = summoningManager.singleSummonButton.GetComponent<RectTransform>();
            singleSummonButtonRectTransform.anchoredPosition = new Vector3(-100, 0, 0);
            summoningSceneCamera.gameObject.SetActive(false);
            summoningBox.SetActive(false);
            Destroy(summoningManager.newCharacter);
            StartCoroutine(IntroCutscene());
        }

        if (introCompleted)
        {
            introBox.SetActive(false);
            introCamera.gameObject.SetActive(false);

            if (!characterSpawned)
            {
                SpawnCharacter();
                mainCharacterCamera.gameObject.SetActive(true);
            }

            /*if (!ethanSpawned)
            {
                SpokenToEthan();
            }
            else
            {
                if (dialogue.npcDialogueData[dialogue.npcIndex].spokenTo)
                {
                    tutorialTower = true;

                    roomManager.SetActive(true);

                    if (!ethanUpdated)
                    {
                        CompletedTower();
                    }
                }
            }*/
        }

        // This will change the starting area, and open the doors for the new tower/s.
        /*if (ethanUpdated)
        {

        }*/
    }

    // Allow the player to summon their first character
    private void SummonFirstCharacter()
    {
        summoningBox.SetActive(true);
        RectTransform singleSummonButtonRectTransform = summoningManager.singleSummonButton.GetComponent<RectTransform>();
        singleSummonButtonRectTransform.anchoredPosition = Vector3.zero;
        summoningManager.multiSummonButton.gameObject.SetActive(false);
    }

    // Start the intro cutscene/show player text
    private IEnumerator IntroCutscene()
    {
        if (skipIntro)
        {
            SkipIntro();
            yield return null;
            introCompleted = true;
        }
        else
        {
            yield return new WaitForSeconds(30);
            SkipIntro();
            introCompleted = true;
        }
    }

    // Skip the intro/text
    public void SkipIntro()
    {
        introBox.SetActive(false);
        skipIntro = true;
    }

    // Spawn the character and let the player move around in the starting area
    private void SpawnCharacter()
    {
        startingArea.SetActive(true);
        SpawnSummonedCharacter();
    }

    // Spawns the player's summoned character
    private void SpawnSummonedCharacter()
    {
        foreach (var pair in characterCollectionManager.characterCounts)
        {
            characterName = pair.Key.characterName;
            Debug.Log(characterName);
        }

        foreach (GameObject character in availableCharactersToStart)
        {
            if (character.name == characterName)
            {
                GameObject firstCharacter = Instantiate(character, new Vector3(0, -50, -400), Quaternion.identity);
                firstCharacter.transform.localScale = new Vector3(5, 5, 5);
                characterSpawned = true;
            }
        }
    }

    // Checks if the player has spoken to Ethan
    /*private void SpokenToEthan()
    {
        Instantiate(ethanMagusNPC, new Vector3(0, -50.30797f, -300), Quaternion.identity);
        ethanMagusNPC.transform.localScale = new Vector3(5, 5, 5);

        ethanSpawned = true;
    }

    // Moves Ethan and changes to his second actions
    private void CompletedTower()
    {
        ethanMagusNPC = GameObject.Find("Ethan Magus NPC(Clone)");
        ethanMagusNPC.transform.position = new Vector3(0, -50.30797f, 300);
        dialogue.npcIndex++;

        ethanUpdated = true;
    }*/
}


