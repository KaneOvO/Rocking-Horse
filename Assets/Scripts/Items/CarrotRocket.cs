using Character;
using NPC;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Items
{
    public class CarrotRocket : GameItem
    {
        public HorseController Controller;
        public float LastTime = 7;

        public float Speed = 20;

        private float CurrentTime = 999;
        private int Index = -1;
        private Vector2 Direction;
        private const float ORIENTATION = 200;

        [Header("Models")]
        [SerializeField]
        private GameObject horseObject;

        [SerializeField]
        private GameObject carrotObject;

        private void Start()
        {
            Controller = GetComponent<HorseController>();
        }

        public override void OnUseItem()
        {
            //Controller.Collider.enabled = true;

            float cloestDistance = 99999999;

            horseObject.SetActive(false);
            carrotObject.SetActive(true);

            for(int i = 0; i < NPCMap.Instance.Path.Count; i++)
            {
                float distance = Vector3.Distance(this.transform.position, NPCMap.Instance.Path[i].transform.position);
                if (distance < cloestDistance)
                {
                    cloestDistance = distance;
                    Index = i;
                }
            }

            Vector2 currentPos = new Vector2(this.transform.position.x, this.transform.position.z);
            Vector3 cpPos3 = NPCMap.GetAt(Index).transform.position;
            Vector3 npPos3 = NPCMap.GetAt(Index + 1).transform.position;

            Vector2 cpPos = new Vector2(cpPos3.x, cpPos3.z);
            Vector2 npPos = new Vector2(npPos3.x, npPos3.z);

            if(Vector2.Angle(cpPos - currentPos, npPos - currentPos) > 90)
            {
                Index++;
                Direction = (npPos - currentPos).normalized;
            }
            else
            {
                Direction = (cpPos - currentPos).normalized;
            }

            CurrentTime = 0;
        }

        public void LateUpdate()
        {
            CurrentTime += Time.deltaTime;
            if(CurrentTime > LastTime)
            {
                horseObject.SetActive(true);
                carrotObject.SetActive(false);

                return;
            }

            Vector3 target = NPCMap.GetAt(Index).transform.position;

            if(Vector3.Distance(target, this.transform.position) < 2)
            {
                Index++;
            }

            Vector3 diff = (target - this.transform.position).normalized;

            Vector2 direction = new Vector2(diff.x, diff.z);
            float targetAngle = Mathf.Atan2(direction.y, direction.x) / Mathf.PI * 180;
            float currentAngle = Mathf.Atan2(Direction.y, Direction.x) / Mathf.PI * 180;

            float angleDiff = targetAngle - currentAngle;
            while(angleDiff > 180)
            {
                angleDiff -= 360;
            }
            while(angleDiff < -180)
            {
                angleDiff += 360;
            }

            float deltaAngle = ORIENTATION * Time.deltaTime;
            if (Mathf.Abs(angleDiff) < deltaAngle)
            {
                currentAngle = targetAngle;
            }
            else if (angleDiff > 0)
            {
                currentAngle += deltaAngle;
            }
            else
            {
                currentAngle -= deltaAngle;
            }


            float currentArc = currentAngle / 180 * Mathf.PI;
            Direction = new Vector2(Mathf.Cos(currentArc), Mathf.Sin(currentArc));

            Controller.HVelocity = Direction * Speed;
            Controller.Direction = Direction;
        }
    }

}
