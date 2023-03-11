using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GetParentBounds : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Debug.Log(gameObject.GetComponent<Renderer>().bounds);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
