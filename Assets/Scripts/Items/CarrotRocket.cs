using Character;
using NPC;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Splines;

namespace Items
{
    public class CarrotRocket : MonoBehaviour
    {
        public HorseController Controller;
        public float LastTime = 5;

        public float Speed = 20;
        public int Index = -1;

        public void Start()
        {
            //Controller.Collider.enabled = true;

            float cloestDistance = 99999999;

            for(int i = 0; i < NPCMap.Instance.Path.Count; i++)
            {
                float distance = Vector3.Distance(this.transform.position, NPCMap.Instance.Path[i].transform.position);
                if (distance < cloestDistance)
                {
                    cloestDistance = distance;
                    Index = i;
                }
            }
            
            GameObject.Destroy(this, LastTime);
        }

        public void OnDestroy()
        {
            //Controller.Collider.enabled= false;
        }

        public void LateUpdate()
        {
            Vector3 target = NPCMap.GetAt(Index).transform.position;

            if(Vector3.Distance(target, this.transform.position) < 1)
            {
                Index++;
            }

            Vector3 diff = (target - this.transform.position).normalized;
            Vector2 direction = new Vector2(diff.x, diff.z);

            Controller.HVelocity = direction * Speed;
        }
    }

}
