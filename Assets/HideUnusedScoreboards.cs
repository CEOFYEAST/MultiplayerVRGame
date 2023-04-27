using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Photon.Pun;
using Photon.Realtime;

/// <summary>
/// class designed to hide all scoreboards in the game room that don't correspond to an active team
/// <summary>
public class HideUnusedScoreboards : MonoBehaviour
{
    private List<int> activeTeamsList = new List<int>();

    #region Private Constants

        // store the team number custom properties key to avoid typos
        const string teamNumberHashmapKey = "TeamNumber";
        
    #endregion

    /// <summary>
    /// method that hides all scoreboards that don't correspond to an active team
    /// <summary>
    public void Hide(){
        // adds the numbers of all active teams to a list
        foreach(Player player in PhotonNetwork.PlayerList){
            int playerTeamNumber = (int) player.CustomProperties[teamNumberHashmapKey];

            activeTeamsList.Add(playerTeamNumber);
        }

        // disables the scoreboards of teams that don't show up in the active teams list
        for(int i = 0; i < 6; i++){
            if(!(activeTeamsList.Contains(i))){
                transform.GetChild(i).gameObject.SetActive(false);
            }
        }
    }
}
