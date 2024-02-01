using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class InteractWithNPC : MonoBehaviour
{
    private IInteractable interactableInstance;

    private NPC npc;

    public GameObject dialogueBox;

    public bool isInteracting = false;
    public bool skipDialogue = false;
    public bool canInteract = false;
    public bool canSkip = false;
    public bool alternateDialogue = false;

    public NPC npcScript;

    List<string> npcs = new List<string>();

    public void OnInteract(InputAction.CallbackContext context)
    {
        if (canInteract)
        {
            if (context.started)
            {
                if (npc.AllNodesCompleted() && !alternateDialogue)
                {
                    npc.deactivateDialogueBox = true;
                    skipDialogue = false;
                    StartAction();
                    alternateDialogue = true;
                    canSkip = true;
                }
                else if (!isInteracting && !npc.AllNodesCompleted())
                {
                    // Start interaction if not already interacting
                    if (interactableInstance != null)
                    {
                        StartAction();
                    }
                    else
                    {
                        Debug.Log("Nothing to interact with");
                    }
                }
                else if (isInteracting && !skipDialogue)
                {
                    if (canSkip)
                    {
                        // Skip dialogue if already interacting and it's a dialogue action
                        if (interactableInstance != null)
                        {
                            skipDialogue = true;
                            interactableInstance.InteractLogic();
                            canSkip = false;
                            alternateDialogue = false;
                        }
                    }                    
                }
                else if (isInteracting && skipDialogue && !npc.AllNodesCompleted())
                {
                    // Check if the event is invoked
                    npc.goToNextAction = true;
                    isInteracting = false;
                    skipDialogue = false;
                    alternateDialogue = false;

                    StartAction();
                    npc.AllNodesCompleted();

                }
            }
        }
    }

    public void StartAction()
    {
        isInteracting = true;
        interactableInstance.InteractLogic();
    }

    public void MoveToNextAction()
    {
        npc.goToNextAction = true;
        StartAction();
    }

    public void SetIInstance(IInteractable interactable)
    {
        Debug.Log("Interactable instance set");

        interactableInstance = interactable;
    }

    public void ClearIInstance()
    {
        Debug.Log("Interactable instance cleared");

        interactableInstance = null;
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Trigger entered");
        npc = other.GetComponent<NPC>();
        string npcName = other.gameObject.name;

        if (npc != null)
        {
            CharacterInstructions instructions = npc.characterInstructions;
            if (instructions != null)
            {
                foreach (InstructionNode node in instructions.Nodes)
                {
                    npcs.Add(node.characterToPerformInstruction);
                }
            }
        }

        if (npcs.Contains(npcName))
        {
            canInteract = true;

            if (interactableInstance == null)
            {
                IInteractable interactable = other.GetComponent<IInteractable>();
                if (interactable != null)
                {
                    SetIInstance(interactable);
                }
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        Debug.Log("Trigger exited");

        if (interactableInstance != null)
        {
            if (npc.AllNodesCompleted())
            {
                foreach (string npcToFind in npcs)
                {
                    GameObject npcObject = GameObject.Find(npcToFind);
                    Debug.Log(npcToFind);
                    if (npcObject != null)
                    {
                        npcScript = npcObject.GetComponent<NPC>();
                        Debug.Log(npcScript);
                        if (npcScript != null)
                        {
                            npcScript.allNodesCompleted = true;
                        }
                    }
                }
            }
            npcs.Clear();
            ClearIInstance();
            dialogueBox.SetActive(false);
        }
    }
}
