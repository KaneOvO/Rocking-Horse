using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameSystem.Input
{
    public class HardwareController : Controller
    {
        private struct RockRecord
        {
            public float Strength;
            public float Time;
        }

        public bool DebugMode = false;
        public MyListener myListener;
        //private int Direction = 0;
        private int LastBoosterStatus = 0;
        //private int LastJumpStatus = 0;
        //private float MinGyroscopeX = 0;
        //private float MaxGyroscopeX = 0;
        private float LastGyroscopeX = 0;

        private const float MistakeRange = 1f;
        private const float RotationDeadRange = 12.5f;
        private List<RockRecord> RockRecords = new List<RockRecord>();

        private void Awake()
        {
            if (!Enabled) return;
            CID = InputLayer.RegisterConatroller(false);
        }
        private void Start()
        {
            myListener.OnSensorDataUpdated += OnMessageUpdate;
        }
        private void OnDestroy()
        {
            if (!Enabled) return;
            myListener.OnSensorDataUpdated -= OnMessageUpdate;
        }

        private void OnMessageUpdate(SensorData data)
        {
            if (!Enabled) return;

            //MinGyroscopeX = Mathf.Min(MinGyroscopeX, data.gyroscopeX);
            //MaxGyroscopeX = Mathf.Max(MaxGyroscopeX, data.gyroscopeX);

            /*if (Direction == 0 && data.gyroscopeX < MaxGyroscopeX - MistakeRange)
            {
                float strength = Mathf.Abs(MaxGyroscopeX - MinGyroscopeX) / 3.5f;
                strength = Mathf.Clamp(strength, 0.0f, 1.0f);

                RockRecord record = new RockRecord();
                record.Strength = strength;
                record.Time = Time.realtimeSinceStartup;
                RockRecords.Add(record);
                MinGyroscopeX = MaxGyroscopeX;
                Direction = 1;
            }
            else if (Direction == 1 && data.gyroscopeX > MinGyroscopeX + MistakeRange)
            {
                float strength = Mathf.Abs(MaxGyroscopeX - MinGyroscopeX) / 3.5f;
                strength = Mathf.Clamp(strength, 0.0f, 1.0f);

                RockRecord record = new RockRecord();
                record.Strength = strength;
                record.Time = Time.realtimeSinceStartup;
                RockRecords.Add(record);
                MaxGyroscopeX = MinGyroscopeX;
                Direction = 0;
            }*/
            
            if(Mathf.Abs(LastGyroscopeX - data.gyroscopeX) > MistakeRange)
            {
                float strength = Mathf.Abs(LastGyroscopeX - data.gyroscopeX) / 1.5f;
                strength = Mathf.Clamp(strength, 0.0f, 1.0f);

                RockRecord record = new RockRecord();
                record.Strength = strength;
                record.Time = Time.realtimeSinceStartup;
                RockRecords.Add(record);
                LastGyroscopeX = data.gyroscopeX;
            }

            //if (DebugMode)
            //{
            //    Debug.Log($"Min:{MinGyroscopeX} - Max:{MaxGyroscopeX} - Current:{data.gyroscopeZ} - Rotation:{data.rotationZ}");
            //}

            float acceleration = 0;
            for (int i = RockRecords.Count - 1; i >= 0; i--)
            {
                acceleration += RockRecords[i].Strength;

                if (Time.realtimeSinceStartup - RockRecords[i].Time >= 0.25f)
                {
                    RockRecords.RemoveAt(i);
                }
            }

            acceleration = Mathf.Clamp(acceleration, 0.0f, 1.0f);
            InputLayer.UpdateAccelerate(CID, acceleration);

            if (data.isBoosted == 1 && LastBoosterStatus == 0)
            {
                InputLayer.UseBooster(CID);
            }
            LastBoosterStatus = data.isBoosted;

            //if (data.isJumped == 1 && LastJumpStatus == 0)
            //{
            //    InputLayer.Jump(CID, 0.5f);
            //}
            //LastJumpStatus = data.isJumped;

            //float rotationSensitivity = 10f;
            //float rotation = (data.rotationZ < 0 ? -1 : 1) * Mathf.Pow(Mathf.Abs(data.rotationZ), 1.5f) * rotationSensitivity;
            //if (Mathf.Abs(rotation) < RotationDeadRange)
            //{
            //    InputLayer.UpdateRotation(CID, 0);
            //}
            //else
            //{
            //    InputLayer.UpdateRotation(CID, rotation);
            //}

            float minRotationSpeed = 45f; 
            float maxRotationSpeed = 75f;  
            float rotationSpeed = Mathf.Lerp(minRotationSpeed, maxRotationSpeed, acceleration);

            if (data.rotationZ == 0)
            {
                InputLayer.UpdateRotation(CID, 0);
            }
            else
            {
                float rotation = data.rotationZ * rotationSpeed;
                InputLayer.UpdateRotation(CID, rotation);
            }


            //if (data.isChangedLeft == 1 && LastChangeLeftStatus == 0)
            //{
            //    InputLayer.ChangeLane(CID, Vector2.left);
            //}
            //LastChangeLeftStatus = data.isChangedLeft;

            //if (data.isChangedRight == 1 && LastChangeRightStatus == 0)
            //{
            //    InputLayer.ChangeLane(CID, Vector2.right);
            //}
            //LastChangeRightStatus = data.isChangedRight;
        }
    }

}
