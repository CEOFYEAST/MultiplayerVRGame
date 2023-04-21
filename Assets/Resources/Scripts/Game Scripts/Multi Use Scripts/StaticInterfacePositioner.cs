using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StaticInterfacePositioner : MonoBehaviour
{

    [SerializeField] private Transform playerCameraTransform; // assign the player camera in the Inspector
    [SerializeField] private float distanceFromPlayer = 2.5f; // set the desired distance from the player in the Inspector

    private void Start()
    {
        MoveCanvasToPlayerPositionAndRotation();
    }

    private void OnEnable()
    {
        MoveCanvasToPlayerPositionAndRotation();
    }

    private void MoveCanvasToPlayerPositionAndRotation()
    {
        if (playerCameraTransform != null)
        {
            // Calculate the new position of the canvas in front of the player camera
            Vector3 newPosition = playerCameraTransform.position + playerCameraTransform.forward * distanceFromPlayer;

            // makes sure the menu never shows up below or partially in the floor
            if(newPosition.y < .6f){
                newPosition.y = .6f;
            }

            // Position and rotate the canvas to be level with the player camera
            transform.position = newPosition;
            transform.rotation = Quaternion.Euler(0, playerCameraTransform.rotation.eulerAngles.y, 0);
        }
        else
        {
            Debug.LogError("Player camera transform is null!");
        }
    }
}
