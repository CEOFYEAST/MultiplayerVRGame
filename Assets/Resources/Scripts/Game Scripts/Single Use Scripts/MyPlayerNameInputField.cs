using UnityEngine;
using UnityEngine.UI;
using TMPro;

using Photon.Pun;
using Photon.Realtime;

using System.Collections;

namespace Com.MyCompany.MyGame
{
    /// <summary>
    /// Player name input field. Let the user input his name, will appear above the player in the game.
    /// </summary>
    public class MyPlayerNameInputField : MonoBehaviour
    {
        #region Public Variables

        // text object that is updated by the player to change their username  
        public TMPro.TextMeshProUGUI inputText;

        #endregion


        #region MonoBehaviour CallBacks

        /// <summary>
        /// Sets the input field text and the player nickname to an existing nickname from a previous session if it exists
        /// </summary>
        void Start () {

            string defaultName = "player";
            if (inputText!=null)
            {
                if (PlayerPrefs.HasKey(StaticConstants.playerNamePrefKey))
                {
                    defaultName = PlayerPrefs.GetString(StaticConstants.playerNamePrefKey);
                    inputText.text = defaultName;
                } 
            }

            Debug.Log("Initialized With: " + defaultName);

            PhotonNetwork.NickName =  defaultName;

            PlayerPrefs.SetString(StaticConstants.playerNamePrefKey,defaultName);
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Sets the name of the player, and save it in the PlayerPrefs for future sessions.
        /// </summary>
        /// <param name="value">The name of the Player</param>
        public void SetPlayerName()
        {
            string value = inputText.text;

            // #Important
            if (string.IsNullOrEmpty(value))
            {
                Debug.LogError("Player Name is null or empty");
                return;
            }
            PhotonNetwork.NickName = value;

            Debug.Log("Set To: " + value);

            PlayerPrefs.SetString(StaticConstants.playerNamePrefKey,value);
        }

        #endregion
    }
}