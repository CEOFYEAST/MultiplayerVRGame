using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Photon.Pun;
using Photon.Realtime;

/**
Purpose: class that contains logic to spawn a ball when necessary
Function: When a basketball leaves the spawner's trigger, a coroutine to spawn a new basketball after a delay starts.
However, this coroutine is stopped if another basketball enters the spawner's trigger. 
*/
public class IndividualNetworkedBasketballSpawner : MonoBehaviourPunCallbacks
{
    public GameObject basketballPrefab;

    //fills empty trigger at start
    void Start()
    {
        StartCoroutine(SpawnNetworkedBasketballCoroutine(1));
    }

    //calls SpawnBasketballCoroutine if a basketball exits the trigger
    public void OnTriggerExit(Collider other)
    {
        //checks if other is basketball to begin with by checking if a MonoBehavior IsBasketball script is attached to other 
        if(other.GetComponent<IsBasketball>() != null){
            StartCoroutine(SpawnNetworkedBasketballCoroutine(1));
        }
    }

    //cancels the spawning of a basktball in the spawner if a basketball enters the spawner
    public void OnTriggerStay(Collider other)
    {
        if (other.GetComponent<IsBasketball>() != null)
        {
            StopAllCoroutines();
        }
    }

    //spawns a basketball at the spawner after a delay 
    IEnumerator SpawnNetworkedBasketballCoroutine(float delay)
    {
        if(PhotonNetwork.IsConnected){
            yield return new WaitForSeconds(delay);

            //makes sure the spawner belongs to the local player
            if(gameObject.GetComponentInParent<PhotonView>().IsMine){
                Debug.Log("Is Mine");
                GameObject newBall = PhotonNetwork.Instantiate(this.basketballPrefab.name, 
                    gameObject.GetComponent<Transform>().position, 
                    Quaternion.identity, 
                    0);
            } else {
                Debug.Log("Isn't Mine");
            }
        }
    }
}
