using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class InteractWithNPC : MonoBehaviour
{
    public List<GameObject> nearbyNPCs = new List<GameObject>();

    public float detectionRadius = 5.0f;

    public GameObject interactButton;
    public GameObject dialogueBox;
    Transform interactButtonTransform;

    public bool isInteracting = false;

    public CharacterInstructions characterInstructions;
    private PlayerMovement playerMovement;

    private void Start()
    {
        dialogueBox = GameObject.Find("Dialogue Box");

        dialogueBox.SetActive(false);
    }

    public void OnInteract(InputAction.CallbackContext context)
    {
        if (context.started && !isInteracting)
        {
            foreach (GameObject npc in nearbyNPCs)
            {
                float distanceToNPC = Vector3.Distance(transform.position, npc.transform.position);

                if (distanceToNPC < detectionRadius)
                {
                    // Play the instructions for this NPC
                    characterInstructions = npc.GetComponent<CharacterInstructions>();

                    if (characterInstructions != null )
                    {
                        playerMovement = GetComponent<PlayerMovement>();

                        if (playerMovement != null)
                        {
                            playerMovement.enabled = false;
                        }

                        isInteracting = true;
                        dialogueBox.SetActive(true);
                        characterInstructions.PerformNextInstruction();
                    }
                }
            }
        }
        else if (context.started && isInteracting)
        {
            if (characterInstructions.currentIndex < characterInstructions.actions.Count)
            {
                characterInstructions.SkipDialogue();
                isInteracting = false;
            }
        }
    }

    private void Update()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, detectionRadius);

        nearbyNPCs.Clear();

        foreach (Collider collider in colliders)
        {
            if (collider.CompareTag("NPC"))
            {
                nearbyNPCs.Add(collider.gameObject);
            }
        }

        UpdateInteractButtons();

        if (playerMovement != null && characterInstructions.currentIndex == characterInstructions.actions.Count)
        {
            playerMovement.enabled = true;
        }
    }

    private void UpdateInteractButtons()
    {
        foreach (GameObject npc in nearbyNPCs)
        {
            // Check if the NPC has an interact button
            interactButtonTransform = npc.transform.Find("Interact Button");

            if (interactButtonTransform != null)
            {
                interactButton = interactButtonTransform.gameObject;

                float distanceToNPC = Vector3.Distance(transform.position, npc.transform.position);

                if (distanceToNPC <= detectionRadius)
                {
                    interactButton.SetActive(true);
                }
                else
                {
                    interactButton.SetActive(false);
                    dialogueBox.SetActive(false);
                    isInteracting = false;
                }
            }
        }
    }
}
