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

        [SerializeField]
        private HorseController Horse;

        private void Start()
        {
            Horse = GetComponent<HorseController>();
            SetTarget(NPCMap.GetNext(ref Index));
        }
        private void SetTarget(PathPoint target)
        {
            TargetPos = new Vector2(target.transform.position.x, target.transform.position.z);

            TargetSpeed = Horse.MaxSpeed * target.TargetSpeed;

            float randomAngle = Mathf.PI * Random.value * 2;
            Vector2 randomPos = new Vector2(Mathf.Cos(randomAngle), Mathf.Sin(randomAngle)) * Random.value * target.RandomRange;

            TargetPos += randomPos;
        }
        private void Update()
        {
            Vector2 thisPos = new Vector2(this.transform.position.x, this.transform.position.z);
            Vector2 diff = TargetPos - thisPos;

            // SpeedUp
            if(AccelerationInterval < 0)
            {
                if(Mathf.Abs(Horse.CurrentSpeed - TargetSpeed) < 1f)
                {
                    RandomAcceleration = (-0.5f + Random.value) * Horse.Acceleration;
                }
                else if (Horse.CurrentSpeed < TargetSpeed)
                {
                    RandomAcceleration = (-0.25f + Random.value * 1.25f) * Horse.Acceleration;
                }
                else
                {
                    RandomAcceleration = (0.25f - Random.value) * Horse.Acceleration;
                }
                AccelerationInterval = 0.65f;
            }

            // Rotation
            float thisArc = this.transform.localEulerAngles.y / 180 * Mathf.PI;
            float targetArc = Mathf.Atan2(diff.y, diff.x);

            float angleRotation = (targetArc - thisArc) / Mathf.PI * 180;
            angleRotation = Mathf.Clamp(angleRotation, -90, 90);

            InputLayer.UpdateRotation(CID, angleRotation);

            // Use Booster
            if(Horse.CurrentEnergy > 100f)
            {
                InputLayer.UseBooster(CID);
            }

            // Set Target
            if(diff.magnitude < ReachRange)
            {
                SetTarget(NPCMap.GetNext(ref Index));
            }
        }
    }

}
