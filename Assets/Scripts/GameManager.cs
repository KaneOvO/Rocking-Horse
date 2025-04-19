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

    public Camera endGameCamera;

    private float endGameTimer;
    private bool endGameTimerStarted;
    private bool isGameEnd;
    
    public static bool IsGameBegin
    {
        get => TimeBeforeStart <= 0;
    }

    public static bool IsStarted { get; private set; } = false;

    public static Action GameStartEvent;

    public int lapCount;
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
        if (isGameEnd)
            return;
        
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

        if (finalRanking.Count > 0)
        {
            endGameTimerStarted = true;
        }

        if (endGameTimerStarted)
        {
            endGameTimer += Time.deltaTime;

            if (endGameTimer >= 60)
            {
                isGameEnd = true;
                GameEnd();
            }
        }
        
        if (finalRanking.Count >= 4)
        {
            isGameEnd = true;
            GameEnd();
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
        
        // Add method to restart game
    }

    private void ShowPlayerPlacement()
    {
        LevelManager.Instance.Podium.SetActive(true);
        
        endGameCamera.enabled = true;
        CameraManager.Instance.DisableAllPlayerCamera();

        for (var i = 0; i < finalRanking.Count; i++)
        {
            var rank = i + 1;
            var (time, controller) = finalRanking[i];
            
            string nameDisplay = $"{controller.gameObject.name}" + "\n";
            string timeDisplay = $"{TimeSpan.FromSeconds(time):mm\\:ss\\:ff}";
            
            var playerScoreBoard = controller.gameObject.GetComponent<LapCounter>().ScoreBoard;
            playerScoreBoard.transform.GetChild(0).GetComponent<TMP_Text>().text = nameDisplay + timeDisplay;
            playerScoreBoard.SetActive(true);
            
            //var playerTransform = LevelManager.Instance.EndGameHorsePos[i];
            //controller.gameObject.transform.SetPositionAndRotation(playerTransform.position, playerTransform.rotation);
        }
        
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
