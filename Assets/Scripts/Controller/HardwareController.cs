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
        private int Direction = 0;
        private int LastBoosterStatus = 0;
        private int LastJumpStatus = 0;
        private int LastChangeLeftStatus = 0;
        private int LastChangeRightStatus = 0;
        private float MinGyroscopeZ = 0;
        private float MaxGyroscopeZ = 0;

        private const float MistakeRange = 5;
        private List<RockRecord> RockRecords = new List<RockRecord>();

        private void Awake()
        {
            if (!Enabled) return;
            CID = InputLayer.RegisterConatroller(false);
            MyListener.OnSensorDataUpdated += OnMessageUpdate;
        }
        private void OnDestroy()
        {
            if (!Enabled) return;
            MyListener.OnSensorDataUpdated -= OnMessageUpdate;
        }

        private void OnMessageUpdate(SensorData data)
        {
            if (!Enabled) return;
            float amplifiedGyroscopeZ = data.gyroscopeZ * 2.0f;
            MinGyroscopeZ = Mathf.Min(MinGyroscopeZ, amplifiedGyroscopeZ);
            MaxGyroscopeZ = Mathf.Max(MaxGyroscopeZ, amplifiedGyroscopeZ);

            // MinGyroscopeZ = Mathf.Min(MinGyroscopeZ, data.gyroscopeZ);
            // MaxGyroscopeZ = Mathf.Max(MaxGyroscopeZ, data.gyroscopeZ);

            if (Direction == 0 && data.gyroscopeZ < MaxGyroscopeZ - MistakeRange)
            {
                //Debug.Log($"Reach Max# Min:{MinGyroscopeZ} - Max:{MaxGyroscopeZ} - Current:{data.gyroscopeZ}");

                float strength = Mathf.Abs(MaxGyroscopeZ - MinGyroscopeZ) / 40f;
                strength = Mathf.Clamp(strength, 0.0f, 1.0f);
                RockRecord record = new RockRecord();
                record.Strength = strength;
                record.Time = Time.realtimeSinceStartup;
                RockRecords.Add(record);
                MinGyroscopeZ = MaxGyroscopeZ;
                Direction = 1;
            }
            else if (Direction == 1 && data.gyroscopeZ > MinGyroscopeZ + MistakeRange)
            {
                //Debug.Log($"Reach Min# Min:{MinGyroscopeZ} - Max:{MaxGyroscopeZ} - Current:{data.gyroscopeZ}");

                float strength = Mathf.Abs(MaxGyroscopeZ - MinGyroscopeZ) / 40f;
                strength = Mathf.Clamp(strength, 0.0f, 1.0f);
                RockRecord record = new RockRecord();
                record.Strength = strength;
                record.Time = Time.realtimeSinceStartup;
                RockRecords.Add(record);
                MaxGyroscopeZ = MistakeRange;
                Direction = 0;
            }

            if (DebugMode)
            {
                Debug.Log($"Min:{MinGyroscopeZ} - Max:{MaxGyroscopeZ} - Current:{data.gyroscopeZ}");
            }

            float acceleration = 0;
            for (int i = RockRecords.Count - 1; i >= 0; i--)
            {
                acceleration += RockRecords[i].Strength;

                if (Time.realtimeSinceStartup - RockRecords[i].Time >= 0.5f)
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

            if (data.isJumped == 1 && LastJumpStatus == 0)
            {
                InputLayer.Jump(CID, 0.5f);
            }
            LastJumpStatus = data.isJumped;

            if (data.isChangedLeft == 1 && LastChangeLeftStatus == 0)
            {
                InputLayer.ChangeLane(CID, Vector2.left);
            }
            LastChangeLeftStatus = data.isChangedLeft;

            if (data.isChangedRight == 1 && LastChangeRightStatus == 0)
            {
                InputLayer.ChangeLane(CID, Vector2.right);
            }
            LastChangeRightStatus = data.isChangedRight;
        }
    }

}
