using System;
using System.Collections;
using System.Collections.Generic;
using Character;
using NPC;
using UnityEngine;

public class LapCounter : MonoBehaviour
{
    public GameObject ScoreBoard;
    
    public int lapFinished = -1;

    private bool isInsideFinishTrigger = false;

    private HorseController controller;

    private void Start()
    {
        controller = GetComponent<HorseController>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (gameObject.TryGetComponent(out NPCPlayer _))
            return;
        
        if (other.CompareTag("FinishLine") && !isInsideFinishTrigger)
        {
            if (controller.CheckPointIndex % 56 != 2)
                return;
            
            isInsideFinishTrigger = true;
            
            lapFinished++;
            Debug.Log(lapFinished);

            if (lapFinished == GameManager.Instance.lapCount)
            {
                OnPlayerFinishGame();
            }
        }
    }

    private void OnPlayerFinishGame()
    {
        GameManager.Instance.finalRanking.Add((RaceTimer.Instance.timer, GetComponent<HorseController>())); 
        // show a finish visual
        Debug.Log("finish game");
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("FinishLine"))
        {
            isInsideFinishTrigger = false;
        }
    }
}
