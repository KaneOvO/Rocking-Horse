using System;
using System.Collections;
using System.Collections.Generic;
using GameSystem.Input;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

[DefaultExecutionOrder(-3)]
public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    public bool RequiredAltController = true;
    public static float TimeBeforeStart = 3;
    public int PlayerCount = 1;
    public GameObject PlayerPrefab;
    public List<GameObject> Players = new List<GameObject>();

    public static bool IsGameBegin
    {
        get => TimeBeforeStart <= 0;
    }

    public static bool IsStarted { get; private set; } = false;

    public static Action GameStartEvent;
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

        IsStarted = !RequiredAltController;
        //MyListener.OnEquipmentConnected += OnControllerConnected;
    }
    private void nDestroy()
    {
       // MyListener.OnEquipmentConnected -= OnControllerConnected;
    }
    private void OnControllerConnected(SensorData data)
    {
        IsStarted = true;
    }

    private void Update()
    {
        if (!IsStarted)
        {
            CheckAllConnected();
        }

        if (IsStarted && TimeBeforeStart > 0)
        {
            TimeBeforeStart -= Time.deltaTime;
            if(TimeBeforeStart <= 0) GameStartEvent?.Invoke();
        }
    }
    public void RestartGame()
    {
        TimeBeforeStart = 3;
        Time.timeScale = 1f; // Unfreeze time
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex); // Reload current scene
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
            IsStarted = true;
        }
    }

    
}
