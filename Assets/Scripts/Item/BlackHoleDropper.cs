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

        /*if (Physics.Raycast(transform.position + Vector3.up, Vector3.down, out RaycastHit hit, 10f))
        {
            Vector3 spawnPosition = hit.point + Vector3.up * 0.05f;

            GameObject blackHole = Instantiate(BlackHolePrefab, spawnPosition, transform.rotation);
            blackHole.GetComponent<BlackHole>().Dropper = gameObject;
        }*/
        
        var blackHole = Instantiate(BlackHolePrefab, transform.position + Vector3.down * 0.25f, transform.rotation);
        blackHole.GetComponent<BlackHole>().Dropper = gameObject;
    }
}
