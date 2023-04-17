using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class LevelTimerUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI timerTxt;

    private float timeSinceStart = 0f;

    private void Start()
    {
        timerTxt.text = ConvertTimerToString(timeSinceStart);
    }

    private void Update()
    {
        if (ServiceLocator.instance.GetService<Spawner>().swarmInProgress)
        {
            timeSinceStart += Time.deltaTime;

            timerTxt.text = ConvertTimerToString(timeSinceStart);
        }
    }

    private string ConvertTimerToString(float timerValue)
    {
        int minutes = Mathf.FloorToInt(timerValue / 60f);
        int seconds = Mathf.FloorToInt(timerValue % 60f);
        string timerString = string.Format("{0:00}:{1:00}", minutes, seconds);
        return timerString;
    }
}
