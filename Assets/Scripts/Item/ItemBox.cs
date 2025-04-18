using System.Collections;
using System.Collections.Generic;
using Character;
using Items;
using Triggers;
using UnityEngine;

public class ItemBox : MonoBehaviour
{
    public enum ItemType
    {
        Lasso,
        BlackHoleDropper,
        CarrotRocket,
        Chicken
    }

    public ItemType ReceivedItemType;

    public Trigger GetItemTrigger;

    public float ItemBoxResponseTime;

    public float UIAnimationDuration = 2.5f;

    public Animator horseshoeAnimator;

    private Dictionary<int, List<(ItemType type, float weight)>> ItemWeights = new()
    {
        {
            0, new List<(ItemType, float)>
            {
                (ItemType.BlackHoleDropper, 100f)
                //(ItemType.CarrotRocket, 100f)
            }
        },
        {
            1, new List<(ItemType, float)>
            {
                
                /*(ItemType.Lasso, 40f),
                (ItemType.CarrotRocket, 10f),
                (ItemType.BlackHoleDropper, 10f),
                (ItemType.Chicken, 40f),*/
                

                (ItemType.Lasso, 100f)
            }
        },
        {
            2, new List<(ItemType, float)>
            {
                (ItemType.Lasso, 20f),
                (ItemType.CarrotRocket, 40f),
                (ItemType.BlackHoleDropper, 5f),
                (ItemType.Chicken, 35f),

                //(ItemType.Lasso, 100f)
            }
        },
        {
            3, new List<(ItemType, float)>
            {
                (ItemType.Lasso, 20f),
                (ItemType.CarrotRocket, 70f),
                (ItemType.BlackHoleDropper, 0f),
                (ItemType.Chicken, 10f),

                //(ItemType.CarrotRocket, 100f)
            }
        }
    };

    private void Awake()
    {
        GetItemTrigger.OnEnter += OnTriggered;
    }

    private void OnDestroy()
    {
        GetItemTrigger.OnEnter -= OnTriggered;
    }

    private void OnTriggered(HorseController controller)
    {
        // Only proceed if this controller has a UI (skip NPCs or unassigned players)
        if (controller.horseUI == null)
        {
            //Debug.LogWarning($"{controller.name} does not have a horseUI assigned. Skipping animation.");
            return;
        }
        
        StartCoroutine(ItemBoxRespawn());
        StartCoroutine(GiveItemAfterScrollAnimation(controller));
    }


    private IEnumerator GiveItemAfterScrollAnimation(HorseController controller)
    {
        // Scroll animation object
        GameObject scrollObject = controller.horseUI.transform.Find("ItemBackground/ItemImage/ItemScrollHorizontal")?.gameObject;
        Animator scrollAnimator = scrollObject?.GetComponent<Animator>();

        // Horseshoe animation object
        GameObject horseshoeObject = controller.horseUI.transform.Find("ItemBackground/Horseshoe")?.gameObject;
        Animator horseshoeAnimator = horseshoeObject?.GetComponent<Animator>();

        if (scrollObject != null)
            scrollObject.SetActive(true);

        if (horseshoeObject != null)
            horseshoeObject.SetActive(true);

        yield return null;

        if (scrollAnimator != null)
        {
            scrollAnimator.Rebind();
            scrollAnimator.Update(0f);
            scrollAnimator.SetTrigger("Scroll");
        }

        // Wait until 1 second is left in the Scroll animation
        yield return new WaitForSeconds(UIAnimationDuration - 3.0f);

        // Trigger the Horseshoe animation
        if (horseshoeAnimator != null)
        {
            horseshoeAnimator.Rebind();
            horseshoeAnimator.Update(0f);
            horseshoeAnimator.SetTrigger("Spin");
        }

        // Wait the final second
        yield return new WaitForSeconds(1.7f);

        // Hide scroll object
        if (scrollObject != null)
            scrollObject.SetActive(false);


        DetermineItem(controller);
        GetItem(controller);
    }

    private IEnumerator ItemBoxRespawn()
    {
        transform.GetChild(0).gameObject.SetActive(false);
        
        yield return new WaitForSeconds(ItemBoxResponseTime);
        
        transform.GetChild(0).gameObject.SetActive(true);
    }

    private void DetermineItem(HorseController controller)
    {
        var items = ItemWeights[controller.Ranking];

        float totalRange = 0;

        foreach(var item in items)
        {
            totalRange += item.weight;
        }

        var random = Random.Range(0f, totalRange);
        var cumulativeWeight = 0f;

        foreach (var item in items)
        {
            cumulativeWeight += item.weight;
            if (random <= cumulativeWeight)
            {
                ReceivedItemType = item.type;
                break;
            }
        }
    }

    private void GetItem(HorseController controller)
    {
        switch (ReceivedItemType)
        {
            case (ItemType.Lasso):
                controller.transform.GetComponent<PlayerItem>().GetItem<Lasso>();
                Debug.Log("Received Lasso");
                break;
            case (ItemType.BlackHoleDropper):
                controller.transform.GetComponent<PlayerItem>().GetItem<BlackHoleDropper>();
                Debug.Log("Received Black Hole Dropper");
                break;
            case (ItemType.CarrotRocket):
                controller.transform.GetComponent<PlayerItem>().GetItem<CarrotRocket>();
                Debug.Log("Received Carrot Rocket");
                break;
            case (ItemType.Chicken):
                controller.transform.GetComponent<PlayerItem>().GetItem<ChickenDropper>();
                Debug.Log("Received Chicken");
                break;
        }
    }

}
