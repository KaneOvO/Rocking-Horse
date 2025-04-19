using System;
using System.Collections;
using System.Collections.Generic;
using Character;
using GameSystem.Input;
using TMPro;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum PlayerColor
{
    Red,
    yellow,
    blue,
    green
}

[DefaultExecutionOrder(-3)]
public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    public bool RequiredAltController = true;
    public static float TimeBeforeStart = 3;
    public int PlayerCount = 1;
    public GameObject PlayerPrefab;
    public GameObject NPCPrefab;
    public List<GameObject> Players = new List<GameObject>();
    
    public static bool IsGameBegin
    {
        get => TimeBeforeStart <= 0;
    }

    public static bool IsStarted { get; private set; } = false;

    public static Action GameStartEvent;

    public int lapCount = 2;
    public List<(float, HorseController)> finalRanking = new();
    
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }

        //IsStarted = !RequiredAltController;
    }

    public void StartGame()
    {
        IsStarted = true;
        
        finalRanking.Clear();
    }

    private void Update()
    {
        if (!IsStarted)
        {
            CheckAllConnected();
        }

        //if (IsStarted && TimeBeforeStart > 0)
        //{
        //    TimeBeforeStart -= Time.deltaTime;
        //    if(TimeBeforeStart < 0) GameStartEvent?.Invoke();
        //}

        //Debug.Log(IsStarted);

        //Debug.Log(TimeBeforeStart);

        if (finalRanking.Count >= PlayerCount)
        {
            //GameEnd();
            Debug.Log("All players finished the game");
        }

        if(Input.GetKeyDown(KeyCode.R))
        {
            SceneManager.LoadScene(0);
        }

        if (Input.GetKeyDown(KeyCode.K))
        {
            MultiSerialManager.Instance.ResearchDevice();
            Debug.Log("Reseach!");
        }
    }
    
    private void GameEnd()
    {
        ShowPlayerPlacement();
    }

    private void ShowPlayerPlacement()
    {
        LevelManager.Instance.Podium.SetActive(true);
        
        // move camera

        for (var i = 0; i < finalRanking.Count; i++)
        {
            var rank = i + 1;
            var (time, controller) = finalRanking[i];
            
            string nameDisplay = $"{controller.gameObject.name}" + "\n";
            string timeDisplay = $"{TimeSpan.FromSeconds(time):mm\\:ss\\.ff}";
            
            controller.gameObject.transform.GetChild(4).GetChild(0).GetComponent<TextMeshProUGUI>().text = nameDisplay + timeDisplay;
        }
        
        // RestartGame();
    }
    
    public void RestartGame()
    {
        TimeBeforeStart = 3;
        Time.timeScale = 1f; // Unfreeze time
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex); // Reload current scene
    }

    public void CleanPlayers()
    {
        Players.Clear();
    }
    
    void CheckAllConnected()
    {
        bool allConnected = true;
        for (int i = 0; i < Players.Count; i++)
        {
            if (Players[i] != null)
            {
                if (!Players[i].GetComponent<HardwareController>().myListener.isConnected)
                {
                    allConnected = false;
                    break;
                }
            }
        }
        if (allConnected)
        {
            //IsStarted = true;
        }
    }

    
}
