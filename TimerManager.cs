using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class TimerManager : MonoBehaviour
{
    public TextMeshProUGUI timerText;
    public bool enableGameOver = true; // Toggle for GameOver on timeout

    private float timeElapsed;
    private float timeAdjustment; // Total reward/penalty applied
    private bool isRunning = true;

    void Start()
    {
        timeElapsed = 0f;
        timeAdjustment = 0f;
        isRunning = true;
    }

    void Update()
    {
        if (isRunning)
        {
            timeElapsed += Time.deltaTime;

            // Check for game over (3 min = 180s)
            if (enableGameOver && GetTime() > 180f)
            {
                isRunning = false;
                SceneManager.LoadScene("GameOver");
            }

            UpdateTimerUI();
        }
    }

    void UpdateTimerUI()
    {
        float totalTime = GetTime();
        int minutes = Mathf.FloorToInt(totalTime / 60);
        int seconds = Mathf.FloorToInt(totalTime % 60);
        int milliseconds = Mathf.FloorToInt((totalTime * 100) % 100);

        timerText.text = string.Format("{0:00}:{1:00}.{2:00}", minutes, seconds, milliseconds);
    }

    public void StopTimer()
    {
        isRunning = false;
    }

    public float GetTime()
    {
        return timeElapsed + timeAdjustment;
    }

    public void ApplyTimeAdjustment(float seconds)
    {
        timeAdjustment += seconds;
        Debug.Log($"Time adjusted by {seconds} seconds. Total adjustment: {timeAdjustment}");
    }

    public void AdjustTime(float amount)
    {
        timeElapsed += amount;
        if (timeElapsed < 0f) timeElapsed = 0f; // Clamp to zero if needed
        UpdateTimerUI();
    }
}
