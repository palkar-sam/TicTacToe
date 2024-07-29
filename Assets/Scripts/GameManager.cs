using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private static GameManager _instance;
    private static readonly object lockObj = new object();

    public static GameManager Instance => _instance;

    public bool IsMultiplayer { get; set; }
    public bool Is1vs1Enabled { get; set; }
    public bool IsSinglePlayer { get; set; }

    public string UserName { get; set; } = "Dummy";
    public string UserColorCode;
    public string AiColorCode;

    private void Awake()
    {
        Application.targetFrameRate = Screen.currentResolution.refreshRate;
        Time.fixedDeltaTime = 1.0f / Application.targetFrameRate;
        if (_instance != null && _instance != this)
        {
            Destroy(gameObject);
        }
        else if (_instance == null)
        {
            lock (lockObj)
            {
                _instance = this;
                DontDestroyOnLoad(gameObject);
            }
        }
    }

    private void Update()
    {
        if (Application.platform == RuntimePlatform.Android)
        {

            // Check if Back was pressed this frame
            if (Input.GetKeyDown(KeyCode.Escape))
            {

                // Quit the application
                Application.Quit();
            }
        }
    }
}
