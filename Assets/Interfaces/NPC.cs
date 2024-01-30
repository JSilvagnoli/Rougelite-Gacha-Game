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
    private InteractWithNPC interactWithNPC;

    private Coroutine currentCoroutine = null;

    public bool goToNextAction = false;

    // This is for testing purposes only
    Dictionary<InstructionNode, bool> nodeCompletionStatus = new Dictionary<InstructionNode, bool>();
    InstructionNode currentNode = null;

    private void Start()
    {
        GetNodes();
    }

    public void InteractLogic()
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
            else
            {
                Debug.Log("No current node found or all nodes completed.");
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
        foreach (bool completionStatus in nodeCompletionStatus.Values)
        {
            if (!completionStatus)
            {
                return false;
            }
        }

        return true;
    }
}
