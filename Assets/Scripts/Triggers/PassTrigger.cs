using Character;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Triggers
{
    public class PassTrigger : Trigger
    {
        public float EnergyIncreasement = 50;
        protected override void OnCharacterEnter(HorseController controller)
        {
            controller.OnCrossingBarrier(EnergyIncreasement);
        }
    }

}
