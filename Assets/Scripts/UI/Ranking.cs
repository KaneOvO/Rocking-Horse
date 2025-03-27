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

        private static float LastUpdateTime = 0;
        private const float UPDATE_INTERVAL = 0.1f;
        private static List<HorseController> RankingList;

        public void Update()
        {
            if(HorseController.Horses.Count < HorseIndex)
            {
                return;
            }

            if(LastUpdateTime <= Time.realtimeSinceStartup - UPDATE_INTERVAL)
            {
                LastUpdateTime = Time.realtimeSinceStartup;
                RankingList = new(HorseController.Horses);
                RankingList.Sort(SortHorse);
                RankingList.Reverse();
            }

            int index = RankingList.IndexOf(HorseController.Horses[HorseIndex]);
            RankingText.text = (index + 1).ToString();
        }

        private static int SortHorse(HorseController a, HorseController b)
        {
            if(a.CheckPointIndex == b.CheckPointIndex)
            {
                return a.NextCheckPointDistance.CompareTo(b.NextCheckPointDistance);
            }
            return a.CheckPointIndex.CompareTo(b.CheckPointIndex);
        }
    }

}
