using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnBallOnRelease : MonoBehaviour
{
    //public GameObject basketballLeft;
    //public GameObject basketballRight;
    public GameObject rightHand;

    public void onRelease(GameObject basketball)
    {
        StartCoroutine(Coroutine(basketball));
        //Debug.Log("run");
        //GameObject newBall = GameObject.Instantiate(basketball);
        //newBall.GetComponent<Rigidbody>().velocity = Vector3.zero;
        //newBall.GetComponent<Rigidbody>().angularVelocity = Vector3.zero;
        //newBall.transform.position = rightHand.transform.position;
        //newBall.transform.position = newBall.transform.position + new Vector3(0, 1, 0);
    }

    
    IEnumerator Coroutine(GameObject basketball)
    {
        yield return new WaitForSeconds(1);
        GameObject newBall = GameObject.Instantiate(basketball);
        //newBall.GetComponent<Rigidbody>().velocity = Vector3.zero;
        //newBall.GetComponent<Rigidbody>().angularVelocity = Vector3.zero;
        newBall.transform.position = rightHand.transform.position + new Vector3(0, .25f, 0);
        newBall.GetComponent<Rigidbody>().useGravity = true;
        newBall.GetComponent<Rigidbody>().angularDrag = .05f;
        //newBall.transform.position = newBall.transform.position 
    }

}
