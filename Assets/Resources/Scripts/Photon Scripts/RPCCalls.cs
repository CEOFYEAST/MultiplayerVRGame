using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Photon.Pun;
using Photon.Realtime;

/// <summary>
/// class designed to contain RPC methods to allow communication between players in the same room
/// <summary>
public class RPCCalls : MonoBehaviour
{
    private GameObject gameManager;

    void Start(){
        gameManager = GameObject.Find("Game Manager");
    }

    /// <summary>
    /// called when a basket is scored by a locally instantiated basketball
    /// <summary>
    [PunRPC]
    public void OnScore(Player scorer){
        Debug.Log("RPC Method Called");

        // calls the local OnScore method
        gameManager.GetComponent<MyGameLoop>().OnScore(scorer);
    }

    /// <summary>
    /// called when a ball is grabbed by another player
    /// <summary>
    [PunRPC]
    public void OnGrab(int grabbingHandViewID){
        Debug.Log("RPC Method Called");

        // calls the local OnGrab method
        gameManager.GetComponent<MyGameManager>().OnGrab(grabbingHandViewID);
    }
}
