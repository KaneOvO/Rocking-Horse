using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Character;

namespace Triggers
{
    public class SlipTrigger : Trigger
    {
        protected override void OnCharacterEnter(HorseController controller)
        {
            // trigger something in controller
            controller.OnHitManure();
            Debug.Log("Manure Enter");
        }
    }
}

