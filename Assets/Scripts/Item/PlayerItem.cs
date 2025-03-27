using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class PlayerItem : MonoBehaviour
{
    public GameItem currItem;

    public void ReceiveItem(GameItem item)
    {
        if (currItem != null)
            return;
        
        // got assigned based on current standing

        currItem = item;
    }

    public void UseItem()
    {
        if (currItem == null)
            return;
        
        currItem.UseItem();
    }
}
