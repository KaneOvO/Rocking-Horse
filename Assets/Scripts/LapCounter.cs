using System;
using System.Collections;
using System.Collections.Generic;
using Character;
using NPC;
using UnityEngine;

public class LapCounter : MonoBehaviour
{
    private int lapFinished = -1;

    private bool isInsideFinishTrigger = false;

    private HorseController controller;

    private void Start()
    {
        controller = GetComponent<HorseController>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("FinishLine") && !isInsideFinishTrigger)
        {
            Debug.Log("Touched the Finish Line " + controller.CheckPointIndex);
            if (controller.CheckPointIndex % 56 != 2)
                return;
            
            isInsideFinishTrigger = true;
            
            lapFinished++;
            Debug.Log("Lap Finished: " + lapFinished);

            if (lapFinished == GameManager.Instance.lapCount)
            {
                OnPlayerFinishGame();
            }
        }
    }

    private void OnPlayerFinishGame()
    {
        GameManager.Instance.finalRanking.Add((RaceTimer.Instance.timer, GetComponent<HorseController>()));
        
        // freeze player control
        // show a finish visual
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("FinishLine"))
        {
            isInsideFinishTrigger = false;
        }
    }
}
