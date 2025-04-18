using Character;
using TMPro;
using UnityEngine;

namespace GameUI
{
    public class Ranking : MonoBehaviour
    {
        public TextMeshProUGUI RankingText;
        public int HorseIndex;

        [Header("Colors")]
        public Color GoldColor = new Color(1f, 0.84f, 0f);
        public Color SilverColor = new Color(0.75f, 0.75f, 0.75f);
        public Color BronzeColor = new Color(0.8f, 0.5f, 0.2f);
        public Color OtherColor = new Color(0.6f, 0.4f, 0.3f);

        private void Update()
        {
            if (HorseIndex >= HorseController.Horses.Count)
                return;

            int rank = HorseController.Horses[HorseIndex].Ranking + 1;

            RankingText.text = rank.ToString() + GetSuffix(rank);
            RankingText.color = GetColorByRank(rank);

        }

        private string GetSuffix(int number)
        {
            int lastTwoDigits = number % 100;
            int lastDigit = number % 10;

            if (lastTwoDigits >= 11 && lastTwoDigits <= 13)
                return "th";

            if (lastDigit == 1)
                return "st";
            else if (lastDigit == 2)
                return "nd";
            else if (lastDigit == 3)
                return "rd";
            else
                return "th";
        }

        private Color GetColorByRank(int rank)
        {
            if (rank == 1) return GoldColor;
            if (rank == 2) return SilverColor;
            if (rank == 3) return BronzeColor;
            return OtherColor;
        }
    }
}
