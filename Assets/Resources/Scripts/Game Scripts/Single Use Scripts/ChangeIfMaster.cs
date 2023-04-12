using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.UI;
using TMPro;

using Photon.Pun;
using Photon.Realtime;

public class ChangeIfMaster : MonoBehaviour
{
    // menu objects to update on ownership change
    public TMPro.TextMeshProUGUI menuTip;
    public GameObject playButtonTip;

    // possible text choices for menu tip
    private string isMasterText = "You are the host. You choose when to start the game, but there needs to be at least two players";
    private string isNotMasterText = "Wait for the host to start the game";

    // stores master status as of last update
    private bool previousMasterStatus = false;

    // Start is called before the first frame update
    void Start()
    {
        previousMasterStatus = PhotonNetwork.IsMasterClient;
        UpdateObjects();
    }

    // Update is called once per frame
    void Update()
    {
        // updates objects if master client status changes
        if(PhotonNetwork.IsMasterClient != previousMasterStatus){
            previousMasterStatus = PhotonNetwork.IsMasterClient;
            UpdateObjects();
        }
        
    }

    /// <summary>
    /// updates menu tip contents and play button visibilty to reflect current host status
    /// <summary>
    void UpdateObjects(){
        if(PhotonNetwork.IsMasterClient){
            menuTip.text = isMasterText;
            playButtonTip.SetActive(true);

            return;
        }

        menuTip.text = isNotMasterText;
        playButtonTip.SetActive(false);
    }
}
