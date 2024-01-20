using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class DialogueController : MonoBehaviour
{
    public bool isClicking = false;

    public GameObject dialogueBox;

    public Dialogue dialogue;

    private void Start()
    {
        dialogueBox = GameObject.FindGameObjectWithTag("DialogueBox");

        Transform childTransform = dialogueBox.transform.Find("Dialogue Box");

        dialogue = childTransform.GetComponent<Dialogue>();
    }

    public void OnClick(InputAction.CallbackContext context)
    {
        if (context.started && dialogue.dialogue == true)
        {
            isClicking = true;
        }
    }
}
