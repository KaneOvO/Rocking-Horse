using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlackHoleDropper : GameItem
{
    public GameObject BlackHolePrefab;

    public override void UseItem()
    {
        Instantiate(BlackHolePrefab);
    }
}
