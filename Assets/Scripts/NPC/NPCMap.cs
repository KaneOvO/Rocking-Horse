using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Splines;

namespace NPC
{
    public class NPCMap : MonoBehaviour
    {
        public static NPCMap Instance { get; private set; }
        public static SplineContainer Spline => Instance.m_Spline;

        public List<PathPoint> Path = new List<PathPoint> ();
        public SplineContainer m_Spline;

        private void Awake()
        {
            Instance = this;
        }

        private void Start()
        {
            Path[Path.Count - 1].isLastPoint = true;
        }

        public static PathPoint GetAt(int index)
        {
            if(Instance.Path.Count <= 2)
            {
                return null;
            }

            return Instance.Path[index % Instance.Path.Count];
        }
        public static PathPoint GetNext(ref int index)
        {
            if(index >= Instance.Path.Count)
            {
                index = 0;
            }
            PathPoint p = Instance.Path[index];
            index++;
            return p;
        }


    }

}
