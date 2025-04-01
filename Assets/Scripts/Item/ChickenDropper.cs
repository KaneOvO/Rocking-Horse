using System.Collections;
using System.Collections.Generic;
using Character;
using Items;
using UnityEngine;

public class ChickenDropper : GameItem
{
    public GameObject ChickenPrefab;

    public override void OnReceiveItem()
    {
        base.OnReceiveItem();
    }

    public override void OnUseItem()
    {
        base.OnUseItem();
        
        var chicken = Instantiate(ChickenPrefab, transform.position, Quaternion.identity);
        chicken.GetComponent<Chick>().Source = GetComponent<HorseController>();
    }
}
