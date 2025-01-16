using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.Rendering.DebugUI;

namespace GameSystem.Input
{
    public class HorseController : MonoBehaviour
    {
        public float MaxSpeed = 10f;
        public float Acceleration = 10f;
        public float MaxRotation = 45f;
        public float JumpSpeed = 10f;

        private float CurrentAcceleration = 0;

        private Vector3 Direction;
        private Vector3 Velocity;
        private Rigidbody Rigidbody;
        private Controller Controller;

        private void Start()
        {
            Direction = Vector3.forward;
            Controller = this.GetComponent<Controller>();
            Rigidbody = this.GetComponent<Rigidbody>();

            InputLayer.AddAccelerateEventListener(Controller.CID, OnAccelerateUpdate);
        }

        private void OnAccelerateUpdate(float value)
        {
            CurrentAcceleration = value * Acceleration;
        }

        private void Update()
        {
            Velocity += Direction * CurrentAcceleration * Time.deltaTime;
            Velocity = Velocity.magnitude > MaxSpeed ? Velocity * MaxSpeed / Velocity.magnitude : Velocity;

            Rigidbody.velocity = Velocity;
        }
    }

}
