using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class DialogueManager : MonoBehaviour
{
    PlayerInputActions inputActions;

    private void Start()
    {
        inputActions = new PlayerInputActions();

        inputActions.DialougeActions.NextDialouge.performed += NextDialouge;
    }

    public void StartDialouge()
    {
        PlayerController.Instance.SetInputSystem(false);
        inputActions.DialougeActions.Enable();
    }

    public void EndDialouge()
    {
        PlayerController.Instance.SetInputSystem(true);
        inputActions.DialougeActions.Disable();
    }

    private void NextDialouge(InputAction.CallbackContext context)
    {
        
    }
}
