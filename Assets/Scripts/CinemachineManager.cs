using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CinemachineManager : MonoBehaviour
{
    [Header("Audio")]
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip introAudioClip;
    [SerializeField] private float introAudioPitch = 1.0f;
    [SerializeField] private AudioClip endSFX;
    [SerializeField] private AudioSource sfxSource;
    private bool hasPlayedEndSFX = false;

    [System.Serializable]
    private struct CineMachinePairing { public CinemachineVirtualCamera camera; public CinemachineDollyCart dolly; }


    [Header("Components")]

    [SerializeField]
    private CineMachinePairing[] pairings;

    [SerializeField]
    private Canvas uiCanvas;

    private bool bIntroInProgress;

    [Header("Debug")]

    [SerializeField]
    private bool bSkipIntro;

    private int currentCamera;

    // Start is called before the first frame update
    void Start()
    {

        foreach (var pairing in pairings)
        {
            pairing.camera.enabled = false;
            pairing.dolly.enabled = false;
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
            if(pairings[currentCamera].dolly.m_Position >= pairings[currentCamera].dolly.m_Path.PathLength)
            {

                pairings[currentCamera].camera.enabled = false;

                currentCamera++;

                if (currentCamera >= pairings.Length)
                {

                    IntroComplete();
                }
                else
                {
                    StartCamera(currentCamera);
                }
            }
            else if (pairings[currentCamera].dolly.m_Position == pairings[currentCamera].dolly.m_Path.PathLength - 4)
            {
                pairings[currentCamera].camera.GetComponent<Animator>().SetBool(01, true);
            }
        }
    }

    private void StartIntro()
    {
        currentCamera = 0;

        StartCamera(currentCamera);

        bIntroInProgress = true;

        if (audioSource != null && introAudioClip != null)
        {
            audioSource.pitch = introAudioPitch;
            audioSource.clip = introAudioClip;
            audioSource.Play();

            StartCoroutine(PlayEndSFXAfterDelay(audioSource.clip.length / audioSource.pitch - 0.5f));
        }
    }


    private void StartCamera(int index)
    {
        pairings[currentCamera].camera.enabled = true;

        pairings[currentCamera].dolly.enabled =true;

        pairings[currentCamera].camera.GetComponent<Animator>().enabled = true;
    }

    private void IntroComplete()
    {
        bIntroInProgress = false;

        uiCanvas.enabled = false;

        FindObjectOfType<LevelManager>().SpawnPlayer();

        FindObjectOfType<GameUI.Timer>()?.BeginCountdown();
    }

    private IEnumerator PlayEndSFXAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);

        if (sfxSource != null && endSFX != null)
        {
            sfxSource.PlayOneShot(endSFX);
        }
    }

}
