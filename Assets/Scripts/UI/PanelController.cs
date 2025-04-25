
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

        private float sliderStepDelay = 0.2f;
        private float sliderTimer = 0f;

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
            if (UIS.Length <= 0 || Controller == null || Controller.Direction == 0)
                return;

            sliderTimer += Time.deltaTime;

            Selectable ui = UIS[Index];
            if (ui is Slider slider)
            {
                if (sliderTimer >= sliderStepDelay)
                {
                    float stepSize = 1f; // can be fractional if needed
                    float direction = Controller.Direction;

                    float newValue = slider.value + direction * stepSize;
                    newValue = Mathf.Clamp(newValue, slider.minValue, slider.maxValue);

                    slider.value = newValue;
                    sliderTimer = 0f;
                }
            }
            else
            {
                sliderTimer = 0f; // reset timer if not a slider
            }
        }

    }

}