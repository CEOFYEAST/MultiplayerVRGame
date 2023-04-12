using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwitchInterfacesOnConnecting : MonoBehaviour
{
    public GameObject initialInterface;
    public GameObject connectingInterface;

    public void Switch(){
        initialInterface.SetActive(false);
        connectingInterface.SetActive(true);
    }
}
