using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class Timer : MonoBehaviour
{
    private bool timerStarted = false; // true - секундомер запущен
    public float timeLeft = 0f;
    [SerializeField] private TextMeshProUGUI timerText;

    private void Start()
    {
        UpdateTimerText();
    }

    private void Update()
    {
        if(timerStarted)
        {
            if(timeLeft > 0)
            {
                timeLeft -= Time.deltaTime;
                UpdateTimerText();
            }
        }
    }

    public void StartTimer()
    {
        timerStarted = true;
    }

    private void UpdateTimerText()
    {
        if (timeLeft < 0) timeLeft = 0;
 
        TimeSpan time = TimeSpan.FromSeconds(timeLeft);
        var totalSeconds = time.Minutes * 60 + time.Seconds;
        timerText.text = string.Format($"{totalSeconds}");
    }
}