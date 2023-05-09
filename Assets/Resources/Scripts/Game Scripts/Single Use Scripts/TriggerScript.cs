using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Photon.Pun;
using Photon.Realtime;

public class TriggerScript : MonoBehaviour
{
    private GameObject RPCReceiver;

    void Start(){
        Debug.Log("Finding RPCReceiver...");

        RPCReceiver = GameObject.Find("RPCReceiver");

        Debug.Log("RPCReceiver: " + RPCReceiver);
    }
    
    public void OnTriggerEnter(Collider other)
    {
        // makes sure the calling object is a basketball
        if(other.GetComponent<IsBasketball>() != null)
        {
            // grabs the photon view of the basketball
            PhotonView basketballView = other.GetComponent<PhotonView>();

            // grabs the owner of the photon view of the basketball
            Player basketballViewOwner = basketballView.Owner;

            // only updates scores if the basketball is mine
            if(basketballView.IsMine){
                // grabs the photon view of the local RPCReceiver
                PhotonView RPCReceiverView = RPCReceiver.GetComponent<PhotonView>();

                // destroys the scoring basketball so it can only count for one score
                PhotonNetwork.Destroy(other.gameObject);

                // updates scores/scoreboards over the network
                RPCReceiverView.RPC("OnScore", RpcTarget.All, basketballViewOwner);
            }
        }

    }
}
