using System.Collections;
using System.Collections.Generic;
using Character;
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
        GetItemTrigger.Enabled = false;
        ItemBoxRenderer.enabled = false;
        
        GetItem(controller);
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
                break;
            case (ItemType.Chicken):
                break;
        }
    }
    
}
