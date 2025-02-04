using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public bool RequiredAltController = true;
    public static float TimeBeforeStart = 3;

    public static bool IsGameBegin
    {
        get => TimeBeforeStart <= 0;
    }

    public static bool IsStarted { get; private set; } = false;

    public static Action GameStartEvent;
    private void Awake()
    {
        IsStarted = !RequiredAltController;
        MyListener.OnEquipmentConnected += OnControllerConnected;
    }
    private void nDestroy()
    {
        MyListener.OnEquipmentConnected -= OnControllerConnected;
    }
    private void OnControllerConnected(SensorData data)
    {
        IsStarted = true;
    }

    private void Update()
    {
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
}
