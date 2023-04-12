using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class InterfacePositioner : MonoBehaviour
{
    public Transform playerCamera;
    [SerializeField]
    private float distance = 1.5f;

    private void LateUpdate()
    {
        // Calculate the target position based on the player's position and rotation
        Vector3 targetPosition = playerCamera.position + playerCamera.forward * distance;

        // Move the interface to the target position
        transform.position = targetPosition;

        // Make the interface always face the player
        transform.LookAt(playerCamera);

        // Rotate the interface by 180 degrees to fix the backwards text problem
        transform.Rotate(Vector3.up, 180f);
    }
}
