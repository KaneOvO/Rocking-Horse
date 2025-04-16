using Character;
using GameSystem.Input;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace GameUI
{
    public class MiniMap : MonoBehaviour
    {
        public float MapScale;
        public GameObject PlayerMark;

        public Vector2 Offset;

        private int Index = 0;
        [SerializeField]
        private List<Sprite> Sprites = new List<Sprite>();
        private List<RectTransform> Marks = new List<RectTransform>();

        private void Update()
        {
            while(Marks.Count < HorseController.Horses.Count)
            {
                GameObject newMark = GameObject.Instantiate(PlayerMark, this.transform);
                Image renderer = newMark.GetComponent<Image>();

                int colorIndex = (int)GameManager.Instance.Players[Index].GetComponent<HardwareController>().myListener.GetComponent<MyListener>().color;
                renderer.sprite = Sprites[Index * 4 + colorIndex];

                if (Index < Sprites.Count - 1)
                {
                    Index++;
                }

                newMark.SetActive(true);
                Marks.Add(newMark.transform as RectTransform);
            }
            for(int i = 0; i < Marks.Count; i++)
            {
                Vector2 pos = new Vector2(HorseController.Horses[i].transform.position.x, HorseController.Horses[i].transform.position.z) + Offset;
                Marks[i].anchoredPosition = pos * MapScale;
            }
        }
    }

}
