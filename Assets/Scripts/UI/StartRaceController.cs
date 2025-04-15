using GameSystem.Input;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace GameUI
{
    public class StartRaceController : MonoBehaviour
    {
        public UnityEvent OnAllHold;

        public UIController[] Controllers = new UIController[0];

        private bool IsHold = false;

        private void OnEnable()
        {
            IsHold = false;
        }

        private void Update()
        {
            if (IsHold)
            {
                return;
            }
            foreach (var controller in Controllers)
            {
                if (controller.IsConnected && !controller.IsHolding)
                {
                    //Debug.Log($"return at:{controller}");
                    return;
                }
            }
            IsHold = true;
            OnAllHold?.Invoke();
        }
    }

}
