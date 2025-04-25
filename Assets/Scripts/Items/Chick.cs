using Character;
using GameSystem.Input;
using NPC;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.VFX;

namespace Items
{
    public class Chick : MonoBehaviour
    {
        public HorseController Source;

        public float Height = 1;
        public float LastTime = 30;

        public float Speed = 30;
        public float SeekRange = 5;

        private HorseController Target;
        private Vector2 Direction;
        private int Index = -1;

        private const float ORIENTATION = 260;

        private float CurrentTime;

        [SerializeField]
        private VisualEffect Chicken;

        [SerializeField]
        private VisualEffect Start01;
        [SerializeField]
        private VisualEffect Start02;

        [SerializeField]
        private VisualEffect End01;
        [SerializeField]
        private VisualEffect End02;

        [SerializeField]
        private VisualEffect Impact;

        private void Start()
        {
            float cloestDistance = 99999999;

            for (int i = 0; i < NPCMap.Instance.Path.Count; i++)
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

            if (Vector2.Angle(cpPos - currentPos, npPos - currentPos) > 90)
            {
                Index++;
                Direction = (npPos - currentPos).normalized;
            }
            else
            {
                Direction = (cpPos - currentPos).normalized;
            }

            Start01.transform.SetParent(null);
            Start02.transform.SetParent(null);

            Start01.enabled = true;
            Start02.enabled = true;

            GameObject.Destroy(Start01.gameObject, 8);
            GameObject.Destroy(Start02.gameObject, 8);

            CurrentTime = 0;
        }

        private void Update()
        {
            CurrentTime += Time.deltaTime;
            if(CurrentTime > LastTime)
            {
                Chicken.enabled = false;

                End01.transform.SetParent(null);
                End02.transform.SetParent(null);

                End01.enabled = true;
                End02.enabled = true;

                GameObject.Destroy(End01.gameObject, 8);
                GameObject.Destroy(End02.gameObject, 8);

                GameObject.Destroy(this.gameObject);
                return;
            }

            if (Target == null)
            {
                foreach(HorseController controller in HorseController.Horses)
                {
                    if(controller == Source)
                    {
                        continue;
                    }
                    float distance = Vector3.Distance(this.transform.position, controller.transform.position);
                    distance += Mathf.Abs(this.transform.position.y - controller.transform.position.y) * 2;
                    if(distance < SeekRange)
                    {
                        Target = controller;
                        break;
                    }
                }
            }

            Vector3 target = Target == null ? NPCMap.GetAt(Index).transform.position : Target.transform.position;

            if (Vector3.Distance(target, this.transform.position) < 2)
            {
                Index++;
            }

            if(Target != null && Vector3.Distance(target, this.transform.position) < 1)
            {
                //Target.OnHitBarrier();
                Target.OnHitChick();
                //TODO: animation on hit
                
                CurrentTime = LastTime + 1;

                Impact.transform.SetParent(null);

                Impact.enabled = true;

                GameObject.Destroy(Impact.gameObject, 8);
                return;
            }

            Vector3 diff = (target - this.transform.position).normalized;

            Vector2 direction = new Vector2(diff.x, diff.z);
            float targetAngle = Mathf.Atan2(direction.y, direction.x) / Mathf.PI * 180;
            float currentAngle = Mathf.Atan2(Direction.y, Direction.x) / Mathf.PI * 180;

            float angleDiff = targetAngle - currentAngle;
            while (angleDiff > 180)
            {
                angleDiff -= 360;
            }
            while (angleDiff < -180)
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

            this.transform.localEulerAngles = new Vector3(0, -currentAngle + 90, 0);

            float currentArc = currentAngle / 180 * Mathf.PI;
            Direction = new Vector2(Mathf.Cos(currentArc), Mathf.Sin(currentArc));

            Vector2 movement = Direction * Speed * Time.deltaTime;
            Debug.Log(movement);
            float posY = this.transform.position.y;

            if(Physics.Raycast(this.transform.position, Vector3.down, out RaycastHit hit, Height + 2f, LayerMask.GetMask("Ground")))
            {
                posY = hit.point.y + Height * 0.5f;
            }
            this.transform.position = new Vector3(transform.position.x + movement.x,
                posY, transform.position.z + movement.y);
        }
    }

}
