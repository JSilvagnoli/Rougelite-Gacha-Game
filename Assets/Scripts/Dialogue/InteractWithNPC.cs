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

    private bool isInteracting = false;
    public bool skipDialogue = false;

    private void Awake()
    {
        dialogueBox.SetActive(false);
    }

    public void OnInteract(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            if (npc.AllNodesCompleted())
            {
                Debug.Log("All nodes completed. No further interaction allowed.");
                return;
            }

            if (!isInteracting)
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
                // Skip dialogue if already interacting and it's a dialogue action
                if (interactableInstance != null)
                {
                    skipDialogue = true;
                    interactableInstance.InteractLogic();
                }
                else
                {
                    Debug.Log("Interactable instance is null?");
                }
            }
            else if (isInteracting && skipDialogue)
            {
                // Check if the event is invoked
                npc.goToNextAction = true;
                isInteracting = false;
                skipDialogue = false;

                StartAction();
            }
        }
    }

    public void StartAction()
    {
        dialogueBox.SetActive(true);
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
        interactableInstance = interactable;
    }

    public void ClearIInstance()
    {
        interactableInstance = null;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (interactableInstance == null)
        {
            IInteractable interactable = other.GetComponent<IInteractable>();
            if (interactable != null)
            {
                SetIInstance(interactable);
            }
        }

        npc = other.GetComponent<NPC>();
    }

    private void OnTriggerExit(Collider other)
    {
        ClearIInstance();
        dialogueBox.SetActive(false);
        isInteracting = false;
    }
}
