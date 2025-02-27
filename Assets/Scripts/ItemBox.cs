using System.Collections;
using System.Collections.Generic;
using Character;
using Triggers;
using Unity.VisualScripting;
using UnityEngine;

public class ItemBox : MonoBehaviour
{
    public Trigger GetItemTrigger;
    
    public MeshRenderer ItemBoxRenderer;

    private void Awake()
    {
        GetItemTrigger.OnEnter += OnTriggered;
    }

    private void OnDestroy()
    {
        GetItemTrigger.OnEnter -= OnTriggered;
    }

    private void OnTriggered(HorseController controller)
    {
        GetItemTrigger.Enabled = false;
        ItemBoxRenderer.enabled = false;
        
        controller.GetComponent<Lasso>().lassoReady = true;
    }
}
