using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class HeartsTimeUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI timerText;
    private int heartRecoveryTime = 300;
    private float timer;

    private void Start()
    {

        string lastTimeString = PlayerPrefs.GetString("LastPlayTime", "");
        if (!string.IsNullOrEmpty(lastTimeString))
        {   
            DateTime lastPlayTime = DateTime.Parse(lastTimeString);
            TimeSpan timeAway = DateTime.Now - lastPlayTime;
            timer = heartRecoveryTime - (float)timeAway.TotalSeconds % heartRecoveryTime;
        }
        else
        {
            timer = heartRecoveryTime;
        }
    }

    private void Update()
    {
        if (StatsManager.Instance.GetCurrentHearts() < 5)
        {
            timer -= Time.deltaTime;
            if (timer <= 0)
            {
                timer = heartRecoveryTime;
            }

            TimeSpan timeLeft = TimeSpan.FromSeconds(timer);
            timerText.text = $"{timeLeft.Minutes:D2}:{timeLeft.Seconds:D2}";
        }
        else
        {
            timerText.text = "Full";
        }
    }
}
