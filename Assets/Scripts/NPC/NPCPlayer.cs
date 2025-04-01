using Character;
using GameSystem.Input;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NPC
{
    public class NPCPlayer : Controller
    {
        private int Index = 0;

        private Vector2 TargetPos;
        private float TargetSpeed;

        private float AccelerationInterval;
        private float RandomAcceleration;

        private float ReachRange;
        private float Rotation;

        private HorseController Horse;

        private void Awake()
        {
            if (!Enabled) return;
            CID = InputLayer.RegisterConatroller(false);
        }
        private void Start()
        {
            Horse = GetComponent<HorseController>();
            SetTarget(NPCMap.GetNext(ref Index));
        }
        private void SetTarget(PathPoint target)
        {
            ReachRange = target.ReachRadius;
            TargetPos = new Vector2(target.transform.position.z, target.transform.position.x);

            TargetSpeed = Horse.MaxSpeed * target.TargetSpeed;

            float randomAngle = Mathf.PI * Random.value * 2;
            Vector2 randomPos = new Vector2(Mathf.Cos(randomAngle), Mathf.Sin(randomAngle)) * Random.value * target.RandomRange;

            TargetPos += randomPos;
        }
        private void Update()
        {
            Vector2 thisPos = new Vector2(this.transform.position.z, this.transform.position.x);
            Vector2 diff = TargetPos - thisPos;

            // SpeedUp
            AccelerationInterval -= Time.deltaTime;
            if (AccelerationInterval < 0)
            {
                if(Mathf.Abs(Horse.CurrentSpeed - TargetSpeed) < 1f)
                {
                    RandomAcceleration = (0.35f + 0.65f * Random.value);
                }
                else if (Horse.CurrentSpeed < TargetSpeed)
                {
                    RandomAcceleration = (0.45f + 0.55f * Random.value);
                }
                else
                {
                    RandomAcceleration = (0.25f + 0.6f * Random.value);
                }
                AccelerationInterval = 0.35f;
            }
            InputLayer.UpdateAccelerate(CID, RandomAcceleration);

            // Rotation
            float thisArc = (this.transform.localEulerAngles.y) / 180 * Mathf.PI;
            Vector2 thisDir = new Vector2(Mathf.Cos(thisArc), Mathf.Sin(thisArc));

            float angleRotation = Vector2.SignedAngle(thisDir.normalized, diff.normalized);

            //while(angleRotation < -180)
            //{
            //    angleRotation += 360f;
            //}
            //while(angleRotation > 180)
            //{
            //    angleRotation -= 360f;
            //}

            //Debug.Log($"{thisPos}, {TargetPos}, {diff}");
            //Debug.Log($"{diff.magnitude} - {angleRotation} - {thisDir.normalized} - {diff.normalized}");

            angleRotation = Mathf.Clamp(angleRotation, -120, 120);
            if(Mathf.Abs(angleRotation) < 5)
            {
                angleRotation = 0;
            }
            else if(angleRotation < -5 && angleRotation > -15)
            {
                angleRotation = -15;
            }
            else if (angleRotation > 5 && angleRotation < 15)
            {
                angleRotation = 15;
            }

                InputLayer.UpdateRotation(CID, angleRotation);

            // Set Target
            if(diff.magnitude < ReachRange)
            {
                //Debug.Log("Reach");
                SetTarget(NPCMap.GetNext(ref Index));
            }
        }
    }

}
