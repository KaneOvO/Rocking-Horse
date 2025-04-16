using System.Collections;
using System.Collections.Generic;
using GameSystem.Input;
using GameUI;
using UnityEngine;

[DefaultExecutionOrder(-1)]
public class LevelManager : MonoBehaviour
{
    public static LevelManager Instance { get; private set; }
    public GameObject[] SpawnPoints;
    public List<GameObject> UI = new List<GameObject>();
    public List<MyListener> Listeners = new List<MyListener>();
    public GameObject MiniMapPrefab;

    public GameObject Podium;
    public Transform EndGameCameraPos;
    public Transform[] EndGameHorsePos;

    [SerializeField]
    public bool skipCameraIntro;

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

        if (skipCameraIntro)
        {
            SpawnPlayer();

            GameManager.Instance.StartGame();
        }

        if (MultiSerialManager.Instance != null)
        {
            for (int i = 0; i < MultiSerialManager.Instance.listeners.Length; i++)
            {
                Listeners.Add(MultiSerialManager.Instance.listeners[i].GetComponent<MyListener>());
            }
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void SpawnPlayer()
    {
        for (int i = 0; i < GameManager.Instance.PlayerCount; i++)
        {
            GameObject player = Instantiate(GameManager.Instance.PlayerPrefab);
            player.name = "Player" + (i + 1);
            player.transform.position = SpawnPoints[i].transform.position;
            player.transform.rotation = SpawnPoints[i].transform.rotation;
            GameManager.Instance.Players.Add(player);
            player.GetComponent<GameSystem.Input.KeyBoardController>().UpdateController();
            player.GetComponent<HardwareController>().myListener = Listeners[i];
            player.GetComponentInChildren<BandanaColor>().SetColor((int)Listeners[i].GetComponent<MyListener>().color);
            //todo: Load the corresponding model according to the player's choice
        }

        for (int i = GameManager.Instance.PlayerCount; i < 4; i++)
        {
            GameObject NPC = Instantiate(GameManager.Instance.NPCPrefab);
            NPC.name = "NPC" + (i + 1);
            NPC.transform.position = SpawnPoints[i].transform.position;
            NPC.transform.rotation = SpawnPoints[i].transform.rotation;
        }


        CameraManager.Instance.SetCamera();
        SetUI();
    }

    void SetUI()
    {
        List<RectTransform> sharedUIs = new List<RectTransform>();
        for (int i = 0; i < GameManager.Instance.PlayerCount; i++)
        {
            UI[i].SetActive(true);

            Camera camera = CameraManager.Instance.UICameras[i].GetComponent<Camera>();

            UI[i].GetComponent<Canvas>().worldCamera = camera;

            GameObject miniMap = Instantiate(MiniMapPrefab);
            miniMap.transform.SetParent(UI[i].transform);

            RectTransform rect = miniMap.transform as RectTransform;
            rect.localScale = Vector3.one;
            rect.localPosition = Vector3.zero;
            //rect.rotation = Quaternion.identity;
            rect.localEulerAngles = Vector3.zero;

            sharedUIs.Add(rect);

            //UI[i].GetComponentInChildren<RocketBooster>().Controller = GameManager.Instance.Players[i].GetComponent<Character.HorseController>();
            GameManager.Instance.Players[i].GetComponent<Character.HorseController>().horseUI = UI[i];
        }

        switch (GameManager.Instance.PlayerCount)
        {
            case 1:
                sharedUIs[0].sizeDelta = Vector2.one * 400;

                sharedUIs[0].anchorMin = Vector2.one;
                sharedUIs[0].anchorMax = Vector2.one;
                sharedUIs[0].anchoredPosition = -new Vector2(sharedUIs[0].rect.width, sharedUIs[0].rect.height) / 2;
                break;
            case 2:
                sharedUIs[0].sizeDelta = Vector2.one * 512;
                sharedUIs[1].sizeDelta = Vector2.one * 512;

                sharedUIs[0].anchorMin = Vector2.right / 2;
                sharedUIs[0].anchorMax = Vector2.right / 2;
                sharedUIs[0].anchoredPosition = Vector2.zero;
                sharedUIs[1].anchorMin = new Vector2(0.5f, 1);
                sharedUIs[1].anchorMax = new Vector2(0.5f, 1);
                sharedUIs[1].anchoredPosition = Vector2.zero;
                break;
            case 3:
                sharedUIs[0].sizeDelta = Vector2.one * 1024;
                sharedUIs[1].sizeDelta = Vector2.one * 1024;
                sharedUIs[2].sizeDelta = Vector2.one * 512;

                sharedUIs[0].anchorMin = Vector2.right;
                sharedUIs[0].anchorMax = Vector2.right;
                sharedUIs[0].anchoredPosition = Vector2.zero;

                sharedUIs[1].anchorMin = Vector2.zero;
                sharedUIs[1].anchorMax = Vector2.zero;
                sharedUIs[1].anchoredPosition = Vector2.zero;

                sharedUIs[2].anchorMin = new Vector2(0.5f, 1);
                sharedUIs[2].anchorMax = new Vector2(0.5f, 1);
                sharedUIs[2].anchoredPosition = Vector2.zero;
                break;
            case 4:
                sharedUIs[0].sizeDelta = Vector2.one * 1024;
                sharedUIs[1].sizeDelta = Vector2.one * 1024;
                sharedUIs[2].sizeDelta = Vector2.one * 1024;
                sharedUIs[3].sizeDelta = Vector2.one * 1024;

                sharedUIs[0].anchorMin = Vector2.right;
                sharedUIs[0].anchorMax = Vector2.right;
                sharedUIs[0].anchoredPosition = Vector2.zero;

                sharedUIs[1].anchorMin = Vector2.zero;
                sharedUIs[1].anchorMax = Vector2.zero;
                sharedUIs[1].anchoredPosition = Vector2.zero;

                sharedUIs[2].anchorMin = Vector2.one;
                sharedUIs[2].anchorMax = Vector2.one;
                sharedUIs[2].anchoredPosition = Vector2.zero;

                sharedUIs[3].anchorMin = Vector2.up;
                sharedUIs[3].anchorMax = Vector2.up;
                sharedUIs[3].anchoredPosition = Vector2.zero;
                break;
        }
    }

}
