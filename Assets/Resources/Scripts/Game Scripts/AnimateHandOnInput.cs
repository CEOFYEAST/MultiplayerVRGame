using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

using Photon.Pun;
using Photon.Realtime;

public class AnimateHandOnInput : MonoBehaviourPun
{
    public InputActionProperty pinchAnimationAction;
    public InputActionProperty gripAnimationAction;

    [HideInInspector]
    public Animator handAnimator;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //makes sure the animator is only animating the local player's hand. PhotonView will take care of animating the other players' hands
        if (photonView.IsMine == false && PhotonNetwork.IsConnected == true)
        {
            return;
        }

        //the value is a float because the action associated with the pinch animation is a float, as opposed to a bool
        float triggerValue = pinchAnimationAction.action.ReadValue<float>();
        handAnimator.SetFloat("Trigger", triggerValue);
        //Debug.Log(triggerValue);

        float gripValue = gripAnimationAction.action.ReadValue<float>();
        handAnimator.SetFloat("Grip", gripValue);
    }
}
