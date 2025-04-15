
using GameSystem.Input;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace GameUI
{
    public class PanelController : MonoBehaviour
    {
        private int Index = 0;
        public UIController Controller;
        public Selectable[] UIS = new Selectable[0];

        private void OnEnable()
        {
            if(UIS.Length <= 0)
            {
                return;
            }

            Index = 0;
            UIS[0].Select();
        }

        public void Switch()
        {
            if (UIS.Length <= 0)
            {
                return;
            }

            Index++;
            Index = Index % UIS.Length;
            UIS[Index].Select();
        }

        public void Click()
        {
            if (UIS.Length <= 0)
            {
                return;
            }

            Selectable ui = UIS[Index];
            if(ui.GetType() == typeof(Button))
            {
                (ui as Button).onClick.Invoke();
            }
            else if (ui.GetType() == typeof(Toggle))
            {
                Toggle toggle = ui as Toggle;
                toggle.isOn = !toggle.isOn;
            }
        }

        public void Update()
        {
            Selectable ui = UIS[Index];
            if (ui.GetType() == typeof(Slider))
            {
                Slider slider = ui as Slider;
                slider.value += Controller.Direction * Mathf.Abs(slider.maxValue - slider.minValue) * Time.deltaTime * 0.5f;
            }
        }

    }

}
