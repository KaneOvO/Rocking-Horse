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
        //字典存储颜色是否被使用
        private Dictionary<PlayerColor, bool> PlayerColors = new Dictionary<PlayerColor, bool>()
        {
            { PlayerColor.Red, false },
            { PlayerColor.yellow, false },
            { PlayerColor.blue, false },
            { PlayerColor.green, false }
        };
        private void Update()
        {
            while (Marks.Count < HorseController.Horses.Count)
            {
                GameObject newMark = GameObject.Instantiate(PlayerMark, this.transform);
                Image renderer = newMark.GetComponent<Image>();

                if (HorseController.Horses[Index].GetComponent<HorseController>().isPlayerHorse)
                {
                    int colorIndex = (int)GameManager.Instance.Players[Index].GetComponent<HardwareController>().myListener.GetComponent<MyListener>().color;
                    renderer.sprite = Sprites[Index * 4 + colorIndex];
                    
                    PlayerColors[(PlayerColor)colorIndex] = true;
                }
                else
                {
                    // NPC Horse Color
                    foreach (var color in PlayerColors.Keys)
                    {
                        if (!PlayerColors[color])
                        {
                            renderer.sprite = Sprites[Index * 4 + (int)color];
                            PlayerColors[color] = true;
                            HorseController.Horses[Index].GetComponentInChildren<BandanaColor>().SetColor((int)color);
                            break;
                        }
                    }
                }



                if (Index < Sprites.Count - 1)
                {
                    Index++;
                }

                newMark.SetActive(true);
                Marks.Add(newMark.transform as RectTransform);
            }
            for (int i = 0; i < Marks.Count; i++)
            {
                Vector2 pos = new Vector2(HorseController.Horses[i].transform.position.x, HorseController.Horses[i].transform.position.z) + Offset;
                Marks[i].anchoredPosition = pos * MapScale;
            }
        }
    }

}
