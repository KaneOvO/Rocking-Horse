using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Character;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class Lasso : MonoBehaviour
{
    public List<GameObject> TargetPlayers;
    public bool lassoReady;
    
    private float LassoDuration;
    private GameObject LassoOther;

    public void Start()
    { 
        foreach (var horse in HorseController.Horses.Where(Horse => Horse.gameObject != gameObject))
        {
            TargetPlayers.Add(horse.gameObject);
        }
    }

    private void Update()
    {
        if (LassoDuration > 0)
        {
            LassoDuration -= Time.deltaTime;
            
            var direction = (LassoOther.transform.position - transform.position).normalized;
            transform.position += direction * (1f * Time.deltaTime);
        }
    }

    public void UseLasso()
    {
        if (!lassoReady) return;
        
        GameObject bestTarget = null;

        foreach (var target in TargetPlayers)
        {
            var targetPos = target.transform.position;
            var direction = (targetPos - transform.position).normalized;
            var angle = Vector3.Angle(transform.forward, direction);

            if ((!(angle < 45f)) || (Vector3.Distance(transform.position, targetPos) > 20f)) continue;
            
            if (bestTarget == null) bestTarget = target;
            else
            {
                if (Vector3.Distance(transform.position, bestTarget.transform.position) >
                    Vector3.Distance(transform.position, targetPos))
                {
                    bestTarget = target;
                }
            }
        }

        if (bestTarget)
        {
            OnLassoHitTarget(bestTarget);
            bestTarget.GetComponent<Lasso>().OnHitByLasso(gameObject);
            
            lassoReady = false;
        }
        else Debug.Log("no target");
    }

    private void OnLassoHitTarget(GameObject lassoTaget)
    {
        LassoDuration = 2f;
        LassoOther = lassoTaget;
        
        GetComponent<HorseController>().OnLassoHitTarget();
    }

    public void OnHitByLasso(GameObject lassoSource)
    {
        LassoDuration = 2f;
        LassoOther = lassoSource;
        
        GetComponent<HorseController>().OnHitByLasso();
    }
}
