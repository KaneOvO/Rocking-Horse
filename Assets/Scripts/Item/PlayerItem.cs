using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class PlayerItem : MonoBehaviour
{
    public GameItem currItem;

    public void GetItem<T>() where T : GameItem
    {
        var item = GetComponent<T>();

        if (item == null)
            return;

        if (currItem != null)
            currItem.enabled = false;
        
        currItem = item;
        currItem.OnReceiveItem();
    }

    public void UseCurrItem()
    {
        if (currItem == null)
            return;

        currItem.OnUseItem();
        currItem = null;
    }
    
}
