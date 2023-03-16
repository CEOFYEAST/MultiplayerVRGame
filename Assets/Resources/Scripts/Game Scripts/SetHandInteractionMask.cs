using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction;


public class SetHandInteractionMask : MonoBehaviour
{
    public GameObject leftHand;
    public GameObject rightHand;

    public void onGrab(GameObject selectorHand)
    {
        if(selectorHand.name.Contains("Left"))
        {
            disable(rightHand);
        } else
        {
            disable(leftHand);
        }
    }

    public void enableBoth()
    {
        enable(leftHand);
        enable(rightHand);
    }

    public void disableBoth()
    {
        disable(leftHand);
        disable(rightHand);
    }

    public void disable(GameObject toDisable)
    {
        toDisable.GetComponent<UnityEngine.XR.Interaction.Toolkit.XRDirectInteractor>().interactionLayers = UnityEngine.XR.Interaction.Toolkit.InteractionLayerMask.GetMask("None");
    }

    public void enable(GameObject toEnable)
    {
        toEnable.GetComponent<UnityEngine.XR.Interaction.Toolkit.XRDirectInteractor>().interactionLayers = UnityEngine.XR.Interaction.Toolkit.InteractionLayerMask.GetMask("Default");
    }
}
