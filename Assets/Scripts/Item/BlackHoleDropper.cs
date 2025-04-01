using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlackHoleDropper : GameItem
{
    public GameObject BlackHolePrefab;

    public override void OnReceiveItem()
    {
        base.OnReceiveItem();
    }
    
    public override void OnUseItem()
    {
        base.OnUseItem();
        
        var blackHole = Instantiate(BlackHolePrefab, transform.position, Quaternion.identity);
        blackHole.GetComponent<BlackHole>().Dropper = gameObject;
    }
}
