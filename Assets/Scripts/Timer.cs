using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class Timer : MonoBehaviour
{
    private bool timerStarted = true; // true - секундомер запущен
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

    private void UpdateTimerText()
    {
        if (timeLeft < 0) timeLeft = 0;
 
        TimeSpan time = TimeSpan.FromSeconds(timeLeft);
        timerText.text = string.Format("{0:00} : {1:00}", time.Minutes, time.Seconds);
    }
}