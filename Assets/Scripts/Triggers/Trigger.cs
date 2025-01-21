using Character;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Triggers
{
    public class Trigger : MonoBehaviour
    {
        [HideInInspector]
        public bool Enabled = true;

        public System.Action<HorseController> OnEnter;
        public System.Action<HorseController> OnExit;

        private void OnTriggerEnter(Collider other)
        {
            if(Enabled && other.TryGetComponent<HorseController>(out HorseController controller))
            {
                OnCharacterEnter(controller);
                OnEnter?.Invoke(controller);
            }
        }
        private void OnTriggerExit(Collider other)
        {
            if (Enabled && other.TryGetComponent<HorseController>(out HorseController controller))
            {
                OnCharacterExit(controller);
                OnExit?.Invoke(controller);
            }
        }
        protected virtual void OnCharacterEnter(HorseController controller) { }
        protected virtual void OnCharacterExit(HorseController controller) { }
    }

}
