using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// class that defines a timer which
/// - takes a given time to count down to
/// - calls a given method at second intervals, passing it's current time as a parameter
/// - calls a given method when finished
/// <summary>
//can you write a class with a timer coroutine that takes a given time to count down to as a parameter, calls a given method at second intervals, passing it's current time as a parameter, and calls a given method when finished. I imagine the methods will be supplied via unity events
public class Timers : MonoBehaviour
{
    [SerializeField]
    private UnityEvent m_callAtInterval;

    [SerializeField]
    private UnityEvent m_callAtFinish;

    public void StartTimer(int length)
    {
        StartCoroutine(Timer(length));
    }

    private IEnumerator Timer(int length){
        length--;

        while (true)
        {
            //updates every tenth second 
            for (int i = 9; i >= 0; i--)
            {
                yield return new WaitForSeconds(.1f);
            }
            if (length == 0)
            {
                m_callAtFinish.Invoke();
                break;
            }
            length--;

            m_callAtInterval.Invoke();
        }
    }
}
