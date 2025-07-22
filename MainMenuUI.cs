using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuUI : MonoBehaviour
{
    [SerializeField] private Button playButton;
    [SerializeField] private Button fireExtButton;
    [SerializeField] private Button smallfireButton;
    [SerializeField] private Button multiButton;
    [SerializeField] private Button aboutButton;
    [SerializeField] private Button quitButton;
    void Start()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }


    private void Awake()
    {
        playButton.onClick.AddListener(() => {
            SceneManager.LoadScene("Cutscene");
        });
        fireExtButton.onClick.AddListener(() => {
            SceneManager.LoadScene("FireExt");
        });
        smallfireButton.onClick.AddListener(() => {
            SceneManager.LoadScene("Small");
        });
        multiButton.onClick.AddListener(() => {
            SceneManager.LoadScene("Loading");
        });
        quitButton.onClick.AddListener(() => {
            Application.Quit();
        });
    }
}

