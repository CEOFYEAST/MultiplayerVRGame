using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Photon.Pun;
using Photon.Realtime;

// currently meant to test if you can check the photonView of any GameObject and see if it's yours
public class ViewTest : MonoBehaviourPun
{
    public GameObject playerPrefab;

    // Start is called before the first frame update
    public void Test()
    {
        GameObject toTest = PhotonNetwork.Instantiate(this.playerPrefab.name, new Vector3(0f,5f,0f), Quaternion.identity, 0);
        Debug.Log("ViewTest.Test() was called, player was instantiated");
    }
}
