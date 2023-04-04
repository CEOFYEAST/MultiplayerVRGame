using System;
using System.Collections;

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
        public GameObject[] imageBackgrounds = new GameObject[6];

        #endregion

        #region MonoBehaviour callbacks

        void Start()
        {
            UpdatePlayerFields();
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

            UpdatePlayerFields();

            if (PhotonNetwork.IsMasterClient)
            {
                Debug.LogFormat("OnPlayerEnteredRoom IsMasterClient {0}", PhotonNetwork.IsMasterClient); // called before OnPlayerLeftRoom
            }
        }

        public override void OnPlayerLeftRoom(Player other)
        {
            Debug.LogFormat("OnPlayerLeftRoom() {0}", other.NickName); // seen when other disconnects

            UpdatePlayerFields();

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
            if (!PhotonNetwork.IsMasterClient )//|| PhotonNetwork.PlayerList.Length < 2)
            {
                return;
            }
            Debug.LogFormat("PhotonNetwork : Loading Level : Game Room");

            //starts the game
            PhotonNetwork.LoadLevel("Game Room");
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// updates player fields to reflect PlayerList
        /// <summary>
        void UpdatePlayerFields(){
            // clears all text and disables all image backgrounds
            foreach(GameObject imageBackground in imageBackgrounds){
                // gets player text item under image background
                TMPro.TextMeshProUGUI playerText = imageBackground.transform.GetChild(0).gameObject.GetComponent<TMPro.TextMeshProUGUI>();

                // empties text of playerText
                playerText.text = "";

                // disables imageBackground
                imageBackground.SetActive(false);
            }
            
            // goes through player fields, changing them to names of players in PlayerList
            int i = 0;
            foreach(Player player in PhotonNetwork.PlayerList){
                // enables image background
                imageBackgrounds[i].SetActive(true);

                // gets player text item under image background
                TMPro.TextMeshProUGUI playerText = imageBackgrounds[i].transform.GetChild(0).gameObject.GetComponent<TMPro.TextMeshProUGUI>();

                // sets text of playerText
                playerText.text = "Player [" + PhotonNetwork.PlayerList[i].ActorNumber + "]";

                i++;
            }
        }

        #endregion
    }
}