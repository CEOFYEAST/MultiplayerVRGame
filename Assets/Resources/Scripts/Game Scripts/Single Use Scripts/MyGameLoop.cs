using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.UI;
using TMPro;

using Photon.Pun;
using Photon.Realtime;

public class MyGameLoop : MonoBehaviour
{
    #region Private Fields

    // dictionary containing the scores of every team and their respective team number as the key
    private static IDictionary<int, int> teamScores = new Dictionary<int, int>();


    int gameTimerLength = 10;

    #endregion

    #region Private Serializable Fields

    // text objects used to display game and start timers
    [SerializeField]
    private TMPro.TextMeshProUGUI popupText;

    [SerializeField]
    private TMPro.TextMeshProUGUI[] timerTexts;

    // trigger that tracks goals
    [SerializeField]
    private GameObject scoreTrigger;

    // scoreboard content displays
    [SerializeField]
    private GameObject[] contentDisplays = new GameObject[3];

    #endregion


    #region Private Constants

        // hashtable keys stored to avoid typos/mis-references
        const string teamNumberHashmapKey = "TeamNumber";
        const string warmupLengthKey = "WarmupLength";
        const string gameLengthKey = "GameLength";
        const string pointsPerScoreKey = "PointsPerScore";
        
    #endregion


    #region Unity Monobehaviour Callbacks

    // Start is called before the first frame update
    void Start()
    {
        StartGameLoop();
    }

    #endregion


    #region Public Methods

    /// <summary>
    /// updates scoreboards on a player scoring a basket
    /// <summary>
    public void OnScore(Player scorer){
        // gets the team number of the player that made the shot
        int scorerTeamNumber = (int) scorer.CustomProperties[teamNumberHashmapKey];

        // updates the proper team score by a fixed amount
        if(teamScores.ContainsKey(scorerTeamNumber)){
            teamScores[scorerTeamNumber] += (int) PhotonNetwork.CurrentRoom.CustomProperties[pointsPerScoreKey];
        }

        // updates the scoreboards based on the new score
        UpdateScoreboard(scorerTeamNumber);
    }

    #endregion


    #region Private Methods

    private void StartGameLoop(){
        // hides unused team scoreboards
        foreach(GameObject scoreboardContent in contentDisplays){
            scoreboardContent.GetComponent<HideUnusedScoreboards>().Hide();
        }

        // fills team scores dictionary with key (team number) - value (team score) pairs
        InitializeTeamScoresDictionary();

        // starts the timer
        StartCoroutine(Timer(popupText, "Warmup ending in ", (int) PhotonNetwork.CurrentRoom.CustomProperties[warmupLengthKey], BlockOne));
    }

    /// <summary>
    /// fills team scores dictionary with teams
    /// <summary>
    private void InitializeTeamScoresDictionary(){
        foreach(Player player in PhotonNetwork.PlayerList){
            // grabs player's team number
            int playerTeamNumber = (int) player.CustomProperties[teamNumberHashmapKey];

            // creates pair for player's team if one doesn't exist
            if(!(teamScores.ContainsKey(playerTeamNumber))){
                teamScores.Add(playerTeamNumber, 0);
            }
        }
    }

    /// <summary> 
    /// updates proper scoreboard with its current score
    /// <summary>
    private void UpdateScoreboard(int scorerTeamNumber){
        foreach(GameObject scoreboardContent in contentDisplays){
            // grabs the proper scoreboard for the scoring team
            GameObject scoringScoreboard = scoreboardContent.transform.GetChild(scorerTeamNumber).gameObject;

            // grabs the text child to the scoreboard
            TMPro.TextMeshProUGUI scoreboardText = scoringScoreboard.transform.GetChild(1).gameObject.GetComponent<TMPro.TextMeshProUGUI>();

            // updates the scoreboard's text
            scoreboardText.text = teamScores[scorerTeamNumber].ToString();
        }
    }

        /// <summary>
        /// methods that constitute the game loop, which 
        ///  - gameloop is broken up into blocks that can be called individually, allows for timers
        /// <summary>
        #region Game Loop Blocks

        /// <summary>
        /// occurs after starting timer ends
        /// is the main body of the game
        /// <summary>
        private void BlockOne(){
            // disables popup text for the time being
            popupText.gameObject.SetActive(false);

            // allows players to score 
            scoreTrigger.SetActive(true);

            // starts game timers
            for(int i = 0; i < timerTexts.Length; i++){
                if(i != timerTexts.Length - 1){
                    StartCoroutine(Timer(timerTexts[i], "", (int) PhotonNetwork.CurrentRoom.CustomProperties[gameLengthKey], null));
                }
                else {
                    StartCoroutine(Timer(timerTexts[i], "", (int) PhotonNetwork.CurrentRoom.CustomProperties[gameLengthKey], BlockTwo));
                }
            }
        }

        /// <summary>
        /// contains the "end" of the game, occurs when game timer ends
        /// <summary>
        private void BlockTwo(){
            Debug.Log("Called Block Two");

            int highestTeamScore = teamScores[0];

            // iterates through team scores and fills highestTeamScore
            for(int i = 1; i < teamScores.Count; i++){
                if(teamScores[i] > highestTeamScore){
                    highestTeamScore = teamScores[i];
                }
            }
            
            scoreTrigger.SetActive(false);

            // grabs team number of local player
            int playerTeamNumber = (int) PhotonNetwork.LocalPlayer.CustomProperties[teamNumberHashmapKey];

            popupText.gameObject.SetActive(true);

            popupText.text = "You lose!";

            // iterates through team scores and displays proper message -
            /// depending on if player's team had the highest score
            for(int i = 0; i < teamScores.Count; i++){
                if(teamScores[i] == highestTeamScore){
                    if(playerTeamNumber == i){
                        popupText.text = "You Win!";
                        return;
                    }   
                }
            }
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
                if(callback != null){
                    callback?.Invoke();
                }

                break;
            }
            length--;
        }
    }

    #endregion
}
