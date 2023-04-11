using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.UI;
using TMPro;

public class AlterUsernameText : MonoBehaviour
{
    public TMPro.TextMeshProUGUI usernameText;

    /// <summary>
    /// converts a given string to a list of chars
    /// <summary>
    private List<char> StringToCharList(string toConvert){
        List<char> toReturn = new List<char>();
        toReturn.AddRange(toConvert);

        return toReturn;
    }

    /// <summary> 
    /// adds the contents of toAdd onto the end of usernameText
    /// <summary>
    public void Add(TMPro.TextMeshProUGUI toAdd){
        // grabs current username
        string currentUsername = usernameText.text;

        // adds toAdd onto usernameText
        if(currentUsername.Length <= 12){
            usernameText.text = currentUsername + toAdd.text;
        }
    }

    public void Delete(){
        // grabs current username
        string currentUsername = usernameText.text;

        // removes last char in usernameText
        if(currentUsername.Length != 0){
            usernameText.text = currentUsername.Substring(0, currentUsername.Length - 1);
        }
        
    }

    public void Clear(){
        usernameText.text = "";
    }
}
