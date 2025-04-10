using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using Character;
using GameSystem.Input;
using UnityEngine;
using UnityEngine.UI;

public class GameItem : MonoBehaviour
{
    public bool IsItemReady;
    public Sprite itemImage;
    public Sprite emptyImage;


    public virtual void OnReceiveItem()
    {
        IsItemReady = true;
        UpdateItemDisplayUI(itemImage);
    }

    public virtual void OnUseItem()
    {
        if (!IsItemReady)
            return;

        IsItemReady = false;
        UpdateItemDisplayUI();
    }

    protected void UpdateItemDisplayUI()
    {
        if (!gameObject.TryGetComponent(out HorseController controller))
        {
            return;
        }

        Transform itemTransform = controller.horseUI.transform.Find("ItemImage");

        if (itemTransform == null)
        {
            return;
        }

        GameObject itemUI = itemTransform.gameObject;
        if (!itemUI.TryGetComponent(out UnityEngine.UI.Image image))
        {
            return;
        }
        
        image.sprite = emptyImage;
    }

    protected void UpdateItemDisplayUI(Sprite itemImage)
    {
        //Debug.Log("UpdateItemDisplayUI");

        if (itemImage == null)
        {
            return;
        }

        if (!gameObject.TryGetComponent(out HorseController controller))
        {
            return;
        }

        Transform itemTransform = controller.horseUI.transform.Find("ItemImage");

        if (itemTransform == null)
        {
            return;
        }

        GameObject itemUI = itemTransform.gameObject;
        if (!itemUI.TryGetComponent(out UnityEngine.UI.Image image))
        {
            return;
        }
        
        image.sprite = itemImage;
    }

}
