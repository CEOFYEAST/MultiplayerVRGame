using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerScript : MonoBehaviour
{
    public GameObject eventCamera;
    
    public void OnTriggerEnter(Collider other)
    {
        if(other.GetComponent<IsBasketball>() != null)
        {
            eventCamera.GetComponent<UpdateText>().TextChange();
        }

    }
}
