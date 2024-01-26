using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Dialogue : MonoBehaviour
{
    public TextMeshProUGUI textComponent;

    public AudioSource audioSource;

    public List<NPCDialogue> npcDialogueData;
    private List<bool> npcInteractedStatus = new List<bool>();

    public float textSpeed;

    private int index;
    public int npcIndex;
    private int displayedCharacterCount = 0;

    public bool dialogue = false;
    private bool hasInteracted = false;

    public DialogueController dialogueController;
    public InteractWithNPC interactWithNPC;

    GameObject character = null;

    private void Start()
    {
        textComponent.text = string.Empty;
        foreach (NPCDialogue npc in npcDialogueData)
        {
            npcInteractedStatus.Add(false);
            npcDialogueData[npcIndex].spokenTo = false;
        }
    }

    private void Update()
    {
        if (character == null)
        {
            character = GameObject.FindGameObjectWithTag("Character");
        }
        else
        {
            dialogueController = character.GetComponent<DialogueController>();
            interactWithNPC = character.GetComponent<InteractWithNPC>();
        }

        if (dialogue && dialogueController.isClicking)
        {
            if (textComponent.text == npcDialogueData[npcIndex].lines[index])
            {
                if (index < npcDialogueData[npcIndex].lines.Count - 1)
                {
                    NextLine();
                    dialogueController.isClicking = false;
                }
                else
                {
                    CloseDialogue();
                }
                dialogueController.isClicking = false;
            }
            else
            {
                StopAllCoroutines();
                textComponent.text = npcDialogueData[npcIndex].lines[index];
                dialogueController.isClicking = false;
            }
        }
    }

    public void StartDialogue()
    {
        dialogue = true;

        hasInteracted = npcInteractedStatus[npcIndex];

        if (hasInteracted)
        {
            index = npcDialogueData[npcIndex].lines.Count - 1;
            StartCoroutine(TypeLine());
        }
        else
        {
            index = 0;
            StartCoroutine(TypeLine());
        }
    }

    private IEnumerator TypeLine()
    {
        displayedCharacterCount = 0;

        foreach (char c in npcDialogueData[npcIndex].lines[index].ToCharArray())
        {
            textComponent.text += c;

            displayedCharacterCount ++;

            yield return new WaitForSeconds(textSpeed);
        }
    }

    private void NextLine()
    {
        if (index < npcDialogueData[npcIndex].lines.Count - 1)
        {
            index++;
            textComponent.text = string.Empty;
            StartCoroutine(TypeLine());
        }
        else
        {
            CloseDialogue();
        }
    }

    private void CloseDialogue()
    {
        textComponent.text = string.Empty;

        gameObject.SetActive(false);
        dialogue = false;
        interactWithNPC.isInteracting = false;

        npcInteractedStatus[npcIndex] = true;

        npcDialogueData[npcIndex].spokenTo = true;
    }
}
