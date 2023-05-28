using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Photon.Pun;
using Photon.Realtime;

/// <summary>
/// class meant to run particular methods on grab or release of a basketball
/// - used in networking of the basketballs and calling of RPCs
/// <summary>
public class OnGrabOrRelease : MonoBehaviour
{
    // gameobject that will be used to send RPC calls over the network
    private GameObject RPCReceiver;

    /// <summary>
    /// finds the RPC receiver at start
    /// <summary>
    void Start(){
        Debug.Log("Finding RPCReceiver...");

        RPCReceiver = GameObject.Find("RPCReceiver");

        Debug.Log("RPCReceiver: " + RPCReceiver);
    }

    /// <summary>
    /// communicates a grab action over the network
    /// <summary>
    public void OnGrab(){
        // grabs the photon view of the local RPCReceiver
        PhotonView RPCReceiverView = RPCReceiver.GetComponent<PhotonView>();

        // photon view of the grabbing hand
        // - used to locate said hand on the other puppets
        PhotonView grabbingHandView = gameObject.GetComponent<PhotonView>();

        RPCReceiverView.RPC("OnGrab", RpcTarget.All, grabbingHandView);
    }
}
