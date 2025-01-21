using Character;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace GameUI
{
    public class RocketBooster : MonoBehaviour
    {
        public HorseController Controller;
        public Image FilledImage;

        private void Start()
        {
            Update();
        }
        private void Update()
        {
            FilledImage.fillAmount = Controller.CurrentEnergy / Controller.MaxEnergy;
        }
    }

}
