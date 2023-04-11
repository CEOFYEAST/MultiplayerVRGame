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
        public GameObject usernameDisplayPrefab;
        public GameObject basketballRackPrefab;

        public GameObject localRig;

        //[Tooltip("The prefab to use for representing the player")]
        //public GameObject playerPrefab;

        // I need to call ManagedSyncedInputs from this script 
        //public GameObject SyncedInputManager;

        #endregion
        
        #region Private Fields

        // positions to place players around the scene
        Vector3[,] playerPositions =
        {
            // even positions 
            {
                new Vector3(-4.23999977f,0f,2.93000007f),
                new Vector3(-4.23999977f,0f,-2.93000007f),
                new Vector3(-1.42999995f,0f,5.36999989f),
                new Vector3(-1.42999995f,0f,-5.36999989f),
                new Vector3(2.07999992f,0f,6.38000011f),
                new Vector3(2.07999992f,0f,-6.38000011f)
            },
            // odd positions
            {
                new Vector3(-4.71000004f,0f,0.0399999991f),
                new Vector3(-3.06999993f,0f,4.36000013f),
                new Vector3(-3.06999993f,0f,-4.36000013f),
                new Vector3(0.230000004f,0f,6.09000015f),
                new Vector3(0.230000004f,0f,-6.09000015f),
                new Vector3(0f,0f,0f)
            }
        };

        // positions to place racks around the scene
        Vector3[,] rackPositions =
        {
            // even positions
            {
                new Vector3(-4.32000017f,0.75f,1.85000002f),
                new Vector3(-3.29999995f,0.75f,-3.44000006f),
                new Vector3(-2.08999991f,0.75f,4.63000011f),
                new Vector3(-0.409999996f,0.75f,-5.51999998f),
                new Vector3(1.15999997f,0.75f,6f),
                new Vector3(3.04999995f,0.75f,-6.67000008f)
            },
            // odd positions 
            {
                new Vector3(-4.28000021f,0.75f,-1.07000005f),
                new Vector3(-3.43000007f,0.75f,3.46000004f),
                new Vector3(-2.07999992f,0.75f,-4.76999998f),
                new Vector3(-0.439999998f,0.75f,5.36999989f),
                new Vector3(1.16999996f,0.75f,-6.01999998f),
                new Vector3(0f,0f,0f)
            }
        };

        // rotations to apply to racks
        float[,] rackRotations =
        {
            // even rotations
            { 
                25.1358223f, 323.608612f, 59.4306564f, 300.569f, 90f, 90f
            },
            // odd rotations 
            { 
                0f, 46.8230438f, 313.177002f, 65.5279922f, 294.472015f, 0f
            }
        };

        String[] playerColors = new []
        {
            "Blue ",
            "Green ",
            "Orange ",
            "Purple ",
            "Red ",
            "Yellow "
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
            }
        }

        public override void OnPlayerLeftRoom(Player other)
        {
            Debug.LogFormat("OnPlayerLeftRoom() {0}", other.NickName); // seen when other disconnects

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

        public void InstantiatePlayer(){
            // original objects for the new networked objects to replace in the hierarchy 
            GameObject originalLeftHand = GameObject.Find("Left Hand Model");
            GameObject originalRightHand = GameObject.Find("Right Hand Model");
            GameObject originalHeadband = GameObject.Find("Headband");

            //sets player color based on their position in the playerlist
            String playerColor = playerColors[GetPlayerIndex()];

            // sets names of prefabs to be instantiated, accounting for color
            String leftHandPrefabName = "Left Hand Model " + playerColor + "(networked)";
            String rightHandPrefabName = "Right Hand Model " + playerColor + "(networked)";
            String headbandPrefabName = "Headband " + playerColor + "(networked)";

            // sets position of usernameDisplay
            Vector3 usernameDisplayPosition = originalHeadband.GetComponent<Transform>().position;
            usernameDisplayPosition.y += 1;

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
            // instantiates player headband over the network
            GameObject headband = PhotonNetwork.Instantiate(headbandPrefabName,
                originalHeadband.GetComponent<Transform>().position,
                originalHeadband.GetComponent<Transform>().rotation,
                0);
            // instantiates player username display over the network 
            GameObject usernameDisplay = PhotonNetwork.Instantiate(usernameDisplayPrefab.name,
                usernameDisplayPosition,
                Quaternion.identity,
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
            //does the same to headband but places it under Main Camera
            headband.transform.parent = headbandParent.transform;
            //usernameDisplay uses the same parent as headband
            usernameDisplay.transform.parent = headbandParent.transform;

            // destroys originals
            Destroy(originalLeftHand);
            Destroy(originalRightHand);
            Destroy(originalHeadband);

            headband.SetActive(true);

            //assigns hand animators of direct hands
            leftHandParent.GetComponent<AnimateHandOnInput>().handAnimator = leftHand.GetComponent<Animator>();
            rightHandParent.GetComponent<AnimateHandOnInput>().handAnimator = rightHand.GetComponent<Animator>();

            //moves player to correct position on the court
            MovePlayer();

            //spawns basketball rack next to the player 
            SpawnRack();
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// moves the player to a position on the three point line based on their index in playerlist
        /// - makes sure every player ends up at a different spot
        /// <summary>
        private void MovePlayer(){
            //sets the local rig's position to the Vector3 at i in positions, accounting for length of playerList
            if(PhotonNetwork.PlayerList.Length % 2 == 0){
                localRig.GetComponent<Transform>().position = playerPositions[0, GetPlayerIndex()];
            }
            else {
                localRig.GetComponent<Transform>().position = playerPositions[1, GetPlayerIndex()];
            }
        }

        /// <summary>
        /// spawns a ball rack over the network next to the player
        /// <summary>
        private void SpawnRack(){
            Vector3 rackPosition;
            Quaternion rackRotation;

            if(PhotonNetwork.PlayerList.Length % 2 == 0){
                rackPosition = rackPositions[0, GetPlayerIndex()];
                rackRotation = Quaternion.AngleAxis(rackRotations[0, GetPlayerIndex()], Vector3.up);
            } else {
                rackPosition = rackPositions[1, GetPlayerIndex()];
                rackRotation = Quaternion.AngleAxis(rackRotations[1, GetPlayerIndex()], Vector3.up);
            }

            //instantiates a basketball rack to the right of the local player over the network
            GameObject basketballRack = PhotonNetwork.Instantiate(this.basketballRackPrefab.name, 
                rackPosition, 
                rackRotation, 
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