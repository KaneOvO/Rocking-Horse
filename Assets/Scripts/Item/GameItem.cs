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

        // Find and reverse the horseshoe spin animation directly
        if (TryGetComponent(out HorseController controller) && controller.horseUI != null)
        {
            Transform horseshoeTransform = controller.horseUI.transform.Find("ItemBackground/Horseshoe");

            if (horseshoeTransform != null)
            {
                Animator horseshoeAnimator = horseshoeTransform.GetComponent<Animator>();

                if (horseshoeAnimator != null)
                {
                    horseshoeAnimator.Rebind();
                    horseshoeAnimator.Update(0f);
                    horseshoeAnimator.Play("Spin", 0, 1.0f); // Start from the end
                    horseshoeAnimator.speed = -1f;           // Reverse playback
                }
            }
        }
    }



    protected void UpdateItemDisplayUI()
    {
        if (!gameObject.TryGetComponent(out HorseController controller))
        {
            return;
        }

        Transform itemTransform = controller.horseUI.transform.Find("ItemBackground/ItemImage");

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

        Transform itemTransform = controller.horseUI.transform.Find("ItemBackground/ItemImage");

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