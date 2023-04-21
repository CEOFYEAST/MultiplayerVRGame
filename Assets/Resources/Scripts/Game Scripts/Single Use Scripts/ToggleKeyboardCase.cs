using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.UI;
using TMPro;

public class ToggleKeyboardCase : MonoBehaviour
{
    public GameObject[] keyboardRows = new GameObject[3];

    private bool isUppercase = false;

    public void ToggleCase(){
        // loops through every row in keyboardRows
        foreach (GameObject row in keyboardRows){
            // loops through every child in row
            foreach (Transform letterButton in row.transform){
                // gets text child of child
                TMPro.TextMeshProUGUI letterText = letterButton.GetChild(0).GetComponent<TMPro.TextMeshProUGUI>();

                // gets first character in letterText
                char letter = letterText.text[0];

                // toggles case
                if(isUppercase){
                    letter = Char.ToLower(letter);
                } else {
                    letter = Char.ToUpper(letter);
                }

                // sets text to updated string
                letterText.text = Char.ToString(letter);
            }
        }
        
        isUppercase = !(isUppercase);
    }
}
