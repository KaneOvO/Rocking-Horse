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

        private float MinGyroscopeZ = 0;
        private float MaxGyroscopeZ = 0;

        private const float MistakeRange = 5;
        private List<RockRecord> RockRecords = new List<RockRecord>();

        private void Awake()
        {
            CID = InputLayer.RegisterConatroller(false);
            MyListener.OnSensorDataUpdated += OnMessageUpdate;
        }
        private void OnDestroy()
        {
            MyListener.OnSensorDataUpdated -= OnMessageUpdate;
        }

        private void OnMessageUpdate(SensorData data)
        {
            MinGyroscopeZ = Mathf.Min(MinGyroscopeZ, data.gyroscopeZ);
            MaxGyroscopeZ = Mathf.Max(MaxGyroscopeZ, data.gyroscopeZ);

            if (data.gyroscopeZ < MaxGyroscopeZ - MistakeRange)
            {
                float strength = Mathf.Abs(MaxGyroscopeZ - MinGyroscopeZ) / 40f;
                strength = Mathf.Clamp(strength, 0.0f, 1.0f);
                RockRecord record = new RockRecord();
                record.Strength = strength;
                record.Time = Time.realtimeSinceStartup;
                RockRecords.Add(record);
                MinGyroscopeZ = MaxGyroscopeZ;
            }
            else if(data.gyroscopeZ > MinGyroscopeZ + MistakeRange)
            {
                float strength = Mathf.Abs(MaxGyroscopeZ - MinGyroscopeZ) / 40f;
                strength = Mathf.Clamp(strength, 0.0f, 1.0f);
                RockRecord record = new RockRecord();
                record.Strength = strength;
                record.Time = Time.realtimeSinceStartup;
                RockRecords.Add(record);
                MaxGyroscopeZ = MistakeRange;
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
        }
    }

}
