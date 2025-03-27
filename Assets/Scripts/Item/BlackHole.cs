using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlackHole : MonoBehaviour
{
    public GameObject Dropper;
    public float Duration;
    public float PullStrength;
    
    public void Update()
    {
        Duration -= Time.deltaTime;

        if (Duration <= 0)
        {
            Destroy(gameObject);
        }
    }

    public void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (other.gameObject == Dropper)
                return;
            
            Vector3 direction = (transform.position - other.transform.position).normalized;
            
            other.transform.position += direction * (PullStrength * Time.deltaTime);
        }
    }
}
