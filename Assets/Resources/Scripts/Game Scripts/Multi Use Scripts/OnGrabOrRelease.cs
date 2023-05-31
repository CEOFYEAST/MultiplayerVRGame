using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Photon.Pun;
using Photon.Realtime;

using UnityEngine.XR.Interaction.Toolkit;

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
    /// sends the photon view of the grabbing hand so it's puppets can be located
    /// <summary>
    public void OnGrab(){
        // grabs the photon view of the local RPCReceiver
        PhotonView RPCReceiverView = RPCReceiver.GetComponent<PhotonView>();

        // gets the interactor of the hand that grabbed the basketball
        XRBaseInteractor interactor = gameObject.GetComponent<XRGrabInteractable>().interactorsSelecting[0] as XRBaseInteractor;

        // gets the hand that grabbed the basketball using its interactor
        GameObject grabbingHand = interactor.gameObject;

        // gets the model of the grabbing hand 
        // - it has the photon view
        GameObject grabbingHandModel = grabbingHand.transform.GetChild(2).gameObject;

        // photon view of the grabbing hand
        // - used to locate said hand on the other puppets
        PhotonView grabbingHandView = grabbingHandModel.GetComponent<PhotonView>();

        // calls the OnGrab method in every other game
        // the Others target makes sure that every receiving player 
        RPCReceiverView.RPC("OnGrab", RpcTarget.Others, grabbingHandView.ViewID);
    }
}
