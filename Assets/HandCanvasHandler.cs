using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class HandCanvasHandler : MonoBehaviour
{
    // camera to face
    public Transform playerCamera;

    // Update is called once per frame
    void Update()
    {
        // Make the interface always face the player
        transform.LookAt(playerCamera);

        // Rotate the interface by 180 degrees to fix the backwards text problem
        transform.Rotate(Vector3.up, 180f);
    }
}
