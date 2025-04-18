using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Character;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class Lasso : GameItem
{
    public GameObject LassoVisual;
    public List<GameObject> TargetPlayers;

    [SerializeField] private float LassoEffectTime;
    [SerializeField] private float LassoRangeAngle;
    [SerializeField] private float LassoRangeDistance;

    private float LassoDuration;
    private GameObject LassoOther;
    
    private LineRenderer lineRenderer;

    public void Start()
    {
        lineRenderer = GetComponent<LineRenderer>();
        lineRenderer.enabled = false;
        
        //GetComponent<PlayerItem>().GetItem<Lasso>();
        
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
            
            lineRenderer.SetPosition(0, transform.position);
            lineRenderer.SetPosition(1, LassoOther.transform.position);
            lineRenderer.enabled = true;
        }
        else
        {
            if (lineRenderer.enabled)
                lineRenderer.enabled = false;
        }
    }

    public override void OnReceiveItem()
    {
        base.OnReceiveItem();
    }

    public override void OnUseItem()
    {
        UseLasso();
    }

    public void UseLasso()
    {
        GameObject bestTarget = null;

        foreach (var target in TargetPlayers)
        {
            var targetPos = target.transform.position;
            var direction = (targetPos - transform.position).normalized;
            var angle = Vector3.Angle(transform.forward, direction);

            if ((!(angle < LassoRangeAngle)) || (Vector3.Distance(transform.position, targetPos) > LassoRangeDistance))
                continue;
            
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
            
            IsItemReady = false;
        }
        else Debug.Log("no target");
    }

    private void OnLassoHitTarget(GameObject lassoTarget)
    {
        LassoDuration += LassoEffectTime;
        LassoOther = lassoTarget;
        
        GetComponent<HorseController>().OnLassoHitTarget();
    }

    public void OnHitByLasso(GameObject lassoSource)
    {
        StartCoroutine(ShowLassoVisual());
        
        LassoDuration += LassoEffectTime;
        LassoOther = lassoSource;
        
        GetComponent<HorseController>().OnHitByLasso();
    }

    private IEnumerator ShowLassoVisual()
    {
        LassoVisual.SetActive(true);
        
        yield return new WaitForSeconds(LassoEffectTime);
        
        LassoVisual.SetActive(false);
    }
}
