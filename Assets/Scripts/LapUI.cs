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

    private void Start()
    {
        finished.SetActive(false);
        finalLapMusicStarted = false; 
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
            MusicManager.Instance?.PlayLap1Audio();

            finalLapMusicStarted = true;
        }
        else if (lapCount == 2 && !hasFinished)
        {
            FinishLap(controller);
        }
    }

    public void StartLap2(int index)
    {
        if (!finalLapMusicStarted)
        {
            MusicManager.Instance?.SwitchToFinalLapMusic();
            MusicManager.Instance?.PlayLap1Audio();

            finalLapMusicStarted = true;
        }

        lap1.SetActive(false);
        lap2.SetActive(true);
    }

    public void FinishedRace()
    {
        if (hasFinished) return;

        var horseController = HorseController.Horses[horseIndex];
        FinishLap(horseController);
    }

    private void FinishLap(HorseController horseController)
    {
        hasFinished = true;

        horseController.DisableMovement();
        horseController.FinishTime = RaceTimer.Instance.timer;
        horseController.HasFinished = true;

        finished.SetActive(true);

        MusicManager.Instance?.PlayLap2Audio();

        foreach (Transform child in transform)
        {
            if (child.name != "Finish" && !child.name.StartsWith("SharedUI"))
            {
                child.gameObject.SetActive(false);
            }
        }
    }
}
