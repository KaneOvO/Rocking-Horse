using System.Collections;
using System.Collections.Generic;
using Character;
using GameSystem.Input;
using UnityEngine;

public class BoostArea : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (!other.TryGetComponent<HorseController>(out var controller))
        {
            return;
        }
        controller.UseBooster();
    }
}
