using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class CharacterInstructions : MonoBehaviour
{
    public List<DialogueNode> actions = new List<DialogueNode>();

    public TextMeshProUGUI textComponent;

    public int currentIndex;
    public float textSpeed;
    public string skippedText;

    public GameObject character;
    public InteractWithNPC interactWithNPC;

    public bool spokenTo = false;

    private Coroutine currentDialogueCoroutine;

    private void Start()
    {
        currentIndex = 0;

        character = GameObject.FindGameObjectWithTag("Character");
        interactWithNPC = character.GetComponent<InteractWithNPC>();
    }

    public void PerformNextInstruction()
    {
        Debug.Log(currentIndex);
        if (currentIndex < actions.Count)
        {
            DialogueNode currentNode = actions[currentIndex];

            if (currentNode.action == ActionType.Nothing)
            {
                // Do nothing
            }
            else if (currentNode.action == ActionType.Dialogue)
            {
                DialogueInformation dialogueInformation = currentNode.dialogueInformation;
                currentDialogueCoroutine = StartCoroutine(TypeLine(dialogueInformation)); // Start typing dialogue and store the coroutine reference
            }
            else if (currentNode.action == ActionType.Walk)
            {
                DialogueInformation dialogueInformation = currentNode.dialogueInformation;
                StartCoroutine(MoveCharacter(dialogueInformation));
            }
        }

        spokenTo = true;
    }

    private IEnumerator TypeLine(DialogueInformation dialogueInformation)
    {
        textComponent.text = "";

        skippedText = dialogueInformation.dialogueLine;

        foreach (char c in dialogueInformation.dialogueLine.ToCharArray())
        {
            if (textComponent != null)
            {
                textComponent.text += c;
            }

            yield return new WaitForSeconds(textSpeed);
        }

        // After typing the line, check if there are more actions to perform
        if (currentIndex < actions.Count)
        {
            currentIndex++;
            // If there are more actions, perform the next instruction
            DialogueNode nextNode = actions[currentIndex];
            if (nextNode.action == ActionType.Dialogue)
            {
                
                // If the next action is dialogue, start typing it
                DialogueInformation nextDialogue = nextNode.dialogueInformation;
                StartCoroutine(TypeLine(nextDialogue));
            }
            else if (nextNode.action == ActionType.Walk)
            {
                // If the next action is walking, start moving character
                DialogueInformation walkDialogue = nextNode.dialogueInformation;
                StartCoroutine(MoveCharacter(walkDialogue));
            }
        }
    }

    public void SkipDialogue()
    {
        // Stop the current coroutine responsible for typing out dialogue
        StopCoroutine(currentDialogueCoroutine);

        currentIndex++;

        textComponent.text = skippedText;
    }

    private IEnumerator MoveCharacter(DialogueInformation dialogueInformation)
    {
        yield return null;

        currentIndex++;
    }
}