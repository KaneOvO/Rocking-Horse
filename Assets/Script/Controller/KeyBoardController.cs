using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameSystem.Input
{
    public class KeyBoardController : MonoBehaviour
    {
        private struct KeyRecord
        {
            public KeyCode KeyCode;
            public float PressTime;
        }

        [SerializeField]
        private bool DebugMode = false;
        [SerializeField]
        private bool PrintEvent = false;

        private List<KeyRecord> Direction = new List<KeyRecord>();
        private List<KeyRecord> MoveRecord = new List<KeyRecord>();

        public int CID { get; private set; }

        private void Awake()
        {
            CID = InputLayer.RegisterConatroller(DebugMode);

            if (PrintEvent)
            {
                InputLayer.AddAccelerateEventListener(CID, (float value) => { Debug.Log( $"OnAccelerate: {value}"); });
                InputLayer.AddRotateEventListener(CID, (float value) => { Debug.Log($"OnRotate: {value}"); });
                InputLayer.AddJumpEventListener(CID, (float value) => { Debug.Log($"OnJump: {value}"); });
            }
        }
        private void Update()
        {
            float recordSpeed = 0;
            for(int i = MoveRecord.Count - 1; i >= 0; i--)
            {
                recordSpeed += 0.15f;
                if (Time.realtimeSinceStartup - MoveRecord[i].PressTime > 1f)
                {
                    MoveRecord.RemoveAt(i);
                }
            }
            recordSpeed = Mathf.Min(recordSpeed, 1);
            InputLayer.UpdateAccelerate(CID, recordSpeed);

            if (UnityEngine.Input.GetKeyDown(KeyCode.W))
            {
                KeyRecord record = new KeyRecord();
                record.KeyCode = KeyCode.W;
                record.PressTime = Time.realtimeSinceStartup;
                MoveRecord.Add(record);
            }
            if (UnityEngine.Input.GetKeyDown(KeyCode.A))
            {
                KeyRecord record = new KeyRecord();
                record.KeyCode = KeyCode.A;
                record.PressTime = Time.realtimeSinceStartup;
                Direction.Add(record);
            }
            else if (UnityEngine.Input.GetKeyUp(KeyCode.A))
            {
                for (int i = Direction.Count - 1; i >= 0; i--)
                {
                    if (Direction[i].KeyCode == KeyCode.A)
                    {
                        Direction.RemoveAt(i);
                    }
                }
            }
            if (UnityEngine.Input.GetKeyDown(KeyCode.D))
            {
                KeyRecord record = new KeyRecord();
                record.KeyCode = KeyCode.D;
                record.PressTime = Time.realtimeSinceStartup;
                Direction.Add(record);
            }
            else if (UnityEngine.Input.GetKeyUp(KeyCode.D))
            {
                for (int i = Direction.Count - 1; i >= 0; i--)
                {
                    if (Direction[i].KeyCode == KeyCode.D)
                    {
                        Direction.RemoveAt(i);
                    }
                }
            }
            if (Direction.Count == 0) InputLayer.UpdateRotation(CID, 0);
            else InputLayer.UpdateRotation(CID, Direction[^1].KeyCode == KeyCode.A ? -1 : 1);

            if (UnityEngine.Input.GetKeyDown(KeyCode.Space))
            {
                InputLayer.Jump(CID, 0.5f);
            }
        }
    }

}
