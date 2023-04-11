using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PositionUsernameDisplay : MonoBehaviour
{
    private Transform MainCameraTransform;

    // Start is called before the first frame update
    void Start()
    {
        MainCameraTransform = gameObject.GetComponentInParent<Transform>();
    }

    // Update is called once per frame
    void LateUpdate()
    {
        // Make the interface always face the player
        transform.LookAt(MainCameraTransform);

        // Rotate the interface by 180 degrees to fix the backwards text problem
        transform.Rotate(Vector3.up, 180f);
    }
}
