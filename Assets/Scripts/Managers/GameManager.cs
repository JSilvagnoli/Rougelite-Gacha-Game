using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public CharacterInstructions tutorialInstructions;
    public InstructionManager instructionManager;

    public bool tutorialCompleted = false;

    private void Update()
    {
        instructionManager = GetComponent<InstructionManager>();

        foreach (InstructionNode node in tutorialInstructions.Nodes)
        {
            if (!tutorialCompleted)
            {
                instructionManager.actions.Add(node);
                
            }
        }
        tutorialCompleted = true;
    }
}