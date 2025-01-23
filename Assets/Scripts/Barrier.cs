using System.Collections;
using System.Collections.Generic;
using GameSystem.Input;
using Character;
using UnityEngine;
using Triggers;

public class Barrier : MonoBehaviour
{
    public Trigger PassTrigger;
    public Trigger HitTrigger;

    public MeshRenderer BarrierRenderer;

    private void Awake()
    {
        PassTrigger.OnEnter += OnTriggered;
        HitTrigger.OnEnter += OnHit;
        HitTrigger.OnEnter += OnTriggered;
    }
    private void OnDestroy()
    {
        PassTrigger.OnEnter -= OnTriggered;
        HitTrigger.OnEnter -= OnHit;
        HitTrigger.OnEnter -= OnTriggered;
    }
    private void OnTriggered(HorseController controller)
    {
        PassTrigger.Enabled = false;
        HitTrigger.Enabled = false;
    }
    private void OnHit(HorseController controller)
    {
        this.BarrierRenderer.enabled = false;
    }

}
