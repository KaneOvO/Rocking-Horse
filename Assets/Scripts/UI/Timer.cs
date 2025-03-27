using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace GameUI
{
    public class Timer : MonoBehaviour
    {
        public TextMeshProUGUI Text;
        public void Update()
        {
            if(GameManager.TimeBeforeStart > 0)
            {
                Text.text = Mathf.CeilToInt(GameManager.TimeBeforeStart).ToString();
            }
            else
            {
                Text.enabled = false;
            }
        }
    }

}
