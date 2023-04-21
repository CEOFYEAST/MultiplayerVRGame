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
        MainCameraTransform = GameObject.Find("Main Camera").transform;

        Debug.Log("Main Camera Transform: " + MainCameraTransform);

        Player owner = photonView.Owner;

        usernameDisplayText.text = owner.NickName;
    }

    // Update is called once per frame
    void LateUpdate()
    {
        // Get the direction to the target
        Vector3 directionToTarget = MainCameraTransform.position - transform.position;
            
        // Calculate the rotation to face the target
        Quaternion targetRotation = Quaternion.LookRotation(directionToTarget);
        
        // Remove the pitch component of the rotation
        targetRotation = Quaternion.Euler(0f, targetRotation.eulerAngles.y, 0f);
        
        // Apply the rotation to the UI
        transform.rotation = targetRotation;
    }
}
