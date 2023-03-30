using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//class meant to return once the GameObject the script is attached to is fully loaded along with all it's children
public class Loaded : MonoBehaviour
{   
    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("Checking Loaded Status");
        StartCoroutine(WaitUntilFullyLoaded(gameObject));
    }

    /// <summary>
    /// IEnumerator that returns once a GameObject "go", its children and all their components are fully loaded in
    /// extra code at the bottom that calls methods to manage input on the newly loaded rig
    /// written by chatGPT
    /// <summary>
    IEnumerator WaitUntilFullyLoaded(GameObject go)
    {
        yield return new WaitForEndOfFrame(); // Wait for the current frame to finish rendering

        if (go.transform.parent != null) // Wait for parent to be fully loaded
        {
            yield return StartCoroutine(WaitUntilFullyLoaded(go.transform.parent.gameObject));
        }

        bool allChildrenLoaded = false;
        while (!allChildrenLoaded) // Wait for all children to be fully loaded
        {
            allChildrenLoaded = true;
            foreach (Transform child in go.transform)
            {
                if (!child.gameObject.activeSelf || !child.gameObject.scene.isLoaded)
                {
                    allChildrenLoaded = false;
                    yield return new WaitForEndOfFrame();
                    break;
                }
            }
        }

        // All parents and children are fully loaded in at this point
        Debug.Log("GameObject " + go.name + " and all of its parents and children are fully loaded in.");

        

        // checks if the newly loaded rig belongs to the local player and disables all components on it if it doesn't
        //GameObject.FindObjectOfType<MyInputManagerViewTest>().DisableComponentsInNonLocalRig(gameObject);
    }
}
