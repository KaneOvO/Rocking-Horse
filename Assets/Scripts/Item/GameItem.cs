using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using Character;

public class GameItem : MonoBehaviour
{
    public bool IsItemReady;
    public Sprite itemImage;
    public Sprite emptyImage;

    public virtual void OnReceiveItem()
    {
        IsItemReady = true;
        UpdateItemDisplayUI(itemImage);
        MusicManager.Instance?.mainTrackMusicSource?.PlayOneShot(MusicManager.Instance.pickItemAudio);
    }

    public virtual void OnUseItem()
    {
        if (!IsItemReady)
            return;

        IsItemReady = false;
        UpdateItemDisplayUI();

        if (TryGetComponent(out HorseController controller) && controller.horseUI != null)
        {
            Transform horseshoeTransform = controller.horseUI.transform.Find("ItemBackground/Horseshoe");

            if (horseshoeTransform != null)
            {
                Animator horseshoeAnimator = horseshoeTransform.GetComponent<Animator>();

                if (horseshoeAnimator != null)
                {
                    horseshoeAnimator.SetTrigger("ReverseSpin"); // This should match the transition trigger name
                }
            }
        }
    }


    protected void UpdateItemDisplayUI()
    {
        if (!TryGetComponent(out HorseController controller))
            return;

        if (controller.horseUI == null)
            return;

        Transform itemTransform = controller.horseUI.transform.Find("ItemBackground/ItemImage");
        if (itemTransform == null)
            return;

        if (!itemTransform.TryGetComponent(out Image image))
            return;

        image.sprite = emptyImage;
    }

    protected void UpdateItemDisplayUI(Sprite itemImage)
    {
        if (itemImage == null)
            return;

        if (!TryGetComponent(out HorseController controller))
            return;

        if (controller.horseUI == null)
            return;

        Transform itemTransform = controller.horseUI.transform.Find("ItemBackground/ItemImage");
        if (itemTransform == null)
            return;

        if (!itemTransform.TryGetComponent(out Image image))
            return;

        image.sprite = itemImage;
    }
}
