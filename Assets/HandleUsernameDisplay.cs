using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.UI;
using TMPro;

using Photon.Pun;
using Photon.Realtime;

public class HandleUsernameDisplay : MonoBehaviourPun
{
    public TMPro.TextMeshProUGUI usernameDisplayText;

    private Transform MainCameraTransform;
    private Player owner; 

    // Start is called before the first frame update
    void Start()
    {
        MainCameraTransform = gameObject.GetComponentInParent<Transform>();

        Player owner = photonView.Owner;

        usernameDisplayText.text = owner.NickName;
    }

    // Update is called once per frame
    void LateUpdate()
    {
        // Make the interface always face the player
        transform.LookAt(MainCameraTransform);

        // Rotate the interface by 180 degrees to fix the backwards text problem
        //transform.Rotate(Vector3.up, 180f);
    }
}
