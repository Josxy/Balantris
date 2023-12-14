using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System;
using UnityEngine.Pool;
//timer function
public class Timer : MonoBehaviour
{
    public TextMeshProUGUI time;
    public float currentTime = 0f;
    public int remainingSec = 10;
    public string timeVar;

    void Start()
    {
        currentTime = remainingSec;
    }

    public void startCount()
    {
        TimeSpan timer = TimeSpan.FromSeconds(currentTime);
        time.text = timeVar + timer.Seconds.ToString();
        currentTime -= Time.deltaTime;
    }

    public void resetCount()
    {
        currentTime = remainingSec;
    }
}
