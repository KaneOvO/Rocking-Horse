using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlackHoleDropper : GameItem
{
    public GameObject BlackHolePrefab;

    public override void OnReceiveItem()
    {
        UpdateItemDisplayUI();

        IsItemReady = true;
    }
    
    public override void OnUseItem()
    {
        var blackHole = Instantiate(BlackHolePrefab, transform.position, Quaternion.identity);
        blackHole.GetComponent<BlackHole>().Dropper = gameObject;
        
        IsItemReady = false;
    }
}
