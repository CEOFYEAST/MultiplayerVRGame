using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerScript : MonoBehaviour
{
    private GameObject gameManager;

    void Start(){
        gameManager = GameObject.Find("Game Manager");
    }
    
    public void OnTriggerEnter(Collider other)
    {
        if(other.GetComponent<IsBasketball>() != null)
        {
            Debug.Log("Game Manager: " + gameManager);
            Debug.Log("My Game Loop: " + gameManager.GetComponent<MyGameLoop>());
            gameManager.GetComponent<MyGameLoop>().OnScore(other.gameObject);
        }

    }
}
