using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InteractWithNPC : MonoBehaviour
{
    private HashSet<GameObject> uniqueNPCs = new HashSet<GameObject>();

    public GameObject dialogueBox;

    public bool isInteracting = false;

    public float interactWithNPCRange = 2.0f;
    public float showInteractPrompt = 10.0f;

    private Dictionary<GameObject, bool> canInteractMap = new Dictionary<GameObject, bool>();

    private EnemyHealth enemyHealth;
    public Dialogue dialogue;

    public GameObject interactButtonGameObject;
    public Transform interactButton;

    private void Start()
    {
        GameObject dialogueCanvas = GameObject.FindGameObjectWithTag("DialogueBox");

        Transform dialogueBoxTransform = dialogueCanvas.transform.Find("Dialogue Box");
        dialogueBox = dialogueBoxTransform.gameObject;

        dialogue = dialogueBox.GetComponent<Dialogue>();
    }

    public void OnInteract(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            foreach (GameObject npc in uniqueNPCs)
            {
                float distanceToInteractWithNPC = Vector3.Distance(transform.position, npc.transform.position);

                if (canInteractMap[npc] && distanceToInteractWithNPC <= interactWithNPCRange)
                {
                    if (!isInteracting)
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

        GameObject[] foundNPCs = GameObject.FindGameObjectsWithTag("NPC");
        foreach (GameObject npc in foundNPCs)
        {
            uniqueNPCs.Add(npc);
        }

        foreach (GameObject npc in uniqueNPCs)
        {
            float distanceToNPC = Vector3.Distance(playerPosition, npc.transform.position);

            if (distanceToNPC <= showInteractPrompt)
            {
                if (npc.CompareTag("NPC")) //|| (npc.CompareTag("Enemy NPC") && npc.GetComponent<EnemyHealth>()?.hasBeenDefeated == true))
                {
                    if (!isInteracting)
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
        interactButton = npc.transform.Find("Interact Button");

        if (interactButton != null)
        {
            interactButtonGameObject = interactButton.gameObject;
            interactButtonGameObject.SetActive(true);
            canInteractMap[npc] = true;
        }
    }

    private void DisableInteractButton(GameObject npc)
    {
        Transform interactButton = npc.transform.Find("Interact Button");

        if (interactButton != null)
        {
            interactButtonGameObject = interactButton.gameObject;
            interactButtonGameObject.SetActive(false);
            canInteractMap[npc] = false;
        }
    }

    private void InteractWithNPCS(GameObject npc)
    {
        int npcIndex = new List<GameObject>(uniqueNPCs).IndexOf(npc);

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
