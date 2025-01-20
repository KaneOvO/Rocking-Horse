using System.Collections;
using System.Collections.Generic;
using GameSystem.Input;
using UnityEngine;

public class Barrier : MonoBehaviour
{
    public float EnergyAddValue = 20f;
    private List<GameObject> Players = new List<GameObject>();

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !Players.Contains(other.gameObject))
        {
            Players.Add(other.gameObject);

            other.GetComponent<HorseController>().OnCrossingBarrier(EnergyAddValue);
        }
    }
}
