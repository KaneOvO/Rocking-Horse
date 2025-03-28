using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Character;
using UnityEngine.Rendering.Universal;

namespace Triggers
{
    public class GetItemTrigger : Trigger
    {
        protected override void OnCharacterEnter(HorseController controller)
        {
            Debug.Log("pick up item");
            
            
        }
    }
}


