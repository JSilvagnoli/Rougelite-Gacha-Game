using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class DialogueController : MonoBehaviour
{
    public bool isClicking = false;

    public Dialogue dialogue;

    public void OnClick(InputAction.CallbackContext context)
    {
        if (context.started && dialogue.dialogue == true)
        {
            isClicking = true;
        }
    }
}
