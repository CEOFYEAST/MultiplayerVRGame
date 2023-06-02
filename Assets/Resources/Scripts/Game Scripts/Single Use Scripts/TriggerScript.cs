using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Photon.Pun;
using Photon.Realtime;


/// <summary>
/// class designed to communicate a score over the network so other players' scoreboards can be updated properly 
/// <summary>
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
            // grabs the photon view of the local RPCReceiver
            PhotonView RPCReceiverView = RPCReceiver.GetComponent<PhotonView>();

            // grabs the owner of the basketball
            Player basketballOwner = PhotonNetwork.LocalPlayer;

            // destroys the scoring basketball so it can only count for one score
            Destroy(other.gameObject);

            // updates scores/scoreboards over the network
            RPCReceiverView.RPC("OnScore", RpcTarget.All, basketballOwner);
        }

    }
}
