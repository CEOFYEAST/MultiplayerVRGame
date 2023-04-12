using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// built to toggle specified interactor menu and direct/ray controllers
public class ToggleObjects : MonoBehaviour
{
    public GameObject[] objectsToToggle;

    public void Toggle()
    {
        foreach(GameObject toToggle in objectsToToggle){
            //sets active status to opposite of current active status
            toToggle.SetActive(!toToggle.activeSelf);
        }
    }
}
