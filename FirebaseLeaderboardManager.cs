using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Firebase;
using Firebase.Database;
using Firebase.Extensions;

public class FirebaseLeaderboardManager : MonoBehaviour
{
    public TMP_InputField nameInputField;
    public GameObject nameColumn;
    public GameObject timeColumn;
    public GameObject nameEntryPrefab;
    public GameObject timeEntryPrefab;

    private DatabaseReference dbReference;
    private bool scoreSubmitted = false;

    void Start()
    {
        dbReference = FirebaseDatabase.DefaultInstance.RootReference;

        // Load leaderboard for current floor
        LoadLeaderboard();
    }

    public void SubmitScore()
    {
        if (scoreSubmitted) return;

        string playerName = nameInputField.text.Trim();
        if (string.IsNullOrEmpty(playerName)) return;

        float time = GameData.finalTime;
        string floorName = GameData.floorName;

        string key = dbReference.Child("leaderboard").Child(floorName).Push().Key;
        LeaderboardEntry entry = new LeaderboardEntry(playerName, time);
        string json = JsonUtility.ToJson(entry);

        dbReference.Child("leaderboard").Child(floorName).Child(key).SetRawJsonValueAsync(json).ContinueWithOnMainThread(task =>
        {
            if (task.IsCompleted)
            {
                Debug.Log("Score submitted.");
                scoreSubmitted = true;
                LoadLeaderboard(); // Refresh leaderboard after submission
            }
            else
            {
                Debug.LogWarning("Failed to submit score: " + task.Exception);
            }
        });
    }

    public void LoadLeaderboard()
    {
        string floorName = GameData.floorName;

        dbReference.Child("leaderboard").Child(floorName).OrderByChild("time").LimitToFirst(10)
        .GetValueAsync().ContinueWithOnMainThread(task =>
        {
            if (task.IsFaulted || task.IsCanceled)
            {
                Debug.LogWarning("Failed to load leaderboard.");
                return;
            }

            // Clear old entries
            foreach (Transform child in nameColumn.transform) Destroy(child.gameObject);
            foreach (Transform child in timeColumn.transform) Destroy(child.gameObject);

            DataSnapshot snapshot = task.Result;
            foreach (DataSnapshot childSnapshot in snapshot.Children)
            {
                IDictionary<string, object> entry = (IDictionary<string, object>)childSnapshot.Value;

                string name = entry.ContainsKey("name") ? entry["name"].ToString() : "???";
                float time = entry.ContainsKey("time") ? float.Parse(entry["time"].ToString()) : 0f;

                // Format time as mm:ss.ms
                int minutes = Mathf.FloorToInt(time / 60);
                int seconds = Mathf.FloorToInt(time % 60);
                int milliseconds = Mathf.FloorToInt((time * 100) % 100);
                string timeFormatted = $"{minutes:00}:{seconds:00}.{milliseconds:00}";

                Instantiate(nameEntryPrefab, nameColumn.transform).GetComponent<TextMeshProUGUI>().text = name;
                Instantiate(timeEntryPrefab, timeColumn.transform).GetComponent<TextMeshProUGUI>().text = timeFormatted;
            }
        });
    }
}

[System.Serializable]
public class LeaderboardEntry
{
    public string name;
    public float time;

    public LeaderboardEntry(string name, float time)
    {
        this.name = name;
        this.time = time;
    }
}
