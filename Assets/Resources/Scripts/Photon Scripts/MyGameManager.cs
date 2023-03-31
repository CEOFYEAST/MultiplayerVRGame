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

        // allows me to call any method from a static context
        // - I can call leave room like "GameManager.Instance.LeaveRoom();" and it will disconnect the local player
        // initialized as this script in Start
        public static MyGameManager Instance;

        // prefabs of networked objects
        public GameObject headbandPrefab;
        public GameObject basketballRackPrefab;

        public GameObject localRig;

        //[Tooltip("The prefab to use for representing the player")]
        //public GameObject playerPrefab;

        // I need to call ManagedSyncedInputs from this script 
        //public GameObject SyncedInputManager;

        #endregion
        
        #region Private Fields

        // positions to place players around the world
        Vector3[] positions = new [] 
        {
            new Vector3(-3.4f, 0.75f, 3.0f),
            new Vector3(-3.4f, 0.75f, -3.0f)
        };

        String[] handColors = new []
        {
            "",
            "Green "
        };

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

                InstantiatePlayer();

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

        public void InstantiatePlayer(){
            // original objects for the new networked objects to replace in the hierarchy 
            GameObject originalLeftHand = GameObject.Find("Left Hand Model");
            GameObject originalRightHand = GameObject.Find("Right Hand Model");
            GameObject originalHeadband = GameObject.Find("Headband");

            // sets names of prefabs to be instantiated, accounting for color
            String leftHandPrefabName = "Left Hand Model " + handColors[GetPlayerIndex()] + "(networked)";
            String rightHandPrefabName = "Right Hand Model " + handColors[GetPlayerIndex()] + "(networked)";

            // instantiates player's hand models over the network
            // - makes sure to set position and rotation of new object to that of object they're replacing in the rig
            GameObject leftHand = PhotonNetwork.Instantiate(leftHandPrefabName, 
                originalLeftHand.GetComponent<Transform>().position, 
                originalLeftHand.GetComponent<Transform>().rotation, 
                0);
            GameObject rightHand = PhotonNetwork.Instantiate(rightHandPrefabName, 
                originalRightHand.GetComponent<Transform>().position, 
                originalRightHand.GetComponent<Transform>().rotation, 
                0);
            //instantiates player headband over the network
            GameObject headband = PhotonNetwork.Instantiate(this.headbandPrefab.name,
                originalHeadband.GetComponent<Transform>().position,
                originalHeadband.GetComponent<Transform>().rotation,
                0);

            //gets parents of hands
            GameObject leftHandParent = originalLeftHand.GetComponentInParent<Transform>().parent.gameObject;
            GameObject rightHandParent = originalRightHand.GetComponentInParent<Transform>().parent.gameObject;
            //gets parent of headband
            GameObject headbandParent = originalHeadband.GetComponentInParent<Transform>().parent.gameObject;

            leftHand.SetActive(false);
            rightHand.SetActive(false);
            headband.SetActive(false);

            //places player's hand models in the correct position under direct hands in the hierarchy
            leftHand.transform.parent = leftHandParent.transform;
            rightHand.transform.parent = rightHandParent.transform;
            //does the same to headband
            headband.transform.parent = headbandParent.transform;

            // destroys originals
            Destroy(originalLeftHand);
            Destroy(originalRightHand);
            Destroy(originalHeadband);

            headband.SetActive(true);

            //assigns hand animators of direct hands
            leftHandParent.GetComponent<AnimateHandOnInput>().handAnimator = leftHand.GetComponent<Animator>();
            rightHandParent.GetComponent<AnimateHandOnInput>().handAnimator = rightHand.GetComponent<Animator>();

            MovePlayer();

            SpawnRack();
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

        /// <summary>
        /// moves the player to a position on the three point line based on their index in playerlist
        /// - makes sure every player ends up at a different spot
        /// <summary>
        private void MovePlayer(){
            //sets the local rig's position to the Vector3 at i in positions
            localRig.GetComponent<Transform>().position = positions[GetPlayerIndex()];
        }

        /// <summary>
        /// spawns a ball rack over the network next to the player
        /// <summary>
        private void SpawnRack(){
            //gets the transform of the local xr rig
            //Vector3 playerPosition = GameObject.FindObjectOfType<Unity.XR.CoreUtils.XROrigin>().GetComponentInParent<Transform>().position;
            Vector3 playerPosition = localRig.GetComponent<Transform>().position;

            //sets the rack's soon to be position to the right of the local player's position
            playerPosition.z -= 1f;

            //instantiates a basketball rig to the right of the local player over the network
            GameObject basketballRack = PhotonNetwork.Instantiate(this.basketballRackPrefab.name, 
                playerPosition, 
                Quaternion.identity, 
                0);
        }

        /// <summary>
        /// returns local player's index in player list
        /// <summary>
        private int GetPlayerIndex(){
            //sets i to the local player's index in player list
            int i = 0;
            foreach(Player player in PhotonNetwork.PlayerList){
                if(player == PhotonNetwork.LocalPlayer){
                    break;
                }
                i++;
            }
            return i;
        }

        #endregion
    }
}