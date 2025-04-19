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
        else if (lapCount == 2)
        {
            lap2.SetActive(false);
        }
    }
}
