using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerScript : MonoBehaviour
{
    public GameObject eventCamera;
    
    public void OnTriggerEnter(Collider other)
    {
        if (other.name.Contains("Basketball"))
        {
            eventCamera.GetComponent<UpdateText>().TextChange();
        }

    }
}
