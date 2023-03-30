using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Photon.Pun;
using Photon.Realtime;

//class meant to return once the GameObject the script is attached to is fully loaded along with all it's children
public class MoveOnLoaded : MonoBehaviourPun
{   
    [SerializeField]
    private GameObject basketballRackPrefab;

    // positions to place players around the world
    Vector3[] positions = new [] 
    {
        new Vector3(-3.4f, 0.75f, 3.0f),
        new Vector3(-3.4f, 0.75f, -3.0f)
    };

    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("Checking Loaded Status");
        StartCoroutine(WaitUntilFullyLoaded(gameObject));
    }

    /// <summary>
    /// IEnumerator that returns once a GameObject "go", its children and all their components are fully loaded in
    /// extra code at the bottom that calls methods to manage input on the newly loaded rig
    /// written by chatGPT
    /// <summary>
    IEnumerator WaitUntilFullyLoaded(GameObject go)
    {
        yield return new WaitForEndOfFrame(); // Wait for the current frame to finish rendering

        if (go.transform.parent != null) // Wait for parent to be fully loaded
        {
            yield return StartCoroutine(WaitUntilFullyLoaded(go.transform.parent.gameObject));
        }

        bool allChildrenLoaded = false;
        while (!allChildrenLoaded) // Wait for all children to be fully loaded
        {
            allChildrenLoaded = true;
            foreach (Transform child in go.transform)
            {
                if (!child.gameObject.activeSelf || !child.gameObject.scene.isLoaded)
                {
                    allChildrenLoaded = false;
                    yield return new WaitForEndOfFrame();
                    break;
                }
            }
        }

        // All parents and children are fully loaded in at this point
        Debug.Log("GameObject " + go.name + " and all of its parents and children are fully loaded in.");

        MovePlayer();

        SpawnRack();
    }

    /// <summary>
    /// moves the player to a position on the three point line based on their index in playerlist
    /// - makes sure every player ends up at a different spot
    /// <summary>
    private void MovePlayer(){
        //sets i to the local player's index in player list
        int i = 0;
        foreach(Player player in PhotonNetwork.PlayerList){
            if(player == PhotonNetwork.LocalPlayer){
                break;
            }
            i++;
        }

        //gets the transform of the local xr rig
        Transform playerTransform = GameObject.FindObjectOfType<Unity.XR.CoreUtils.XROrigin>().GetComponentInParent<Transform>();

        //gets the local xr rig
        GameObject localRig = playerTransform.parent.gameObject;

        Debug.Log("New Position: " + positions[i]);

        //sets the local rig's position to the Vector3 at i in positions
        localRig.GetComponent<Transform>().position = positions[i];
    }

    /// <summary>
    /// spawns a ball rack over the network next to the player
    /// <summary>
    private void SpawnRack(){
        //gets the transform of the local xr rig
        Vector3 playerPosition = GameObject.FindObjectOfType<Unity.XR.CoreUtils.XROrigin>().GetComponentInParent<Transform>().position;

        //sets the rack's soon to be position to the right of the local player's position
        playerPosition.z -= 1f;

        //instantiates a basketball rig to the right of the local player over the network
        GameObject basketballRack = PhotonNetwork.Instantiate(this.basketballRackPrefab.name, 
            playerPosition, 
            Quaternion.identity, 
            0);
    }

}
