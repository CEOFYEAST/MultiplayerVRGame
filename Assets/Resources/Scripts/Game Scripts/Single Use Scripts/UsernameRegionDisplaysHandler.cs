using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.UI;
using TMPro;

using Photon.Pun;
using Photon.Realtime;

public class UsernameRegionDisplaysHandler : MonoBehaviour
{
    public TMPro.TextMeshProUGUI regionDisplayText;
    public TMPro.TextMeshProUGUI usernameDisplayText;

    // stores the region pref key to avoid typos
    const string regionPrefKey = "RegionPreference";

    // stores the username pref Key to avoid typos
    const string playerNamePrefKey = "PlayerName";

    // Start is called before the first frame update
    void Start()
    {
        if(PlayerPrefs.HasKey(regionPrefKey)){
            regionDisplayText.text = GetRegionName(PlayerPrefs.GetString(regionPrefKey));
        } else {
            regionDisplayText.text = "Best Region";
        }

        if(PlayerPrefs.HasKey(playerNamePrefKey)){
            usernameDisplayText.text = PlayerPrefs.GetString(playerNamePrefKey);
        } else {
            regionDisplayText.text = "Player";
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(PlayerPrefs.HasKey(regionPrefKey)){
            if(GetRegionName(PlayerPrefs.GetString(regionPrefKey)) != regionDisplayText.text){
                regionDisplayText.text = GetRegionName(PlayerPrefs.GetString(regionPrefKey));
            }
        }

        if(PlayerPrefs.HasKey(playerNamePrefKey)){
            if(PlayerPrefs.GetString(playerNamePrefKey) != usernameDisplayText.text){
                usernameDisplayText.text = PlayerPrefs.GetString(playerNamePrefKey);
            }
        }
    }

    private string GetRegionName(string regionKey){
        switch(regionKey){
            case "asia":
                return "Asia";
                break;
            case "au":
                return "Australia";
                break;
            case "cae":
                return "Canada, East";
                break;
            case "eu":
                return "Europe";
                break;
            case "in":
                return "India";
                break;
            case "jp":
                return "Japan";
                break;
            case "za":
                return "South Africa";
                break;
            case "sa":
                return "South America";
                break;
            case "kr":
                return "South Korea";
                break;
            case "tr":
                return "Turkey";
                break;
            case "us":
                return "USA, East";
                break;
            case "usw":
                return "USA, West";
                break;
            default: 
                return "error";
                break;
        }
    }
}
