using System;
using System.Collections;

using UnityEngine;
using UnityEngine.SceneManagement;

using Photon.Pun;
using Photon.Realtime;

namespace Com.MyCompany.MyGame
{
    public class MyGameManager : MonoBehaviourPunCallbacks
    {
        #region Public Fields 

        // objects used for player network instantiation
        public GameObject leftHandPrefab;
        public GameObject rightHandPrefab;
        public GameObject headbandPrefab;

        public GameObject directLeftHand;
        public GameObject directRightHand;

        public GameObject originalLeftHand;
        public GameObject originalRightHand;
        public GameObject originalHeadband;

        public GameObject mainCamera;

        public GameObject localRig;

        // allows me to call any method from a static context
        // - I can call leave room like "GameManager.Instance.LeaveRoom();" and it will disconnect the local player
        // initialized as this script in Start
        public static MyGameManager Instance;

        //[Tooltip("The prefab to use for representing the player")]
        //public GameObject playerPrefab;

        // I need to call ManagedSyncedInputs from this script 
        //public GameObject SyncedInputManager;

        #endregion

        #region MonoBehaviour callbacks

        void Start()
        {
            //instantiates instance of MyGameManager declared in public fields 
            Instance = this;
            
            //instantiates the networked objects
            if (localRig == null)
            {
                Debug.LogError("<Color=Red><a>Missing</a></Color> playerPrefab Reference. Please set it up in GameObject 'Game Manager'",this);
            }
            else
            {
                Debug.LogFormat("We are Instantiating LocalPlayer from {0}", Application.loadedLevelName);

                //instantiates player's hand models over the network
                GameObject leftHand = PhotonNetwork.Instantiate(this.leftHandPrefab.name, new Vector3(0f,0f,0f), Quaternion.identity, 0);
                GameObject rightHand = PhotonNetwork.Instantiate(this.rightHandPrefab.name, new Vector3(0f,5f,0f), Quaternion.identity, 0);
                //instantiates headband over the network
                GameObject headband = PhotonNetwork.Instantiate(this.headbandPrefab.name, new Vector3(0f,5f,0f), Quaternion.identity, 0);

                leftHand.SetActive(false);
                rightHand.SetActive(false);
                headband.SetActive(false);

                leftHand.transform.position = originalLeftHand.transform.position;
                leftHand.transform.rotation = originalLeftHand.transform.rotation;

                rightHand.transform.position = originalRightHand.transform.position;
                rightHand.transform.rotation = originalRightHand.transform.rotation;

                headband.transform.position = originalHeadband.transform.position;
                headband.transform.rotation = originalHeadband.transform.rotation;

                Destroy(originalLeftHand);
                Destroy(originalRightHand);
                Destroy(originalHeadband);
                
                headband.SetActive(true);

                //places player's hand models in the correct position under direct hands in the hierarchy
                leftHand.transform.parent = directLeftHand.transform;
                rightHand.transform.parent = directRightHand.transform;
                //places headband in the correct position under main camera in the hierarchy
                headband.transform.parent = mainCamera.transform;

                //assigns hand animators of direct hands
                directLeftHand.GetComponent<AnimateHandOnInput>().handAnimator = leftHand.GetComponent<Animator>();
                directRightHand.GetComponent<AnimateHandOnInput>().handAnimator = rightHand.GetComponent<Animator>();

                // we're in a room. spawn a character for the local player. it gets synced by using PhotonNetwork.Instantiate
                //PhotonNetwork.Instantiate(this.playerPrefab.name, new Vector3(0f,5f,0f), Quaternion.identity, 0);

                // manages inputs of all players every time a new player is instantiated
                // - prevents players from controlling synced objects belonging to other players
                //SyncedInputManager.ManageSyncedInputs();
            }
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

            if (PhotonNetwork.IsMasterClient)
            {
                Debug.LogFormat("OnPlayerEnteredRoom IsMasterClient {0}", PhotonNetwork.IsMasterClient); // called before OnPlayerLeftRoom

                LoadArena();
            }
        }

        public override void OnPlayerLeftRoom(Player other)
        {
            Debug.LogFormat("OnPlayerLeftRoom() {0}", other.NickName); // seen when other disconnects

            if (PhotonNetwork.IsMasterClient)
            {
                Debug.LogFormat("OnPlayerLeftRoom IsMasterClient {0}", PhotonNetwork.IsMasterClient); // called before OnPlayerLeftRoom

                LoadArena();
            }
        }

        #endregion

        #region Public Methods

        public void LeaveRoom()
        {
            PhotonNetwork.LeaveRoom();
        }

        #endregion

        #region Private Methods

        //loads a level based on the room's current player count
        void LoadArena()
        {
            //makes sure a level is only being loaded by the master client
            if (!PhotonNetwork.IsMasterClient)
            {
                Debug.LogError("PhotonNetwork : Trying to Load a level but we are not the master Client");
                return;
            }
            Debug.LogFormat("PhotonNetwork : Loading Level : {0}", PhotonNetwork.CurrentRoom.PlayerCount);

            //loads the level by name, accounting for player count 
            PhotonNetwork.LoadLevel("Room for " + PhotonNetwork.CurrentRoom.PlayerCount);
        }

        #endregion
    }
}