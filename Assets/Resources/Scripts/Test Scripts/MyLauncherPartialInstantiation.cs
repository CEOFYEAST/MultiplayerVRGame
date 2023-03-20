using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

namespace Com.MyCompany.MyGame
{
    public class MyLauncherPartialInstantiation : MonoBehaviourPunCallbacks
    {
        #region Public Fields

        public GameObject leftHandPrefab;
        public GameObject rightHandPrefab;

        public GameObject directLeftHand;
        public GameObject directRightHand;

        public GameObject originalLeftHand;
        public GameObject originalRightHand;

        public GameObject localRig;

        #endregion

        //private serializable fields are private fields which are made visible in the inspector via serialization
        // - the default for private fields is to be hidden in the inspector
        #region Private Serializable Fields

        /// <summary>
        /// The maximum number of players per room. When a room is full, it can't be joined by new players, and so new room will be created.
        /// </summary>
        [Tooltip("The maximum number of players per room. When a room is full, it can't be joined by new players, and so new room will be created")]
        [SerializeField]
        private byte maxPlayersPerRoom = 4;

        #endregion

        #region Private Fields

        /// <summary>
        /// This client's version number. Users are separated from each other by gameVersion (which allows you to make breaking changes).
        /// </summary>
        string gameVersion = "1";

        /// <summary>
        /// Keep track of the current process. Since connection is asynchronous and is based on several callbacks from Photon,
        /// we need to keep track of this to properly adjust the behavior when we receive call back by Photon.
        /// Typically this is used for the OnConnectedToMaster() callback.
        /// </summary>
        bool isConnecting;

        #endregion

        #region MonoBehaviour CallBacks

        /// <summary>
        /// MonoBehaviour method called on GameObject by Unity during early initialization phase.
        /// </summary>
        void Awake()
        {
            // #Critical
            // this makes sure we can use PhotonNetwork.LoadLevel() on the master client and all clients in the same room sync their level automatically
            PhotonNetwork.AutomaticallySyncScene = true;
        }

        /// <summary>
        /// MonoBehaviour method called on GameObject by Unity during initialization phase.
        /// </summary>
        void Start()
        {
            Connect();
        }

        #endregion


        #region Public Methods

        /// <summary>
        /// Start the connection process.
        /// - If already connected, we attempt joining a random room
        /// - if not yet connected, Connect this application instance to Photon Cloud Network
        /// </summary>
        public void Connect()
        {
            Debug.Log("checking connection...");

            // we check if we are connected or not, we join if we are, else we initiate the connection to the server.
            if (PhotonNetwork.IsConnected)
            {
                Debug.Log("already connected");

                // #Critical we need at this point to attempt joining a Random Room. If it fails, we'll get notified in OnJoinRandomFailed() and we'll create one.
                PhotonNetwork.JoinRandomRoom();
            }
            else
            {
                Debug.Log("connecting to Photon Online Server");

                // #Critical, we must first and foremost connect to Photon Online Server.
                isConnecting = PhotonNetwork.ConnectUsingSettings();
                PhotonNetwork.GameVersion = gameVersion;
            }
        }

        #endregion

        #region MonoBehaviourPunCallbacks Callbacks

        /// <summary>
        /// Runs when connection to master server is established 
        /// The master server handles connection to rooms, a responsibility that is reflected in the JoinRandomRoom call below
        /// <summary>
        public override void OnConnectedToMaster()
        {
            Debug.Log("PUN Basics Tutorial/Launcher: OnConnectedToMaster() was called by PUN");

            // we don't want to do anything if we are not attempting to join a room.
            // this case where isConnecting is false is typically when you lost or quit the game, when this level is loaded, OnConnectedToMaster will be called, in that case
            // we don't want to do anything.
            if (isConnecting)
            {
                // #Critical: The first we try to do is to join a potential existing room. If there is, good, else, we'll be called back with OnJoinRandomFailed()
                PhotonNetwork.JoinRandomRoom();
                isConnecting = false;
            }
        }

        public override void OnDisconnected(DisconnectCause cause)
        {
            Debug.LogWarningFormat("PUN Basics Tutorial/Launcher: OnDisconnected() was called by PUN with reason {0}", cause);
            isConnecting = false;
        }

        //called if JoinRandomRoom fails
        public override void OnJoinRandomFailed(short returnCode, string message)
        {
            Debug.Log("PUN Basics Tutorial/Launcher:OnJoinRandomFailed() was called by PUN. No random room available, so we create one.\nCalling: PhotonNetwork.CreateRoom");

            // #Critical: we failed to join a random room, maybe none exists or they are all full. No worries, we create a new room.
                //the max players count is set using the private serialized maxPlayersPerRoom variable initialized in the inspector
            PhotonNetwork.CreateRoom(null, new RoomOptions { MaxPlayers = maxPlayersPerRoom });
        }

        //called if JoinRandomRoom succeeds
        public override void OnJoinedRoom()
        {
            Debug.Log("PUN Basics Tutorial/Launcher: OnJoinedRoom() called by PUN. Now this client is in a room.");

            //instantiates player's hand models over the network
            GameObject leftHand = PhotonNetwork.Instantiate(this.leftHandPrefab.name, new Vector3(0f,0f,0f), Quaternion.identity, 0);
            GameObject rightHand = PhotonNetwork.Instantiate(this.rightHandPrefab.name, new Vector3(0f,5f,0f), Quaternion.identity, 0);

            leftHand.SetActive(false);
            rightHand.SetActive(false);

            leftHand.transform.position = originalLeftHand.transform.position;
            leftHand.transform.rotation = originalLeftHand.transform.rotation;

            rightHand.transform.position = originalRightHand.transform.position;
            rightHand.transform.rotation = originalRightHand.transform.rotation;

            originalLeftHand.SetActive(false);
            originalRightHand.SetActive(false);

            leftHand.SetActive(true);
            rightHand.SetActive(true);

            //places player's hand models in the correct position under direct hands in the hierarchy
            leftHand.transform.parent = directLeftHand.transform;
            rightHand.transform.parent = directRightHand.transform;

            //assigns hand animators of direct hands
            directLeftHand.GetComponent<AnimateHandOnInput>().handAnimator = leftHand.GetComponent<Animator>();
            directRightHand.GetComponent<AnimateHandOnInput>().handAnimator = rightHand.GetComponent<Animator>();

            //enables the scene XR rig 
            //localRig.SetActive(true);
        }

        #endregion
    }
}