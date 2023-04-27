using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UpdateText : MonoBehaviour
{
    public int highScore = 0;
    public int score = 0;

    public AudioSource scoreAudio;
    public AudioSource timerIncrement;

    public TMPro.TextMeshProUGUI scoreText;
    public TMPro.TextMeshProUGUI highScoreText;
    public TMPro.TextMeshProUGUI timerText;
    public GameObject eventCamera;

    public void TextChange()
    {
        scoreAudio.Play();
        score += 1;
        scoreText.text = score.ToString();
    }

    public void StartTimer(int length)
    {
        if(length == 5)
        {
            StartCoroutine(CoroutineOne(length));
        } else
        {
            StartCoroutine(CoroutineTwo(length));
        }
    }

    IEnumerator CoroutineOne(int length)
    {
        length--;
        while (true)
        {
            timerIncrement.Play();
            //updates every tenth second 
            for (int i = 9; i >= 0; i--)
            {
                timerText.text = length.ToString() + ':' + i.ToString();
                yield return new WaitForSeconds(.1f);
            }
            if (length == 0)
            {
                timerText.text = "0:0";
                eventCamera.GetComponent<GameLoop>().BlockOne();
                break;
            }
            length--;
        }
    }

    IEnumerator CoroutineTwo(int length)
    {
        length--;
        while (true)
        {
            if(length > 10)
            {
                if (length % 5 == 0)
                {
                    timerIncrement.Play();
                }
            } else
            {
                timerIncrement.Play();
            }
            //updates every tenth second 
            for (int i = 9; i >= 0; i--)
            {
                timerText.text = length.ToString() + ':' + i.ToString();
                yield return new WaitForSeconds(.1f);
            }
            if (length == 0)
            {
                timerText.text = "0:0";
                eventCamera.GetComponent<GameLoop>().BlockTwo();
                break;
            }
            length--;
        }
    }
}
