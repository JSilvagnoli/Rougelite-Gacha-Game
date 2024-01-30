using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using System;
using Unity.VisualScripting;

public class DialogueAction : INodeAction
{
    private InstructionNode node;
    private InteractWithNPC interactWithNPC;

    private float textSpeed = 0.08f;

    private string skippedText = "";

    public DialogueAction(InstructionNode node)
    {
        this.node = node;
    }

    // We must provide this public method to satisfy the interface
    public IEnumerator Execute()
    {
        GameObject dialogueTextBox = GameObject.Find("Dialogue Text");
        TextMeshProUGUI textComponent = dialogueTextBox.GetComponent<TextMeshProUGUI>();

        GameObject player = GameObject.FindGameObjectWithTag("Character");
        interactWithNPC = player.GetComponent<InteractWithNPC>();

        if (interactWithNPC != null)
        {
            if (!interactWithNPC.skipDialogue)
            {
                textComponent.text = "";

                foreach (char c in node.dialogueInformation.dialogueLine.ToCharArray())
                {
                    if (textComponent != null)
                    {
                        textComponent.text += c;
                    }

                    yield return new WaitForSeconds(textSpeed);
                }

                interactWithNPC.skipDialogue = true;
            }
            else
            {
                skippedText = "";
                skippedText = node.dialogueInformation.dialogueLine;
                textComponent.text = skippedText;
            }
        }
    }
}
