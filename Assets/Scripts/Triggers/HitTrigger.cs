using Character;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Triggers
{
    public class HitTrigger : Trigger
    {
        protected override void OnCharacterEnter(HorseController controller)
        {
            controller.OnHitBarrier();
        }
    }

}
