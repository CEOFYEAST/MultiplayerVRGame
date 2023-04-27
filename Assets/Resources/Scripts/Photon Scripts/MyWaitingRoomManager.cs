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

        // store the team number custom properties key to avoid typos
        const string teamNumberHashmapKey = "TeamNumber";
        
        #endregion

        #region MonoBehaviour callbacks

        void Start()
        {
            AssignPlayerTeam();

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

            AssignPlayerTeam();

            StartCoroutine(Timer());

            if (PhotonNetwork.IsMasterClient)
            {
                Debug.LogFormat("OnPlayerEnteredRoom IsMasterClient {0}", PhotonNetwork.IsMasterClient); // called before OnPlayerLeftRoom
            }
        }

        public override void OnPlayerLeftRoom(Player other)
        {
            Debug.LogFormat("OnPlayerLeftRoom() {0}", other.NickName); // seen when other disconnects

            AssignPlayerTeam();

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
            if (!PhotonNetwork.IsMasterClient || PhotonNetwork.PlayerList.Length < 2)
            {
                return;
            }
            Debug.LogFormat("PhotonNetwork : Loading Level : Game Room");

            // closes the room to prevent new joiners
            PhotonNetwork.CurrentRoom.IsOpen = false;

            //starts the game
            PhotonNetwork.LoadLevel("Game Room");
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// assigns the local player's team and updates their custom properties team hashmap to reflect the change
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
        void UpdatePlayerFields(GameObject[] chosenImageBackgrounds){
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

        #endregion

        #region Private IEnumerators

        /// <summary>
        /// IEnumerator that makes sure every player has a team selected 
        /// before player fields are updated
        /// <summary>
        private IEnumerator Timer(){
            //updates every tenth second 
            for (int i = 9; i >= 0; i--)
            {
                yield return new WaitForSeconds(.1f);
            }

            UpdatePlayerFields(masterImageBackgrounds);
            UpdatePlayerFields(nonMasterImageBackgrounds);
        }

        #endregion
    }
}