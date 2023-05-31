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
    /// <summary>
    public void OnGrab(){
        // grabs the photon view of the local RPCReceiver
        PhotonView RPCReceiverView = RPCReceiver.GetComponent<PhotonView>();

        // gets the interactor of the hand that grabbed the basketball
        XRBaseInteractor interactor = gameObject.GetComponent<XRGrabInteractable>().interactorsSelecting[0] as XRBaseInteractor;

        Debug.Log("interactor: " + interactor);

        // gets the hand that grabbed the basketball using its interactor
        GameObject grabbingHand = interactor.gameObject;

        Debug.Log("hand: " + grabbingHand);

        GameObject grabbingHandModel = grabbingHand.transform.GetChild(2).gameObject;

        // photon view of the grabbing hand
        // - used to locate said hand on the other puppets
        PhotonView grabbingHandView = grabbingHandModel.GetComponent<PhotonView>();

        Debug.Log("hand view: " + grabbingHandView);

        RPCReceiverView.RPC("OnGrab", RpcTarget.All, grabbingHandView.ViewID);
    }
}
