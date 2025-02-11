using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Character;
using Triggers;
using Unity.VisualScripting;

public class Manure : MonoBehaviour
{
    public Trigger SlipTrigger;
    
    public MeshRenderer ManureRenderer;

    private void Awake()
    {
        SlipTrigger.OnEnter += OnTriggered;
    }

    private void OnDestroy()
    {
        SlipTrigger.OnEnter -= OnTriggered;
    }
    
    private void OnTriggered(HorseController controller)
    {
        SlipTrigger.Enabled = false;
        ManureRenderer.enabled = false;
    }
}
