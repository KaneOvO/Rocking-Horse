using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NPC
{
    public class NPCMap : MonoBehaviour
    {
        public static NPCMap Instance { get; private set; }

        public List<PathPoint> Path = new List<PathPoint> ();

        private void Awake()
        {
            Instance = this;
        }

        public static PathPoint GetNext(ref int index)
        {
            if(index > Instance.Path.Count)
            {
                index = 0;
            }
            index++;
            return Instance.Path[index];
        }


    }

}
