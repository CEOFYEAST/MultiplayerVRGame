using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.UI;
using TMPro;

public class MyGameLoop : MonoBehaviour
{
    #region Private Serializable Fields

    /// text objects used to display game and start timers
    [SerializeField]
    private TMPro.TextMeshProUGUI popupText;

    [SerializeField]
    private TMPro.TextMeshProUGUI timerText;

    /// trigger that tracks goals
    [SerializeField]
    private GameObject scoreTrigger;

    #endregion


    #region Unity Monobehaviour Callbacks

    // Start is called before the first frame update
    void Start()
    {
        StartGameLoop();
    }

    #endregion


    #region Private Methods

    private void StartGameLoop(){
        // prevents players from scoring during the warmup
        scoreTrigger.SetActive(false);

        // starts the timer
        StartCoroutine(Timer(popupText, "Warmup ending in ", 15, BlockOne));
    }

        /// <summary>
        /// methods that constitute the game loop, which 
        ///  - gameloop is broken up into blocks that can be called individually, allows for timers
        /// <summary>
        #region Game Loop Blocks

        private void BlockOne(){
            // disables popup text for the time being
            popupText.transform.parent.gameObject.SetActive(false);

            // allows players to score 
            scoreTrigger.SetActive(true);

            // starts game timer
            StartCoroutine(Timer(timerText, "", 60, BlockTwo));
        }

        private void BlockTwo(){

        }

        #endregion

    #endregion


    #region Private Inumerators

    /// <summary>
    /// timer used during game start and during the game that  
    /// - counts down from a given length (length)
    /// - updates a given text object (textToUpdate) with the current time displayed after a given string (message)
    ///     - message gives context to the timer's purpose
    /// - calls a given method (callback) upon reaching zero
    /// <summary>
    private IEnumerator Timer(TMPro.TextMeshProUGUI textToUpdate, string message, int length, Action callback){
        length--;

        while (true)
        {
            //updates every tenth second 
            for (int i = 9; i >= 0; i--)
            {
                yield return new WaitForSeconds(.1f);
                textToUpdate.text = message + length + ":" + i;
            }
            if (length == 0)
            {
                callback?.Invoke();
                break;
            }
            length--;
        }
    }

    #endregion
}
