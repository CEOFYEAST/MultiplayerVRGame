using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
Purpose: class that contains logic to spawn a ball when necessary
Function: When a basketball leaves the spawner's trigger, a coroutine to spawn a new basketball after a delay starts.
However, this coroutine is stopped if another basketball enters the spawner's trigger. 
*/
public class BasketballSpawner : MonoBehaviour
{
    public GameObject basketballPrefabReference;

    //calls SpawnBasketballCoroutine if a basketball exits the trigger
    public void OnTriggerExit(Collider other)
    {
        //checks if other is basketball to begin with by checking if a MonoBehavior IsBasketball script is attached to other 
        if(other.GetComponent<IsBasketball>() != null){
            StartCoroutine(SpawnBasketballCoroutine(other.gameObject));
        }
    }

    //cancels the spawning of a basktball in the spawner if a basketball enters the spawner
    public void OnTriggerStay(Collider other)
    {
        if (other.name.Contains("Basketball"))
        {
            StopAllCoroutines();
        }
    }

    //spawns a basketball at the spawner after a delay (1 second)
    IEnumerator SpawnBasketballCoroutine(GameObject basketball)
    {
        yield return new WaitForSeconds(1);

        GameObject newBall = GameObject.Instantiate(basketballPrefabReference);
        
        newBall.transform.position = gameObject.transform.position;
        newBall.GetComponent<Rigidbody>().useGravity = true;
        newBall.GetComponent<Rigidbody>().angularDrag = .05f;
    }
}
