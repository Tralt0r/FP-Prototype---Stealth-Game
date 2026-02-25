using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using TMPro;

public class CS_Timer : MonoBehaviour
{
    [Header("Timer Settings")]
    public float startTime = 10f;
    public float currentTime;
    private bool isRunning = false;

    [Header("UI")]
    [Tooltip("Drag your TextMeshProUGUI component here")]
    public TextMeshProUGUI timerText; 

    [Header("Timer Event")]
    public UnityEvent onTimerEnd;

    void Start()
    {
        if (timerText == null)
        {
            timerText = GetComponentInChildren<TextMeshProUGUI>();
            if (timerText == null)
                Debug.LogWarning("Timer TextMeshProUGUI not assigned or found in children.");
        }

        StartTimer();
    }

    void Update()
    {
        if (!isRunning) return;

        currentTime -= Time.deltaTime;

        if (currentTime <= 0f)
        {
            currentTime = 0f;
            isRunning = false;

            UpdateUI();
            TimerEnded();
            return;
        }

        UpdateUI();
    }

    public void StartTimer()
    {
        currentTime = startTime;
        isRunning = true;
        UpdateUI();
    }

    public void StopTimer()
    {
        isRunning = false;
    }

    public void ResetTimer()
    {
        currentTime = startTime;
        UpdateUI();
    }

    void TimerEnded()
    {
        Debug.Log("Timer Finished!");
        onTimerEnd?.Invoke();
    }

    void UpdateUI()
    {
        if (timerText == null) return;

        int minutes = Mathf.FloorToInt(currentTime / 60f);
        int seconds = Mathf.FloorToInt(currentTime % 60f);

        timerText.text = $"{minutes:00}:{seconds:00}";
    }
    
    // For showing other scripts the time
    #region

    // If we need another script to look at the time
    public float GetElapsedTime()
    {
        return currentTime;
    }

    #endregion
}
