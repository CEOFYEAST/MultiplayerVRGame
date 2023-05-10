using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.UI;
using TMPro;

using Photon.Pun;
using Photon.Realtime;

public class GameSettingsDisplaysHandler : MonoBehaviour
{
    [SerializeField]
    private TMPro.TextMeshProUGUI[] displayTextsInOrder = new TMPro.TextMeshProUGUI[3];

    string[] roomPropertiesKeysInOrder = 
    {
        "WarmupLength",
        "GameLength",
        "PointsPerScore"
    };

    // Update is called once per frame
    void Update()
    {
        for(int i = 0; i < 3; i++){
            if(PhotonNetwork.CurrentRoom.CustomProperties.ContainsKey(roomPropertiesKeysInOrder[i])){
                // true if the current warmup length doesn't match the contents of the warmup length display 
                if(!(String.Equals(PhotonNetwork.CurrentRoom.CustomProperties[roomPropertiesKeysInOrder[i]].ToString(), displayTextsInOrder[i].text))){
                    displayTextsInOrder[i].text = PhotonNetwork.CurrentRoom.CustomProperties[roomPropertiesKeysInOrder[i]].ToString();
                }
            }
        }
    }
}
