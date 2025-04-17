using GameSystem.Input;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace GameUI
{
    public class StartRaceController : MonoBehaviour
    {
        [Header("Events")]
        public UnityEvent OnAllHold;

        [Header("Controllers")]
        public UIController[] Controllers = new UIController[0];

        [Header("Circle UI Elements")]
        public Image[] HoldCircleBases = new Image[0];
        public Image[] HoldCircleFills = new Image[0]; 

        private bool IsHold = false;

        private void OnEnable()
        {
            IsHold = false;
            UpdateVisibleCircles();
        }

        private void Update()
        {
            if (IsHold)
                return;

            for (int i = 0; i < Controllers.Length; i++)
            {
                if (Controllers[i].IsConnected && !Controllers[i].IsHolding)
                {
                    UpdateVisibleCircles();
                    return;
                }
            }

            IsHold = true;
            UpdateVisibleCircles();
            OnAllHold?.Invoke();
        }

        private void UpdateVisibleCircles()
        {
            for (int i = 0; i < HoldCircleBases.Length; i++)
            {
                bool isConnected = i < Controllers.Length && Controllers[i].IsConnected;
                bool isHolding = isConnected && Controllers[i].IsHolding;

                HoldCircleBases[i].gameObject.SetActive(isConnected);
                HoldCircleFills[i].gameObject.SetActive(isConnected && isHolding);
            }
        }

    }
}
