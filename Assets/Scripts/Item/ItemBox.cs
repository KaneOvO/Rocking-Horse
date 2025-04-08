using System.Collections;
using System.Collections.Generic;
using Character;
using Items;
using Triggers;
using Unity.VisualScripting;
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
    
    public MeshRenderer ItemBoxRenderer;

    public float ItemBoxResponseTime;

    private Dictionary<int, List<(ItemType type, float weight)>> ItemWeights = new()
    {
        {
            0, new List<(ItemType, float)>
            {
                (ItemType.BlackHoleDropper, 100f)
            }
        },
        {
            1, new List<(ItemType, float)>
            {
                (ItemType.Lasso, 40f),
                (ItemType.CarrotRocket, 10f),
                (ItemType.BlackHoleDropper, 10f),
                (ItemType.Chicken, 40f),
            }
        },
        {
            2, new List<(ItemType, float)>
            {
                (ItemType.Lasso, 20f),
                (ItemType.CarrotRocket, 40f),
                (ItemType.BlackHoleDropper, 5f),
                (ItemType.Chicken, 35f),
            }
        },
        {
            3, new List<(ItemType, float)>
            {
                (ItemType.Lasso, 20f),
                (ItemType.CarrotRocket, 70f),
                (ItemType.BlackHoleDropper, 0f),
                (ItemType.Chicken, 10f),
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
        DetermineItem(controller);
        GetItem(controller);

        StartCoroutine(ItemBoxRespawn());
    }

    private IEnumerator ItemBoxRespawn()
    {
        GetItemTrigger.Enabled = false;
        ItemBoxRenderer.enabled = false;
        
        yield return new WaitForSeconds(ItemBoxResponseTime);
        
        GetItemTrigger.Enabled = true;
        ItemBoxRenderer.enabled = true;
    }

    private void DetermineItem(HorseController controller)
    {
        var items = ItemWeights[controller.Ranking];
        Debug.Log($"Ranking: {controller.Ranking}");
        
        var random = Random.Range(0f, 1f);
        var cumulativeWeight = 0f;

        foreach (var item in  items)
        {
            cumulativeWeight += item.weight;
            if (random <= cumulativeWeight)
                ReceivedItemType = item.type;
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
