using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[DefaultExecutionOrder(-2)]
public class CameraManager : MonoBehaviour
{
    public static CameraManager Instance { get; private set; }

    public List<GameObject> MainCameras = new List<GameObject>();
    public List<GameObject> UICameras = new List<GameObject>();

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

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void GetAllCamras()
    {
        for (int i = 0; i < GameManager.Instance.PlayerCount; i++)
        {
            GameObject player = GameManager.Instance.Players[i];
            MainCameras.Add(player.transform.Find("Main Camera").gameObject);
            UICameras.Add(player.transform.Find("UI Camera").gameObject);
        }

    }

    void OnePlayerMode()
    {
        MainCameras[0].GetComponent<Camera>().rect = new Rect(0, 0, 1, 1);
        UICameras[0].GetComponent<Camera>().rect = new Rect(0, 0, 1, 1);
    }

    void TwoPlayerMode()
    {
        MainCameras[0].GetComponent<Camera>().rect = new Rect(0, 0.5f, 1, 0.5f);
        MainCameras[1].GetComponent<Camera>().rect = new Rect(0, 0, 1, 0.5f);

        UICameras[0].GetComponent<Camera>().rect = new Rect(0, 0.5f, 1, 0.5f);
        UICameras[1].GetComponent<Camera>().rect = new Rect(0, 0, 1, 0.5f);
    }

    void FourPlayerMode()
    {
        MainCameras[0].GetComponent<Camera>().rect = new Rect(0, 0.5f, 0.5f, 0.5f);
        MainCameras[1].GetComponent<Camera>().rect = new Rect(0.5f, 0.5f, 0.5f, 0.5f);
        MainCameras[2].GetComponent<Camera>().rect = new Rect(0, 0, 0.5f, 0.5f);
        MainCameras[3].GetComponent<Camera>().rect = new Rect(0.5f, 0, 0.5f, 0.5f);

        UICameras[0].GetComponent<Camera>().rect = new Rect(0, 0.5f, 0.5f, 0.5f);
        UICameras[1].GetComponent<Camera>().rect = new Rect(0.5f, 0.5f, 0.5f, 0.5f);
        UICameras[2].GetComponent<Camera>().rect = new Rect(0, 0, 0.5f, 0.5f);
        UICameras[3].GetComponent<Camera>().rect = new Rect(0.5f, 0, 0.5f, 0.5f);
    }

    public void SetCamera()
    {
        GetAllCamras();
        switch (GameManager.Instance.PlayerCount)
        {
            case 1:
                OnePlayerMode();
                break;
            case 2:
                TwoPlayerMode();
                break;
            case 4:
                FourPlayerMode();
                break;
            default:
                break;
        }
    }
    
}
