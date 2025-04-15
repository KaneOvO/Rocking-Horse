using System;
using System.Collections;
using System.Collections.Generic;
using Character;
using UnityEngine;

public class BlackHole : MonoBehaviour
{
    public GameObject Dropper;
    public GameObject Target;
    public float Duration;
    public float PullStrength;
    public float TriggeredDuration;
    
    private bool hasTriggered = false;
    
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
            
            if (!Target)
                Target = other.gameObject;
            
            Vector3 direction = (transform.position - Target.transform.position).normalized;
            
            Target.transform.position += direction * (PullStrength * Time.deltaTime);

            if (Vector3.Distance(transform.position, Target.transform.position) < 1f && hasTriggered == false)
            {
                hasTriggered = true;
                
                //todo: freeze movement
                if(other.TryGetComponent(out HorseController controller))
                {
                    controller.OnHitBlackHole();
                }

                StartCoroutine(SelfDestroy());
            }
        }
    }

    

    private IEnumerator SelfDestroy()
    {
        yield return new WaitForSeconds(TriggeredDuration);
        
        Destroy(gameObject);
    }
}
