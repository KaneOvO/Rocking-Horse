using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Windows;


namespace GameSystem.Input
{
    public class UIController : MonoBehaviour
    {
        public int ListenerIndex;

        public UnityEvent OnHitButton;
        public UnityEvent OnHoldButton;

        public float HoldTriggerTime = 1.25f;

        public bool KeyboardDebug = false;
        public KeyCode DebugKey = KeyCode.Space;

        private MyListener Listener;
        private float HoldTime = 0;
        private bool IsTriggered = false;
        private bool IsPressed = false;

        private float WaitTime = 0;

        public bool IsHolding {  get; private set; }
        public bool IsConnected => (Listener != null && Listener.isConnected) || KeyboardDebug;
        public int Direction {  get; private set; }

        private const float WAIT_SWITCH_TIME = 0.75f;

        private void Awake()
        {
            Listener = MultiSerialManager.Instance.listeners[ListenerIndex].GetComponent<MyListener>();
            Listener.OnSensorDataUpdated += OnMessageUpdate;
        }

        private void OnEnable()
        {
            WaitTime = Time.realtimeSinceStartup;
        }

        private void OnDestroy()
        {
            Listener.OnSensorDataUpdated -= OnMessageUpdate;
        }
        private void Update()
        {
            if (KeyboardDebug)
            {
                SensorData data = new()
                {
                    isBoosted = UnityEngine.Input.GetKey(DebugKey) ? 1 : 0
                };
                OnMessageUpdate(data);
            }
        }
        private void OnMessageUpdate(SensorData data)
        {
            if (!isActiveAndEnabled || WaitTime + WAIT_SWITCH_TIME > Time.realtimeSinceStartup)
            {
                IsPressed = false;
                IsTriggered = false;
                return;
            }

            if(data.rotationZ < -8)
            {
                Direction = -1;
            }
            else if (data.rotationZ > 8)
            {
                Direction = 1;
            }
            else
            {
                Direction = 0;
            }

            IsHolding = data.isBoosted == 1;

            if (data.isBoosted == 1 && !IsPressed && !IsTriggered)
            {
                HoldTime = Time.realtimeSinceStartup;
                IsPressed = true;
            }
            else if (data.isBoosted != 1)
            {
                IsTriggered = false;
            }

            if (IsPressed && !IsTriggered)
            {
                if (Time.realtimeSinceStartup > HoldTime + HoldTriggerTime)
                {   
                    IsPressed = false;
                    IsTriggered = true;
                    OnHoldButton.Invoke();
                }
                else if (data.isBoosted != 1)
                {
                    IsPressed = false;
                    IsTriggered = true;
                    OnHitButton.Invoke();
                }
            }
        }
    }
}
