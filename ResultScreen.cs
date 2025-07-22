using UnityEngine;
using TMPro;

public class ResultScreen : MonoBehaviour
{
    public TextMeshProUGUI resultTimeText;

    void Start()
    {
        float time = GameData.finalTime;

        int minutes = Mathf.FloorToInt(time / 60);
        int seconds = Mathf.FloorToInt(time % 60);
        int milliseconds = Mathf.FloorToInt((time * 100) % 100);

        resultTimeText.text = $"Time: {minutes:00}:{seconds:00}.{milliseconds:00}";
    }
}
