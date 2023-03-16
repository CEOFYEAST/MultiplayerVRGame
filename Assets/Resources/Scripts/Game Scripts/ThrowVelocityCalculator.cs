using System;
using UnityEngine;

public class ThrowVelocityCalculator : MonoBehaviour
{
    public float distance = 0;
    public float throwVelocity = 0;
    //tweak the scale if shots are off
    public float scale = 4.5f;

    private Vector3 goalPos;
    private Vector3 hmdPos;

    private GameObject goalRing;
    private GameObject mainCamera;
    //public GameObject basketball;

    public int updateIterations = 0;

    void Start()
    {
        goalRing = GameObject.Find("Goal Ring");
        mainCamera = GameObject.Find("Main Camera");
        goalPos = goalRing.transform.position;
    }

    //algorithm that sets the velocity of a shot proportional to the distance from the goal 
    void Update()
    {
        if (updateIterations % 50 == 0)
        {
            hmdPos = mainCamera.transform.position;
            distance = (float)Math.Sqrt(Math.Pow(hmdPos.x, 2) + Math.Pow(hmdPos.z, 2));
            throwVelocity = distance * scale;
            //float velocityScale = distance/
            //basketball.GetComponent<UnityEngine.XR.Interaction.Toolkit.XRGrabInteractable>().throwVelocityScale = throwVelocity;
            GetComponent<UnityEngine.XR.Interaction.Toolkit.XRGrabInteractable>().throwVelocityScale = throwVelocity;
            
        }
        updateIterations += 1;
    }
}
