using System.Collections;
using System.Collections.Generic;
using Character;
using GameSystem.Input;
using UnityEngine;

public class LapUI : MonoBehaviour
{
    public int horseIndex;
    public GameObject lap1;
    public GameObject lap2;
    public GameObject finished;

    private bool hasFinished = false;
    private static bool finalLapMusicStarted = false;

    private bool[] onSecondLap;

    private void Start()
    {
        finished.SetActive(false);
        onSecondLap = new bool[4] { false, false, false, false };
    }

    void Update()
    {
        if (horseIndex >= HorseController.Horses.Count)
            return;

        var controller = HorseController.Horses[horseIndex];
        var lapCount = controller.GetComponent<LapCounter>().lapFinished;

        if (lapCount == 1 && !finalLapMusicStarted)
        {
            lap1.SetActive(false);
            lap2.SetActive(true);

            MusicManager.Instance?.SwitchToFinalLapMusic();
            MusicManager.Instance?.mainTrackMusicSource?.PlayOneShot(MusicManager.Instance.lap1Audio);
            finalLapMusicStarted = true;
        }
        else if (lapCount == 2 && !hasFinished)
        {
            // Hide all children except Finish
            foreach (Transform child in transform)
            {
                if (child.name != "Finish" && !child.name.StartsWith("SharedUI"))
                {
                    child.gameObject.SetActive(false);
                }

            }


            // Show the Finish banner
            finished.SetActive(true);

            MusicManager.Instance?.mainTrackMusicSource?.PlayOneShot(MusicManager.Instance.lap2Audio);

            // Disable player movement
            var horseController = controller.GetComponent<HorseController>();
            horseController.DisableMovement();

            hasFinished = true;
        }
    }

    public void StartLap2(int index)
    {
        if (!finalLapMusicStarted)
        {
            MusicManager.Instance?.SwitchToFinalLapMusic();
            MusicManager.Instance?.mainTrackMusicSource?.PlayOneShot(MusicManager.Instance.lap1Audio);
            finalLapMusicStarted = true;
        }

        if (!onSecondLap[index])
        {
            onSecondLap[index] = true;
            lap1.SetActive(false);
            lap2.SetActive(true);
        }
        
    }

    public void FinishedRace()
    {
        // Show the Finish banner
        finished.SetActive(true);

        MusicManager.Instance?.mainTrackMusicSource?.PlayOneShot(MusicManager.Instance.lap2Audio);

        // Disable player movement
        var horseController = HorseController.Horses[horseIndex];
        horseController.DisableMovement();

        // Set finish time and status
        horseController.FinishTime = RaceTimer.Instance.timer;
        horseController.HasFinished = true;

        hasFinished = true;

        // Hide all children except Finish
        foreach (Transform child in transform)
        {
            if (child.name != "Finish" && !child.name.StartsWith("SharedUI"))
            {
                child.gameObject.SetActive(false);
            }
        }
    }



}
