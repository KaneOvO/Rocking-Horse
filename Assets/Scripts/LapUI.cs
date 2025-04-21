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

    private void Start()
    {
        finished.SetActive(false);
    }

    void Update()
    {
        if (horseIndex >= HorseController.Horses.Count)
            return;

        var controller = HorseController.Horses[horseIndex];
        var lapCount = controller.GetComponent<LapCounter>().lapFinished;

        if (lapCount == 1)
        {
            lap1.SetActive(false);
            lap2.SetActive(true);
        }
        else if (lapCount == 2 && !hasFinished)
        {
            // Hide all children except Finish
            foreach (Transform child in transform)
            {
                if (child.name != "Finish")
                {
                    child.gameObject.SetActive(false);
                }
            }

            // Show the Finish banner
            finished.SetActive(true);

            // Disable player movement
            var horseController = controller.GetComponent<HorseController>();
            horseController.DisableMovement();

            hasFinished = true;
        }
    }
}
