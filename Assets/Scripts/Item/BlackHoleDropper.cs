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

        if (Physics.Raycast(transform.position + Vector3.up, Vector3.down, out RaycastHit hit, 10f))
        {
            Vector3 spawnPosition = hit.point + Vector3.up * 0.05f;
            Quaternion spawnRotation = Quaternion.LookRotation(Vector3.forward, hit.normal); // Y轴对齐法线

            GameObject blackHole = Instantiate(BlackHolePrefab, spawnPosition, spawnRotation);
            //blackHole.GetComponent<BlackHole>().Dropper = gameObject;
        }
    }
}
