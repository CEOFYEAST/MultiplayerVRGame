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

    // Start is called before the first frame update
    void Start()
    {
        if(PlayerPrefs.HasKey(StaticConstants.regionPrefKey)){
            regionDisplayText.text = GetRegionName(PlayerPrefs.GetString(StaticConstants.regionPrefKey));
        } else {
            regionDisplayText.text = "Best Region";
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(PlayerPrefs.HasKey(StaticConstants.regionPrefKey)){
            if(GetRegionName(PlayerPrefs.GetString(StaticConstants.regionPrefKey)) != regionDisplayText.text){
                regionDisplayText.text = GetRegionName(PlayerPrefs.GetString(StaticConstants.regionPrefKey));
            }
        }

        if(PlayerPrefs.HasKey(StaticConstants.playerNamePrefKey)){
            if(PlayerPrefs.GetString(StaticConstants.playerNamePrefKey) != usernameDisplayText.text){
                usernameDisplayText.text = PlayerPrefs.GetString(StaticConstants.playerNamePrefKey);
            }
        }
    }

    private string GetRegionName(string regionKey){
        switch(regionKey){
            case "best":
                return "Best Region";
                break;
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
