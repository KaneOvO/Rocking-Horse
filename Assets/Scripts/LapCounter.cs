using System;
using System.Collections;
using System.Collections.Generic;
using Character;
using NPC;
using UnityEngine;

public class LapCounter : MonoBehaviour
{
    private int lapFinished = 0;

    private bool isInsideFinishTrigger = false;

    public event Action OnRaceFinished;
    
    private HorseController controller;

    private void Start()
    {
        controller = GetComponent<HorseController>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("FinishLine") && !isInsideFinishTrigger)
        {
            var pathPoint = NPCMap.GetAt(controller.CheckPointIndex);

            if (!pathPoint.isLastPoint)
                return;
            
            isInsideFinishTrigger = true;

            lapFinished++;
            Debug.Log("Lap Finished: " + lapFinished);

            if (lapFinished >= GameManager.Instance.lapCount)
            {
                GameManager.Instance.finalRanking.Add((RaceTimer.Instance.timer, GetComponent<HorseController>()));
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("FinishLine"))
        {
            isInsideFinishTrigger = false;
        }
    }
}
