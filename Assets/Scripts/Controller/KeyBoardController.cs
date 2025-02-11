using System.Collections;
using System.Collections.Generic;
using Character;
using UnityEngine;

namespace GameSystem.Input
{
    public class KeyBoardController : Controller
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

        [SerializeField]
        private KeyCode MoveKey = KeyCode.W;
        [SerializeField]
        private KeyCode LeftKey = KeyCode.A;
        [SerializeField]
        private KeyCode RightKey = KeyCode.D;
        [SerializeField]
        private KeyCode ChangeLeftKey = KeyCode.Q;
        [SerializeField]
        private KeyCode ChangeRightKey = KeyCode.E;
        [SerializeField]
        private KeyCode JumpKey = KeyCode.Space;
        [SerializeField]
        private KeyCode BoosterKey = KeyCode.LeftShift;

        private List<KeyRecord> Direction = new List<KeyRecord>();
        private List<KeyRecord> MoveRecord = new List<KeyRecord>();

        private void Awake()
        {
            if (!Enabled) return;
            CID = InputLayer.RegisterConatroller(DebugMode);

            if (PrintEvent)
            {
                InputLayer.AddAccelerateEventListener(CID, (float value) => { Debug.Log( $"OnAccelerate: {value}"); });
                InputLayer.AddRotateEventListener(CID, (float value) => { Debug.Log($"OnRotate: {value}"); });
                InputLayer.AddJumpEventListener(CID, (float value) => { Debug.Log($"OnJump: {value}"); });
                InputLayer.AddChangeLaneEventListener(CID, (Vector2 value) => { Debug.Log($"OnChangeLane: {value}"); });
                InputLayer.AddBoosterEventListener(CID, () => { Debug.Log($"OnUseBooster"); });
            }
        }
        private void Update()
        {
            if (!Enabled) return;
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
            //InputLayer.UpdateAccelerate(CID, recordSpeed);
            InputLayer.UpdateAccelerate(CID, UnityEngine.Input.GetKey(MoveKey) ? 0.85f : 0);

            if (UnityEngine.Input.GetKeyDown(MoveKey))
            {
                KeyRecord record = new KeyRecord();
                record.KeyCode = MoveKey;
                record.PressTime = Time.realtimeSinceStartup;
                MoveRecord.Add(record);
            }

            if (UnityEngine.Input.GetKeyDown(LeftKey))
            {
                KeyRecord record = new KeyRecord();
                record.KeyCode = LeftKey;
                record.PressTime = Time.realtimeSinceStartup;
                Direction.Add(record);
            }
            else if (UnityEngine.Input.GetKeyUp(LeftKey))
            {
                for (int i = Direction.Count - 1; i >= 0; i--)
                {
                    if (Direction[i].KeyCode == LeftKey)
                    {
                        Direction.RemoveAt(i);
                    }
                }
            }
            if (UnityEngine.Input.GetKeyDown(RightKey))
            {
                KeyRecord record = new KeyRecord();
                record.KeyCode = RightKey;
                record.PressTime = Time.realtimeSinceStartup;
                Direction.Add(record);
            }
            else if (UnityEngine.Input.GetKeyUp(RightKey))
            {
                for (int i = Direction.Count - 1; i >= 0; i--)
                {
                    if (Direction[i].KeyCode == RightKey)
                    {
                        Direction.RemoveAt(i);
                    }
                }
            }
            if (Direction.Count == 0) InputLayer.UpdateRotation(CID, 0);
            else InputLayer.UpdateRotation(CID, Direction[^1].KeyCode == LeftKey ? -60 : 60);

            if (UnityEngine.Input.GetKeyDown(JumpKey))
            {
                InputLayer.Jump(CID, 0.5f);
            }
            if (UnityEngine.Input.GetKeyDown(ChangeLeftKey))
            {
                InputLayer.ChangeLane(CID, Vector2.left);
                
                GetComponent<Lasso>().UseLasso();
            }
            if (UnityEngine.Input.GetKeyDown(ChangeRightKey))
            {
                InputLayer.ChangeLane(CID, Vector2.right);
            }
            if (UnityEngine.Input.GetKeyDown(BoosterKey))
            {
                InputLayer.UseBooster(CID);
            }
        }

        public void UpdateController()
        {
            if(GameManager.Instance.Players.Count == 1) return;

            if (GameManager.Instance.Players[1] == gameObject)
            {
                MoveKey = KeyCode.UpArrow;
                LeftKey = KeyCode.LeftArrow;
                RightKey = KeyCode.RightArrow;
                ChangeLeftKey = KeyCode.Alpha1;
                ChangeRightKey = KeyCode.Alpha2;
                JumpKey = KeyCode.KeypadEnter;
                BoosterKey = KeyCode.KeypadPlus;
                return;
            }

            if (GameManager.Instance.Players[2] == gameObject)
            {
                MoveKey = KeyCode.I;
                LeftKey = KeyCode.J;
                RightKey = KeyCode.L;
                ChangeLeftKey = KeyCode.U;
                ChangeRightKey = KeyCode.O;
                JumpKey = KeyCode.K;
                BoosterKey = KeyCode.P;
                return;
            }

            if (GameManager.Instance.Players[3] == gameObject)
            {
                MoveKey = KeyCode.T;
                LeftKey = KeyCode.F;
                RightKey = KeyCode.H;
                ChangeLeftKey = KeyCode.G;
                ChangeRightKey = KeyCode.J;
                JumpKey = KeyCode.Y;
                BoosterKey = KeyCode.U;
                return;
            }
            
            
        }
    }

    

}
