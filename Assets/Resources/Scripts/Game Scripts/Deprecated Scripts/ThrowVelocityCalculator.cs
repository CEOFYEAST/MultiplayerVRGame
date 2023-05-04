using System;
using UnityEngine;

public class ThrowVelocityCalculator : MonoBehaviour
{
    [SerializeField]
    private int velocityOnReleaseScale;

    private Rigidbody rigidbody;

    void Start(){
        rigidbody = gameObject.GetComponent<Rigidbody>();
    }

    public void ScaleVelocity(){
        rigidbody.velocity = rigidbody.velocity * velocityOnReleaseScale;
    }
}
