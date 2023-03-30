using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

//class meant to toggle interface menu/controllers on pressing primary and secondary buttons on right controller
public class ToggleInterfaceOnButtonPress : MonoBehaviour
{
    public InputActionProperty righthandInterfaceToggle;

    private float lastToggleValue;

    void Start()
    {
        lastToggleValue = 0;
    }

    void Update()
    {
        //reads value from right hand primary and secondary buttons. 1 means pressed and 0 means not unpressed.
        float toggleValue = righthandInterfaceToggle.action.ReadValue<float>();

        //toggles interface/controllers once upon button press. lastToggleValue is implemented to prevent toggle every frame
        if(toggleValue == 1 && lastToggleValue != 1){
            GetComponent<ToggleObjects>().Toggle();
        }

        lastToggleValue = toggleValue;
    }
}
