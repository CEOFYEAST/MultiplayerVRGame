using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameLoop : MonoBehaviour
{
    public TMPro.TextMeshProUGUI popupText;
    public GameObject eventCamera;
    public int roundLength;
    public GameObject originBall;
    public GameObject trigger;
    public GameObject rightHand;
    public GameObject leftHand;
    public AudioSource gameStart;
    public AudioSource loss;
    public AudioSource win;

    private Vector3 originPosition = new Vector3(-3.96700001f, 0.289999992f, 0);
    private int updateIterations = 0;

    private bool gameLoopStarted = false;
    
    public void Update()
    {
        if (updateIterations % 100 == 0)
        {
            if (eventCamera.GetComponent<EffectDuringRuntime>().startGameLoop == 1 && gameLoopStarted == false)
            {
                gameLoopStarted = true;
                StartGameplayLoop();
            }
        }
        updateIterations++;

    }

    public void StartGameplayLoop()
    {
        rightHand.SetActive(true);
        leftHand.SetActive(true);

        eventCamera.GetComponent<SetHandInteractionMask>().enableBoth();

        originBall.SetActive(true);

        popupText.text = "";

        eventCamera.GetComponent<UpdateText>().score = 0;
        eventCamera.GetComponent<UpdateText>().scoreText.text = eventCamera.GetComponent<UpdateText>().score.ToString();
        eventCamera.GetComponent<UpdateText>().highScoreText.text = eventCamera.GetComponent<UpdateText>().highScore.ToString();

        popupText.text = "BEAT  THE  HIGH  SCORE";

        eventCamera.GetComponent<UpdateText>().StartTimer(5);
    }

    public void BlockOne()
    {
        gameStart.Play();

        trigger.GetComponent<Collider>().isTrigger = true;

        originBall.transform.position = originPosition;
        originBall.GetComponent<Rigidbody>().velocity = Vector3.zero;
        originBall.GetComponent<Rigidbody>().angularVelocity = Vector3.zero;

        popupText.text = "";

        eventCamera.GetComponent<UpdateText>().StartTimer(roundLength);
    }

    public void BlockTwo()
    {
        /*
        while (true)
        {
            GameObject toDestroy = GameObject.Find("Basketball Ball");
            if(toDestroy == null)
            {
                break;
            }
            Destroy(toDestroy);
        }
        */

        //eventCamera.GetComponent<SetHandInteractionMask>().disableBoth();

        rightHand.SetActive(false);
        leftHand.SetActive(false);

        originBall.SetActive(false);

        gameLoopStarted = false;

        StartCoroutine(DeleteBallClones());

         trigger.GetComponent<Collider>().isTrigger = false;

        if (eventCamera.GetComponent<UpdateText>().score == eventCamera.GetComponent<UpdateText>().highScore)
        {
            popupText.text = "TIE";
            win.Play();
            return;
        }
        else if (eventCamera.GetComponent<UpdateText>().score > eventCamera.GetComponent<UpdateText>().highScore)
        {
            eventCamera.GetComponent<UpdateText>().highScore = eventCamera.GetComponent<UpdateText>().score;
            popupText.text = "YOU  WIN";
            win.Play();
            return;
        }
        else
        {
            popupText.text = "YOU  LOSE";
            loss.Play();
            return;
        }
    }

    IEnumerator DeleteBallClones()
    {
        Debug.Log("destroying///");
        yield return new WaitForSeconds(1);
        Debug.Log("destroying///");
        var objects = GameObject.FindGameObjectsWithTag("Basketball");
        foreach (var obj in objects)
        {
            if (obj.name.Contains("Clone"))
            {
                Destroy(obj);
            }
        }
    }
}
