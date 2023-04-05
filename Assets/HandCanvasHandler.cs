using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class HandCanvasHandler : MonoBehaviour
{
    // camera to face
    public Transform playerCamera;

    // button press input used to disable canvas
    public InputActionProperty righthandInterfaceToggle;

    // Update is called once per frame
    void Update()
    {
        // Make the interface always face the player
        transform.LookAt(playerCamera);

        // reads value from right hand primary and secondary buttons. 1 means pressed and 0 means not unpressed.
        float toggleValue = righthandInterfaceToggle.action.ReadValue<float>();

        if(toggleValue == 1){
            gameObject.SetActive(false);
        }
    }
}
