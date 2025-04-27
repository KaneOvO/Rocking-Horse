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
    blue,
    yellow,
    green
}

[DefaultExecutionOrder(-3)]
public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    public bool RequiredAltController = true;
    public static float TimeBeforeStart = 3;
    public int PlayerCount = 0;
    public int deviceCount = 0;
    public GameObject PlayerPrefab;
    public GameObject NPCPrefab;
    public List<GameObject> Players = new List<GameObject>();
    public GameObject pauseMenu;
    public GameObject endGameCanvas;
    public PauseAndEndGameUIController PauseAndEndGameUIController;

    private float endGameTimer;
    private bool endGameTimerStarted;
    private bool isGameEnd;
    public static event Action OnGameEnded;

    public static bool IsGameEnded => Instance != null && Instance.isGameEnd;


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
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;

        if (SceneManager.GetActiveScene().name != "TitleScreenScene")
        {
            DontDestroyOnLoad(gameObject); // Only persist in gameplay scenes
        }
    }


    public void StartGame()
    {
        IsStarted = true;

        finalRanking.Clear();
        endGameTimerStarted = false;
        endGameTimer = 0f;
        isGameEnd = false;

        RaceTimer.Instance?.ResetTimer();
        MusicManager.Instance?.ResetMusicTriggers();

        foreach (var horse in HorseController.Horses)
        {
            horse.ResetRaceState();
        }
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
        
        if (finalRanking.Count >= PlayerCount)
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

        if (Input.GetKeyDown(KeyCode.P))
        {
            PauseAndEndGameUIController.TogglePause();
            Debug.Log("Pause!");
        }
    }

    private void GameEnd()
    {
        // DNF handling logic
        for (int i = 0; i < PlayerCount; i++)
        {
            string uiName = $"UI{i + 1}";
            GameObject uiObject = GameObject.Find(uiName);

            if (uiObject != null)
            {
                Transform finish = uiObject.transform.Find("Finish");
                Transform dnf = uiObject.transform.Find("DNF");

                if (finish != null && dnf != null)
                {
                    if (!finish.gameObject.activeSelf)
                    {
                        dnf.gameObject.SetActive(true);
                    }
                }
            }
            else
            {
                Debug.LogWarning($"UI object {uiName} not found!");
            }
        }

        StartCoroutine(HandleGameEndSequence());
        OnGameEnded?.Invoke();
    }


    private void ShowPlayerPlacement()
    {
        // Fill final ranking if incomplete
        if (finalRanking.Count != PlayerCount)
        {
            finalRanking.Clear();
            foreach (var player in Players)
            {
                var controller = player.GetComponent<HorseController>();
                float time = controller.HasFinished ? controller.FinishTime : 0f;
                finalRanking.Add((time, controller));
            }

            // Sort by finish time; DNF (time = 0) goes last
            finalRanking.Sort((a, b) =>
            {
                if (a.Item1 == 0) return 1;
                if (b.Item1 == 0) return -1;
                return a.Item1.CompareTo(b.Item1);
            });
        }

        var podium = LevelManager.Instance.Podium;
        podium.SetActive(true);

        LevelManager.Instance.EndGameCamera.enabled = true;
        CameraManager.Instance.DisableAllPlayerCamera();

        // Ensure enough podium slots
        Transform podiumTransform = podium.transform;
        GameObject podiumSlotTemplate = podiumTransform.childCount > 0
            ? podiumTransform.GetChild(0).gameObject
            : new GameObject("PodiumSlotTemplate");

        for (int i = podiumTransform.childCount; i < PlayerCount; i++)
        {
            GameObject newSlot = Instantiate(podiumSlotTemplate, podiumTransform);
            newSlot.name = $"Slot{i + 1}";
        }

        // Assign players to podium
        for (int i = 0; i < finalRanking.Count; i++)
        {
            var (time, controller) = finalRanking[i];

            var slot = podiumTransform.GetChild(i).gameObject;
            var scoreText = slot.transform.GetChild(0).GetComponent<TMP_Text>();

            string name = controller.gameObject.name;
            string timeDisplay = time == 0 ? "DNF" : $"{TimeSpan.FromSeconds(time):mm\\:ss\\:ff}";
            scoreText.text = $"{name}\n{timeDisplay}";
            scoreText.gameObject.SetActive(true);

            // Place the horse on the corresponding position
            if (i < LevelManager.Instance.EndGameHorsePos.Length)
            {
                var pos = LevelManager.Instance.EndGameHorsePos[i];
                controller.transform.SetPositionAndRotation(pos.position, pos.rotation);
                controller.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezePosition;
            }
        }
    }


    public void RestartGame()
    {
        TimeBeforeStart = 3;
        Time.timeScale = 1f; // Unfreeze time
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex,LoadSceneMode.Single); // Reload current scene
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

    public void GoToMenu()
    {
        RaceTimer.Instance?.ResetTimer();
        Time.timeScale = 1f;

        // Clean up game state before returning to menu
        Instance = null; // Important so new scene can assign it again

        Destroy(gameObject); // Remove this persistent GameManager

        SceneManager.LoadScene("TitleScreenScene");
    }


    private IEnumerator HandleGameEndSequence()
    {
        yield return new WaitForSeconds(5f);

        ShowPlayerPlacement();
        MusicManager.Instance?.PlayPostRaceTrack();

        // Try to find the EndGameCamera by name
        GameObject endGameCamObj = GameObject.Find("EndGameCamera");
        if (endGameCamObj != null)
        {
            endGameCamObj.SetActive(true);
        }
        else
        {
            Debug.LogWarning("EndGameCamera not found in scene!");
        }

        yield return new WaitForSeconds(10f);

        endGameCanvas.SetActive(true);
    }

    public void ReturnToMenuIfEndGameVisible()
    {
        if (endGameCanvas != null && endGameCanvas.activeInHierarchy)
        {
            GoToMenu();
        }
    }

}
