using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.UI;
using TMPro;

public class MyGameLoop : MonoBehaviour
{
    [SerializeField]
    private TMPro.TextMeshProUGUI popupText;

    [SerializeField]
    private TMPro.TextMeshProUGUI timerText;

    // variables required for starting timer
    private int startingTimerLength = 15;
    private int startingTimerCurrentInterval;

    // variables required for game timer
    private int timerLength = 60;
    private int timerCurrentInterval;

    // Start is called before the first frame update
    void Start()
    {
        // sets current intervals
        startingTimerCurrentInterval = startingTimerLength;
        timerCurrentInterval = timerLength;

        // starts starting timer
        gameObject.GetComponent<Timers>().StartTimer(startingTimerLength);
    }

    public void UpdatePopupText(){
        popupText.text = "Starting In " + startingTimerCurrentInterval;
        startingTimerCurrentInterval--;
    }

    public void StartGameLoop(){
        popupText.transform.parent.gameObject.SetActive(false);
    }
}
