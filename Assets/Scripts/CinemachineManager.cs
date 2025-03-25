using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CinemachineManager : MonoBehaviour
{

    [SerializeField]
    private CinemachineVirtualCamera[] cCameras;

    [SerializeField]
    private CinemachineDollyCart[] dollyCarts;

    [SerializeField]
    private float[] dollyDistances;

    private bool bIntroInProgress;

    [SerializeField]
    private bool bSkipIntro;

    private int currentCamera;

    // Start is called before the first frame update
    void Start()
    {
        foreach (var cam in cCameras)
        {
            cam.enabled = false;
        }

        foreach (var dolly in dollyCarts)
        {
            dolly.enabled = false;
        }

        if(bSkipIntro)
        {
            IntroComplete();
        }
        else
        {
            StartIntro();   
        }

    }

    void Update()
    {
        if(bIntroInProgress)
        {
            if(dollyCarts[currentCamera].m_Position >= dollyDistances[currentCamera])
            {

                cCameras[currentCamera].enabled = false;

                currentCamera++;

                if (currentCamera >= cCameras.Length)
                {

                    IntroComplete();
                }
                else
                {
                    StartCamera(currentCamera);
                }
            }
            else if (dollyCarts[currentCamera].m_Position == dollyDistances[currentCamera] - 4)
            {
                cCameras[currentCamera].GetComponent<Animator>().SetBool(01, true);
            }
        }
    }

    private void StartIntro()
    {
        currentCamera = 0;
        
        StartCamera(currentCamera);

        bIntroInProgress = true;

    }

    private void StartCamera(int index)
    {
        cCameras[currentCamera].enabled = true;

        dollyCarts[currentCamera].enabled =true;

        cCameras[currentCamera].GetComponent<Animator>().enabled = true;
    }

    private void IntroComplete()
    {
        bIntroInProgress = false;

        FindObjectOfType<LevelManager>().SpawnPlayer();
    }
    
}
