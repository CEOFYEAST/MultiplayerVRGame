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
    #region Private Fields

    // gameobject that will be used to send RPC calls over the network
    private GameObject RPCReceiver;

    // view of hand used on last grab
    // - used to identify releasing hand
    private PhotonView lastGrabbingHandIdentifier;

    #endregion


    #region MonoBehaviour callbacks

    /// <summary>
    /// finds the RPC receiver at start
    /// <summary>
    void Start(){
        Debug.Log("Finding RPCReceiver...");

        RPCReceiver = GameObject.Find("RPCReceiver");

        Debug.Log("RPCReceiver: " + RPCReceiver);
    }

    #endregion


    #region Public Methods

    /// <summary>
    /// communicates the grabbing of a basketball over the network 
    /// sends the photon view of the grabbing hand so it's puppets can be located
    /// <summary>
    public void CommunicateGrab(){
        Debug.Log("Communicating Grab");

        // grabs the photon view of the local RPCReceiver
        PhotonView RPCReceiverView = RPCReceiver.GetComponent<PhotonView>();

        // gets the interactor of the hand that grabbed the basketball (the interactable)
        XRBaseInteractor interactor = GetInteractorViaInteractable();

        // gets the hand that grabbed the basketball using its interactor
        GameObject grabbingHand = interactor.gameObject;

        // gets the model of the grabbing hand 
        // - it has the photon view
        GameObject grabbingHandModel = grabbingHand.transform.GetChild(2).gameObject;

        // photon view of the grabbing hand
        // - used to locate said hand on the other puppets
        PhotonView grabbingHandView = grabbingHandModel.GetComponent<PhotonView>();

        Debug.Log("grabbingHandView" + grabbingHandView);

        lastGrabbingHandIdentifier = grabbingHandView;

        // calls either the OnGrab method in every other game
        // the Others target makes sure that every player receives the RPC except the local player 
        RPCReceiverView.RPC("OnGrab", RpcTarget.All, grabbingHandView.ViewID);
    }

    /// <summary>
    /// communicates the release of a basketball over the network 
    /// sends the photon view of the releasing hand so it's puppets can be located
    /// <summary>
    public void CommunicateRelease(){
        Debug.Log("Communicating Release");

        // grabs the rigidbody component of the basketball
        Rigidbody releasedBasketballRigidbody = gameObject.GetComponent<Rigidbody>();

        // grabs the velocity of the basketball's rigidbody
        Vector3 releasedBasketballVelocity = releasedBasketballRigidbody.velocity;

        // grabs the photon view of the local RPCReceiver
        PhotonView RPCReceiverView = RPCReceiver.GetComponent<PhotonView>();

        // calls the OnRelease method in every other game
        // the Others target makes sure that every player receives the RPC except the local player 
        RPCReceiverView.RPC("OnRelease", RpcTarget.All, lastGrabbingHandIdentifier.ViewID, releasedBasketballVelocity);
    }

    #endregion


    #region Private Methods

    /// <summary>
    /// gets an interactor via the interactable its interacting with
    /// - in this case, the basketball this script is attached to is the interactor
    /// - a left or right hand is the only possible interactor
    /// <summary>
    // https://forum.unity.com/threads/how-can-i-get-the-interactor-gameobject-when-the-interactable-gameable-is-grabbed.1293342/
    private XRBaseInteractor GetInteractorViaInteractable(){
        // gets the interactable component of the basketball
        // - used to get the interactor selecting it
        var grabInteractable = gameObject.GetComponent<XRGrabInteractable>();

        // makes sure the grab interactable is actually currently selected by an interactor
        if (grabInteractable.isSelected) {
            // gets the interactor currently interacting with the interactable
            var interactor = grabInteractable.firstInteractorSelecting as XRBaseInteractor;

            // returns said interactor
            return interactor;
        }

        Debug.Log("Interactor is Null");

        return null;
    }

    #endregion
}
