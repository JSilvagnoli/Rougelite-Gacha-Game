using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using TMPro;
using UnityEngine;

public class InstructionManager : MonoBehaviour
{
    public List<InstructionNode> actions = new List<InstructionNode>();
    public List<AlternateDialogueLines> alteranateDialogueLines = new List<AlternateDialogueLines>();

    public GameObject dialogueBox;
    public TextMeshProUGUI textComponent;

    public int currentIndex;
    public int dialogueLineToChoose;
    public float textSpeed;
    public float walkSpeed;

    public string skippedText;
    public string characterToMove;
    public string waypointToMoveTo;
    public string extraDialogueLine;

    public GameObject character;
    public GameObject waypoint;

    public InteractWithNPC interactWithNPC;
    private InstructionNode currentNode = null;
    public PlayerMovement playerMovement;

    public bool spokenTo = false;
    public bool instructionsCompleted = false;

    private Coroutine currentDialogueCoroutine;
    public Coroutine extraDialogueCoroutine;

    private void Start()
    {
        currentIndex = 0;

        character = GameObject.FindGameObjectWithTag("Character");
        interactWithNPC = character.GetComponent<InteractWithNPC>();
        playerMovement = character.GetComponent<PlayerMovement>();
    }

    /*private void Update()
    {
        if (interactWithNPC.isInteracting && currentIndex == actions.Count && !instructionsCompleted)
        {
            instructionsCompleted = true;
            playerMovement.enabled = true;
        }
    }

    public IEnumerator ExecuteAllInstruction()
    {
        List<INodeActionTest> actions = new List<INodeActionTest>();

        foreach (InstructionNode node in actions)
        {
            INodeActionTest nodeAction = NodeInterpreter.ConvertNodeToIAction(node);
        }

        foreach (INodeAction action in actions)
        {
            yield return action.Execute();

            while (!action.isFinished)
            {
                yield return null;
            }
        }
    }

    public void SkipDialogueLineForNormalInstructions()
    {
        // Stop the current coroutine responsible for typing out dialogue
        StopCoroutine(currentDialogueCoroutine);

        currentIndex++;

        textComponent.text = skippedText;
    }

    public IEnumerator DisplayFinalDialogueLine(GameObject character)
    {
        dialogueBox.SetActive(true);

        textComponent.text = "";

        foreach (AlternateDialogueLines alternateDialogueLine in alteranateDialogueLines)
        {
            if (alternateDialogueLine.characterName == character.name)
            {
                dialogueLineToChoose = Random.Range(0, alternateDialogueLine.dialogueLines.Count);
                extraDialogueLine = alternateDialogueLine.dialogueLines[dialogueLineToChoose];

                foreach (char c in alternateDialogueLine.dialogueLines[dialogueLineToChoose])
                {
                    if (textComponent != null)
                    {
                        textComponent.text += c;
                    }

                    yield return new WaitForSeconds(textSpeed);
                }
            }
        }
    }

    public IEnumerator SkipDialogueForExtraDialogue()
    {
        textComponent.text = extraDialogueLine;

        return null;
    }*/
}
