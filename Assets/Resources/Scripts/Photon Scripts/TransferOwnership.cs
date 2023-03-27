using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Photon.Pun;
using Photon.Realtime;

/// <summary>
/// class designed to transfer ownership of the basketball to the client that calls TransferOwnership
/// <summary>
public class TransferOwnership : MonoBehaviourPun
{
    [Tooltip("Set to 1 to call TransferOwnership, used for testing")]
    public int tryTransfer = 0;

    //used to prevent multiple TransferOwnership calls in update 
    private int lastInput = 0;

    void Update(){
        if(tryTransfer == 1 && lastInput != 1){
            lastInput = 1;
            TryTransferOwnership();
        } else if(tryTransfer == 0){
            lastInput = 0;
        }
    }

    public void TryTransferOwnership(){
        //makes sure basketball's owner isn't already the player
        if (!(gameObject.GetComponent<PhotonView>().Owner.ActorNumber == PhotonNetwork.LocalPlayer.ActorNumber))
        {
            Debug.Log("Photon View isn't mine");
            
            //sets basketball's owner to be the player
            gameObject.GetComponent<PhotonView>().TransferOwnership(PhotonNetwork.LocalPlayer);
        }
    }
}
