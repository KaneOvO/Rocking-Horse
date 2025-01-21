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

    private void Awake()
    {
        PassTrigger.OnEnter += OnTriggered;
        HitTrigger.OnEnter += OnTriggered;
    }
    private void OnDestroy()
    {
        PassTrigger.OnEnter -= OnTriggered;
        HitTrigger.OnEnter -= OnTriggered;
    }
    private void OnTriggered(HorseController controller)
    {
        PassTrigger.enabled = false;
        HitTrigger.enabled = false;
    }

}
