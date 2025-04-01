using Character;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace GameUI
{
    public class Ranking : MonoBehaviour
    {
        public TextMeshProUGUI RankingText;
        public int HorseIndex;

        public void Update()
        {
            if(HorseController.Horses.Count < HorseIndex)
            {
                return;
            }

            RankingText.text = HorseController.Horses[HorseIndex].Ranking.ToString();
        }
    }

}
