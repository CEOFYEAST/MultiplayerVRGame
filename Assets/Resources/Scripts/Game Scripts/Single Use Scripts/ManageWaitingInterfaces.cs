using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Photon.Pun;
using Photon.Realtime;

/// <summary> 
/// class desgined to manage waiting room interfaces
/// - hide and show interfaces without disabling them so their respective scripts can still run
/// - give player access to host interfaces if they become host
/// <summary>
public class ManageWaitingInterfaces : MonoBehaviour
{
    public GameObject masterInterface;

    public GameObject nonMasterInterface;

    // stores master status as of last update
    private bool previousMasterStatus = false;

    // Start is called before the first frame update
    void Start()
    {
        previousMasterStatus = PhotonNetwork.IsMasterClient;

        if(PhotonNetwork.IsMasterClient){
            ShowInterface(masterInterface);
        } else {
            ShowInterface(nonMasterInterface);
        }
    }

    // Update is called once per frame
    void Update()
    {
        // updates objects if master client status changes
        if(PhotonNetwork.IsMasterClient != previousMasterStatus){
            previousMasterStatus = PhotonNetwork.IsMasterClient;

            if(PhotonNetwork.IsMasterClient){
                if(gameObject == masterInterface){
                    ShowInterface(gameObject);
                } else {
                    HideInterface(gameObject);
                }
            }
        }
        
    }

    /// <summary> 
    /// sets position of given interface to hidden position
    /// <summary>
    public void HideInterface(GameObject interfaceToHide){
        // variable containing the distance beneath the default location 
        // of the interface to hide the current interface
        float distanceToHide = 10f;

        // default location for the waiting room interface (un-hidden position)
        Vector3 defaultPosition = new Vector3(-1.36000001f,1.32000005f,0f);

        // sets defaultPosition to hidden position
        defaultPosition.y -= distanceToHide;

        // hides current interface
        gameObject.transform.position = defaultPosition;
    }

    /// <summary>
    // sets position of given interface to default position
    /// <summary>
    public void ShowInterface(GameObject interfaceToShow){
        // default location for the waiting room interface (un-hidden position)
        Vector3 defaultPosition = new Vector3(-1.36000001f,1.32000005f,0f);

        // shows new interface
        interfaceToShow.transform.position = defaultPosition;
    }

    public void SwapInterfaces(GameObject interfaceToShow){
        HideInterface(gameObject);

        ShowInterface(interfaceToShow);
    }
}
