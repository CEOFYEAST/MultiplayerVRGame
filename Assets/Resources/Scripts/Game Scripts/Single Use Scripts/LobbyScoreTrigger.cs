using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.UI;
using TMPro;

public class LobbyScoreTrigger : MonoBehaviour
{
    private int scoreCounter = 0;

    [SerializeField]
    private TMPro.TextMeshProUGUI scoreText;

    // Start is called before the first frame update
    void Start()
    {
        scoreText.text = scoreCounter.ToString();
    }

    public void OnTriggerEnter(Collider other)
    {
        // makes sure the calling object is a basketball
        if(other.GetComponent<IsBasketball>() != null)
        {
            scoreCounter += 1;

            scoreText.text = scoreCounter.ToString();

            // destroys the scoring basketball so it can only count for one score
            GameObject.Destroy(other.gameObject);
        }
    }
}
