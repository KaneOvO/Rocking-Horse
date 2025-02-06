using System.Collections;
using System.Collections.Generic;
using GameUI;
using UnityEngine;

public class LevelManager: MonoBehaviour
{
    public static LevelManager Instance { get; private set; }
    public GameObject[] SpawnPoints;
    public List<GameObject> UI = new List<GameObject>();

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        SpawnPlayer();
    }

    // Update is called once per frame
    void Update()
    {

    }

    void SpawnPlayer()
    {
        for (int i = 0; i < GameManager.Instance.PlayerCount; i++)
        {
            GameObject player = Instantiate(GameManager.Instance.PlayerPrefab);
            player.name = "Player" + (i + 1);
            player.transform.position = SpawnPoints[i].transform.position;
            player.transform.rotation = SpawnPoints[i].transform.rotation;
            GameManager.Instance.Players.Add(player);
            player.GetComponent<GameSystem.Input.KeyBoardController>().UpdateController();
            //todo: Load the corresponding model according to the player's choice
        }

        CameraManager.Instance.SetCamera();
        SetUI();
    }

    void SetUI()
    {
        for(int i = 0; i < GameManager.Instance.PlayerCount; i++)
        {
            UI[i].SetActive(true);
            UI[i].GetComponent<Canvas>().worldCamera = CameraManager.Instance.UICameras[i].GetComponent<Camera>();
            UI[i].GetComponentInChildren<RocketBooster>().Controller = GameManager.Instance.Players[i].GetComponent<Character.HorseController>();
        }
    }
    
}
