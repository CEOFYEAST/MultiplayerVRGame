using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Photon.Pun;
using Photon.Realtime;

/// <summary>
/// class representing an input manager that stops the player from sending inputs to rigs that don't belong to them
/// <summary>
public class MyInputManagerViewTest : MonoBehaviourPun
{
    //array containing all ActionBasedController components in the scene
    private UnityEngine.XR.Interaction.Toolkit.ActionBasedController[] handControllers;

    /// <summary>
    /// function meant to disable all ActionBasedController components that don't belong to the player in order to prevent bad inputs
    /// <summary>
    public void CheckHands(){
        //fills handControllers
        handControllers = GameObject.FindObjectsOfType<UnityEngine.XR.Interaction.Toolkit.ActionBasedController>();

        Debug.Log(handControllers.Length);

        foreach(UnityEngine.XR.Interaction.Toolkit.ActionBasedController handController in handControllers){
            //gets PhotonView of handController, or if that doesn't exist, gets closest PhotonView up the parent tree
            PhotonView handView = handController.GetComponentInParent<PhotonView>();

            //checks for controllers that aren't mine
            if(!handView.IsMine){
                handController.enabled = false;
            }
        }
    }

    /// <summary> 
    /// function meant to disable all components on a rig that doesn't belong to the player
    /// used so I can selectively enable components that don't screw with input
    /// <summary>
    public void DisableComponentsInNonLocalRig(GameObject rig){
        //photon view of rig being checked
        PhotonView rigView = rig.GetComponent<PhotonView>();

        // checks if the parent rig belongs to the local player and disables all components if it doesn't
        if(!rigView.IsMine){
            SelectivelyDisableComponentsRecursively(rig);
        }
    }

    /// <summary>
    /// function meant to disable all components on a GameObject and it's children while avoiding disabling certain components
    /// written by ChatGPT
    /// <summary>
    void SelectivelyDisableComponentsRecursively(GameObject obj)
    {
        foreach (Behaviour behaviour in obj.GetComponents<Behaviour>())
        {
            Debug.Log(behaviour.GetType());

            //skips PhotonTransformView and PhotonAnimatorView components
            if ((behaviour.GetType() == typeof(PhotonTransformView)) || (behaviour.GetType() == typeof(PhotonAnimatorView)))
            {
                continue;
            }

            behaviour.enabled = false;
        }

        for (int i = 0; i < obj.transform.childCount; i++)
        {
            SelectivelyDisableComponentsRecursively(obj.transform.GetChild(i).gameObject);
        }
    }
}
