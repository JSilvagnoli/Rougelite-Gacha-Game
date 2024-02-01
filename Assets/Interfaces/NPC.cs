using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using System;
using TMPro;
using System.Linq;
using UnityEditor.Experimental.GraphView;

public class NPC : MonoBehaviour, IInteractable
{
    // Reference to the CharacterInstructions ScriptableObject
    public CharacterInstructions characterInstructions;
    public InteractWithNPC interactWithNPC;

    public GameObject dialogueBox;

    private TextMeshProUGUI textComponent;

    private float textSpeed = 0.08f;
    private string skippedText = "";

    private Coroutine currentCoroutine = null;
    private Coroutine altDialogueCoroutine = null;

    public bool goToNextAction = false;
    public bool deactivateDialogueBox = false;
    public bool allNodesCompleted = false;

    Dictionary<InstructionNode, bool> nodeCompletionStatus = new Dictionary<InstructionNode, bool>();
    InstructionNode currentNode = null;

    private void Start()
    {
        GetNodes();
        dialogueBox.SetActive(false);
    }

    public void InteractLogic()
    {
        if (!AllNodesCompleted())
        {
            if (characterInstructions != null && characterInstructions.Nodes.Length > 0)
            {
                if (currentNode != null)
                {
                    if (goToNextAction && !nodeCompletionStatus[currentNode])
                    {
                        nodeCompletionStatus[currentNode] = true;
                        goToNextAction = false;
                    }
                }

                foreach (var kvp in nodeCompletionStatus)
                {
                    InstructionNode node = kvp.Key;
                    bool completionStatus = kvp.Value;
                    Debug.Log("Node: " + node + ", Completion Status: " + completionStatus);
                }

                StartCoroutine(ExecuteInstructions());
            }
            else
            {
                Debug.LogWarning("No dialogue instructions set for this NPC.");
            }
        }
        else
        {
            if (altDialogueCoroutine != null)
            {
                StopCoroutine(altDialogueCoroutine);
            }
            Debug.Log("Alternate Dialogue Line");
            altDialogueCoroutine = StartCoroutine(HandleAlternateDialogue());
        }
    }

    private IEnumerator HandleAlternateDialogue()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Character");
        interactWithNPC = player.GetComponent<InteractWithNPC>();

        if (characterInstructions != null && characterInstructions.AlternateDialogue.Length > 0)
        {
            if (!interactWithNPC.alternateDialogue)
            {
                foreach (AlternateDialogueLine alternateDialogueLine in characterInstructions.AlternateDialogue)
                {
                    if (EnumHelper.GetEnumDescription(alternateDialogueLine.characterToPerformDialogue) == this.gameObject.name)
                    {
                        int randomLine = UnityEngine.Random.Range(0, alternateDialogueLine.alternateDialogueLines.Length);
                        string randomDialogueLine = alternateDialogueLine.alternateDialogueLines[randomLine];

                        dialogueBox.SetActive(true);

                        GameObject characterNameTextBox = GameObject.Find("Character Name Text");
                        TextMeshProUGUI nameComponent = characterNameTextBox.GetComponent<TextMeshProUGUI>();

                        nameComponent.text = EnumHelper.GetEnumDescription(alternateDialogueLine.characterToPerformDialogue);

                        GameObject dialogueTextBox = GameObject.Find("Dialogue Text");
                        textComponent = dialogueTextBox.GetComponent<TextMeshProUGUI>();

                        skippedText = randomDialogueLine;

                        textComponent.text = "";

                        foreach (char c in randomDialogueLine.ToCharArray())
                        {
                            if (textComponent != null)
                            {
                                textComponent.text += c;
                            }

                            yield return new WaitForSeconds(textSpeed);
                        }

                        interactWithNPC.skipDialogue = true;
                    }
                }
            }
            else if (interactWithNPC.alternateDialogue)
            {
                textComponent.text = skippedText;
            }
        }
    }

    private void GetNodes()
    {
        foreach (InstructionNode node in characterInstructions.Nodes)
        {
            nodeCompletionStatus.Add(node, false);
        }
    }

    private IEnumerator ExecuteInstructions()
    {
        foreach (InstructionNode node in nodeCompletionStatus.Keys)
        {
            if (nodeCompletionStatus[node])
                continue;

            INodeAction nodeAction = NodeConverter.ConvertToIAction(node);

            currentNode = node;

            GameObject player = GameObject.FindGameObjectWithTag("Character");
            interactWithNPC = player.GetComponent<InteractWithNPC>();

            if (nodeAction is DialogueAction)
            {
                if (interactWithNPC != null)
                {
                    dialogueBox.SetActive(true);

                    if (!interactWithNPC.skipDialogue)
                    {
                        currentCoroutine = StartCoroutine(nodeAction.Execute());
                    }
                    else
                    {
                        if (currentCoroutine != null)
                        {
                            StopCoroutine(currentCoroutine);
                            StartCoroutine(nodeAction.Execute());
                        }
                    }
                }
                break;
            }
            else if (nodeAction is WalkAction)
            {
                dialogueBox.SetActive(false);

                StartCoroutine(nodeAction.Execute());
                break;
            }

            while (!goToNextAction)
            {
                yield return null;
            }
        }
    }

    // Method to check if all nodes are completed
    public bool AllNodesCompleted()
    {
        if (allNodesCompleted)
        {
            return true;
        }

        foreach (bool completionStatus in nodeCompletionStatus.Values)
        {
            if (!completionStatus)
            {
                return false;
            }
        }

        if (!deactivateDialogueBox)
        {
            Debug.Log("All nodes completed");
            dialogueBox.SetActive(false);
        }
        return true;
    }
}
