using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameItem : MonoBehaviour
{
    public bool IsItemReady;
    
    public virtual void OnReceiveItem()
    {
        IsItemReady = true;
    }
    
    public virtual void OnUseItem()
    {
        IsItemReady = false;
    }

    protected void UpdateItemDisplayUI()
    {
        
    }
}
