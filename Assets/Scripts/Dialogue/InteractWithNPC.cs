using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InteractWithNPC : MonoBehaviour
{
    public List<GameObject> npcs;

    public GameObject dialogueBox;

    public bool isInteracting = false;

    public float interactWithNPCRange = 2.0f;
    public float showInteractPrompt = 10.0f;

    private Dictionary<GameObject, bool> canInteractMap = new Dictionary<GameObject, bool>();

    private EnemyHealth enemyHealth;
    public Dialogue dialogue;

    public void OnInteract(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            foreach (GameObject npc in npcs)
            {
                float distanceToInteractWithNPC = Vector3.Distance(transform.position, npc.transform.position);

                if (canInteractMap[npc] && distanceToInteractWithNPC <= interactWithNPCRange)
                {
                    if (isInteracting == false)
                    {
                        isInteracting = true;
                        InteractWithNPCS(npc);
                        break;
                    }
                }
            }
        }
    }

    private void Update()
    {
        Vector3 playerPosition = transform.position;

        foreach (GameObject npc in npcs)
        {
            float distanceToNPC = Vector3.Distance(playerPosition, npc.transform.position);

            if (distanceToNPC <= showInteractPrompt)
            {
                if (npc.CompareTag("NPC") || (npc.CompareTag("Enemy NPC") && npc.GetComponent<EnemyHealth>()?.hasBeenDefeated == true))
                { 
                    if (isInteracting == false)
                    {
                        EnableInteractButton(npc);
                    }
                    else
                    {
                        DisableInteractButton(npc);
                    }
                }
                else
                {
                    DisableInteractButton(npc);
                }
            }
            else
            {
                DisableInteractButton(npc);
            }
        }
    }

    private void EnableInteractButton(GameObject npc)
    {
        Transform interactButton = npc.transform.Find("Interact Button");

        if (interactButton != null)
        {
            interactButton.gameObject.SetActive(true);
            canInteractMap[npc] = true;
        }
    }

    private void DisableInteractButton(GameObject npc)
    {
        Transform interactButton = npc.transform.Find("Interact Button");

        if (interactButton != null)
        {
            interactButton.gameObject.SetActive(false);
            canInteractMap[npc] = false;
        }
    }

    private void InteractWithNPCS(GameObject npc)
    {
        int npcIndex = npcs.IndexOf(npc);

        dialogue.npcIndex = npcIndex;

        dialogueBox.SetActive(true);
        StartCoroutine(StartDialogueDelay());
    }

    private IEnumerator StartDialogueDelay()
    {
        yield return new WaitForSeconds(0.1f);

        dialogue.textComponent.text = string.Empty;

        dialogue.StartDialogue();
    }
}
