using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[DefaultExecutionOrder(-2)]
public class CameraManager : MonoBehaviour
{
    public static CameraManager Instance { get; private set; }
    public GameObject mainCamera;

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

        // if(mainCamera != null)
        //     mainCamera.GetComponent<Camera>().enabled = false;
        
    }

    void TwoPlayerMode()
    {
        MainCameras[0].GetComponent<Camera>().rect = new Rect(0, 0.5f, 1, 0.5f);
        MainCameras[1].GetComponent<Camera>().rect = new Rect(0, 0, 1, 0.5f);

        UICameras[0].GetComponent<Camera>().rect = new Rect(0, 0.5f, 1, 0.5f);
        UICameras[1].GetComponent<Camera>().rect = new Rect(0, 0, 1, 0.5f);

        // if(mainCamera != null)
        //     mainCamera.GetComponent<Camera>().enabled = false;
    }

    void ThreePlayerMode()
    {
        MainCameras[0].GetComponent<Camera>().rect = new Rect(0, 0.5f, 0.5f, 0.5f);
        MainCameras[1].GetComponent<Camera>().rect = new Rect(0.5f, 0.5f, 0.5f, 0.5f);
        MainCameras[2].GetComponent<Camera>().rect = new Rect(0, 0, 1f, 0.5f);

        UICameras[0].GetComponent<Camera>().rect = new Rect(0, 0.5f, 0.5f, 0.5f);
        UICameras[1].GetComponent<Camera>().rect = new Rect(0.5f, 0.5f, 0.5f, 0.5f);
        UICameras[2].GetComponent<Camera>().rect = new Rect(0, 0, 1f, 0.5f);

        //mainCamera.GetComponent<Camera>().rect = new Rect(0, 0, 0, 0);
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

        // if(mainCamera != null)
        //     mainCamera.GetComponent<Camera>().enabled = false;
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
            case 3:
                ThreePlayerMode();
                break;
            case 4:
                FourPlayerMode();
                break;
            default:
                break;
        }
    }

    public void DisableAllPlayerCamera()
    {
        foreach (var cam in MainCameras)
        {
            cam.GetComponent<Camera>().enabled = false;
        }

        foreach (var cam in UICameras)
        {
            cam.GetComponent<Camera>().enabled = false;
        }
    }
}
