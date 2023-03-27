using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

using Photon.Pun;
using Photon.Realtime;

/// <summary>
/// class desgined to destory a networked game object as soon as it's owner changes
/// - used to destroy certain objects whose owner doesn't change when that owner leaves the room
/// <summary>
public class DestroyOnOwnerLeave : MonoBehaviour
{
    private Player owner;

    // Start is called before the first frame update
    void Start()
    {
        owner = GetObjectOwner();
    }

    // Update is called once per frame
    void Update()
    {
        if(GetObjectOwner() != owner){
            PhotonNetwork.Destroy(gameObject);
        }
    }

    private Player GetObjectOwner(){
        try{
            return gameObject.GetComponent<PhotonView>().Owner;
        }
        catch (Exception e) {
            return null;
        }
    }
}
