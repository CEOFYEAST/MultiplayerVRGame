using System;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.SceneManagement;

using UnityEngine.UI;
using TMPro;

using Photon.Pun;
using Photon.Realtime;

namespace Com.MyCompany.MyGame
{
    public class MyWaitingRoomManager : MonoBehaviourPunCallbacks
    {
        #region Public Fields 

        // array containing text fields for player names 
        public GameObject[] masterImageBackgrounds = new GameObject[6];
        public GameObject[] nonMasterImageBackgrounds = new GameObject[6];

        #endregion

        #region Private Constants

        // hashtable keys stored to avoid typos/mis-references
        const string teamNumberHashmapKey = "TeamNumber";
        const string warmupLengthKey = "WarmupLength";
        const string gameLengthKey = "GameLength";
        const string pointsPerScoreKey = "PointsPerScore";
        const string gameModeKey = "GameMode";
        
        #endregion

        #region MonoBehaviour callbacks

        void Start()
        {
            // sets default warmup length (15 seconds)
            UpdateCustomRoomSettings(115);

            // sets default game length (60 seconds)
            UpdateCustomRoomSettings(260);

            // sets default points per ball (3 points)
            UpdateCustomRoomSettings(33);

            // sets default game mode (ffa)
            UpdateCustomRoomSettings(40);

            // assigns a player to a team
            AssignPlayerTeam();

            // updates the waiting room interface with the new information after a small delay
            StartCoroutine(Timer());
        }

        void Update(){
            StartCoroutine(Timer());
        }

        #endregion

        #region Photon Callbacks

        /// <summary>
        /// Called when the local player left the room. We need to load the launcher scene.
        /// </summary>
        public override void OnLeftRoom()
        {
            SceneManager.LoadScene(0);
        }

        public override void OnPlayerEnteredRoom(Player other)
        {
            Debug.LogFormat("OnPlayerEnteredRoom() {0}", other.NickName); // not seen if you're the player connecting

            // only sets player's team to their index in playerList if the gamemode is free for all
            if((int) PhotonNetwork.CurrentRoom.CustomProperties[gameModeKey] == 0){
                AssignPlayerTeam();
            }

            StartCoroutine(Timer());

            if (PhotonNetwork.IsMasterClient)
            {
                Debug.LogFormat("OnPlayerEnteredRoom IsMasterClient {0}", PhotonNetwork.IsMasterClient); // called before OnPlayerLeftRoom
            }
        }

        public override void OnPlayerLeftRoom(Player other)
        {
            Debug.LogFormat("OnPlayerLeftRoom() {0}", other.NickName); // seen when other disconnects

            // only sets player's team to their index in playerList if the gamemode is free for all
            if((int) PhotonNetwork.CurrentRoom.CustomProperties[gameModeKey] == 0){
                AssignPlayerTeam();
            }

            StartCoroutine(Timer());

            if (PhotonNetwork.IsMasterClient)
            {
                Debug.LogFormat("OnPlayerLeftRoom IsMasterClient {0}", PhotonNetwork.IsMasterClient); // called before OnPlayerLeftRoom
            }
        }

        #endregion

        #region Public Methods

        public void LeaveRoom()
        {
            PhotonNetwork.LeaveRoom();
        }

        public void StartGame(){
            //makes sure a level is only being loaded by the master client and there are atleast two players in the waiting room
            // if (!PhotonNetwork.IsMasterClient || PhotonNetwork.PlayerList.Length < 2)
            if (!PhotonNetwork.IsMasterClient)
            {
                return;
            }
            Debug.LogFormat("PhotonNetwork : Loading Level : Game Room");

            // closes the room to prevent new joiners
            PhotonNetwork.CurrentRoom.IsOpen = false;

            //starts the game
            PhotonNetwork.LoadLevel("Game Room");
        }

        /// <summary>
        /// adds a custom room setting using updateWith
        /// - the first digit of updateWith represents the key to use
        /// - everything after the first digit represents the value to fill the key with
        /// <summary>
        public void UpdateCustomRoomSettings(int updateWith){
            string fieldToUpdate = warmupLengthKey;

            // determines the key to update using the first digit of updateWith
            switch(GetFirstDigit(updateWith)){
                case 1:
                    fieldToUpdate = warmupLengthKey;
                    break;
                case 2:
                    fieldToUpdate = gameLengthKey;
                    break;
                case 3:
                    fieldToUpdate = pointsPerScoreKey;
                    break;
                case 4:
                    fieldToUpdate = gameModeKey;

                    // sets every player's team to their index in player list if the gm is switched from teams to ffa
                    if(updateWith == 0){
                        AssignPlayerTeam();
                        StartCoroutine(Timer());
                    }

                    break;
            }

            // creates a new hashmap to store a custom room setting
            ExitGames.Client.Photon.Hashtable _myCustomProperties = new ExitGames.Client.Photon.Hashtable();

            // sets the hashmap to the value of updateWith
            _myCustomProperties[fieldToUpdate] = RemoveFirstDigit(updateWith);

            // updates the room's custom properties over the network
            PhotonNetwork.CurrentRoom.SetCustomProperties(_myCustomProperties);

            Debug.Log("Intialized " + fieldToUpdate + " with " + RemoveFirstDigit(updateWith));

            Debug.Log("Accessing new field = " + PhotonNetwork.CurrentRoom.CustomProperties[fieldToUpdate]);
        }

        /// <summary>
        /// sets the local player's team to teamToAssign (AssignPlayerTeam overload)
        /// <summary>
        public void AssignPlayerTeam(int teamToAssign){
            // creates a new hashmap to store the player's team number
            ExitGames.Client.Photon.Hashtable _myCustomProperties = new ExitGames.Client.Photon.Hashtable();

            // sets the hashmap (team number) to teamToAssign
            _myCustomProperties[teamNumberHashmapKey] = teamToAssign;

            // updates the player's custom properties locally 
            PhotonNetwork.LocalPlayer.CustomProperties = _myCustomProperties;

            // updates the player's custom properties over the network
            PhotonNetwork.LocalPlayer.SetCustomProperties(_myCustomProperties);

            StartCoroutine(Timer());
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// sets the local player's team to their index in playerList
        /// <summary>
        private void AssignPlayerTeam(){
            // creates a new hashmap to store the player's team number
            ExitGames.Client.Photon.Hashtable _myCustomProperties = new ExitGames.Client.Photon.Hashtable();

            // grabs the player's index in PlayerList
            int playerIndex = 0;
            foreach(Player player in PhotonNetwork.PlayerList){
                if(player.IsLocal){
                    break;
                }
                playerIndex++;
            }

            // sets the hashmap (team number) to the player's index in PlayerList
            _myCustomProperties[teamNumberHashmapKey] = playerIndex;

            // updates the player's custom properties locally 
            PhotonNetwork.LocalPlayer.CustomProperties = _myCustomProperties;

            // updates the player's custom properties over the network
            PhotonNetwork.LocalPlayer.SetCustomProperties(_myCustomProperties);
        }

        /// <summary>
        /// updates player fields to reflect PlayerList
        /// <summary>
        private void UpdatePlayerFields(GameObject[] chosenImageBackgrounds){
            // clears all text and disables all image backgrounds
            foreach(GameObject imageBackground in chosenImageBackgrounds){
                // gets player text item under image background
                TMPro.TextMeshProUGUI playerText = imageBackground.transform.GetChild(0).gameObject.GetComponent<TMPro.TextMeshProUGUI>();

                // empties text of playerText
                playerText.text = "";

                // disables imageBackground
                imageBackground.SetActive(false);
            }
            
            // goes through player fields, changing them to names of players in PlayerList 
            // and changing the background colors to reflect the teams of players
            int i = 0;
            foreach(Player player in PhotonNetwork.PlayerList){
                // enables image background
                chosenImageBackgrounds[i].SetActive(true);

                // grabs team number of player from custom properties
                int playerTeamNumber = (int) PhotonNetwork.PlayerList[i].CustomProperties[teamNumberHashmapKey];

                // grabs team color of player using team number
                Color32 playerTeamColor = StaticTeamColors.teamColors[playerTeamNumber];

                // changes color of image background to reflect the player's team
                chosenImageBackgrounds[i].GetComponent<Image>().color = playerTeamColor;

                // gets player text item under image background
                TMPro.TextMeshProUGUI playerText = chosenImageBackgrounds[i].transform.GetChild(0).gameObject.GetComponent<TMPro.TextMeshProUGUI>();

                // sets text of playerText
                playerText.text = PhotonNetwork.PlayerList[i].NickName;

                i++;
            }
        }

        // Find the first digit
        private int GetFirstDigit(int n)
        {
            string stringN = n.ToString();

            stringN = stringN.Substring(0, 1);

            return Int32.Parse(stringN);
        }

        // Remove the first digit
        private int RemoveFirstDigit(int n)
        {
            string stringN = n.ToString();

            stringN = stringN.Substring(1);

            return Int32.Parse(stringN);
        }

        #endregion

        #region Private IEnumerators

        /// <summary>
        /// IEnumerator that makes sure every player has a team selected 
        /// before player fields are updated
        /// <summary>
        private IEnumerator Timer(){
            yield return new WaitForSeconds(.5f);

            UpdatePlayerFields(masterImageBackgrounds);
            UpdatePlayerFields(nonMasterImageBackgrounds);
        }

        #endregion
    }
}