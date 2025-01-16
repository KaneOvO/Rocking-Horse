using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
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

        public float Gravity = 9.8f;

        private float CurrentAcceleration = 0;
        private float VVelocity;
        private Vector2 Direction;
        private Vector2 HVelocity;
        private Rigidbody Rigidbody;
        private Controller Controller;

        private bool IsOnGround = true;

        private void Start()
        {
            Direction = Vector2.up;
            Controller = this.GetComponent<Controller>();
            Rigidbody = this.GetComponent<Rigidbody>();

            InputLayer.AddAccelerateEventListener(Controller.CID, OnAccelerateUpdate);
            InputLayer.AddJumpEventListener(Controller.CID, OnJump);
        }
        private void OnJump(float value)
        {
            if(IsOnGround) VVelocity += JumpSpeed * value;
        }
        private void OnAccelerateUpdate(float value)
        {
            CurrentAcceleration = value * Acceleration;
        }

        private void Update()
        {
            IsOnGround = Physics.Raycast(this.transform.position, Vector3.down, 0.65f, LayerMask.GetMask("Ground"));

            if (IsOnGround && VVelocity < 0)
            {
                VVelocity = Mathf.Min(Rigidbody.velocity.y, -1f);
            }
            else
            {
                VVelocity -= Gravity * Time.deltaTime;
            }

            CurrentAcceleration -= 0.5f;

            float acceleration = IsOnGround ? CurrentAcceleration : (CurrentAcceleration * 0.15f);
            float frameAccelerate = acceleration * Time.deltaTime;

            if (frameAccelerate > 0)
            {
                HVelocity += Direction * frameAccelerate;
            }
            else
            {
                frameAccelerate = Mathf.Abs(frameAccelerate);
                if (HVelocity.magnitude < frameAccelerate) HVelocity = Vector2.zero;
                else HVelocity = HVelocity * (HVelocity.magnitude - frameAccelerate) / HVelocity.magnitude;
            }

            HVelocity = HVelocity.magnitude > MaxSpeed ? HVelocity * MaxSpeed / HVelocity.magnitude : HVelocity;

            Vector3 velocity = new Vector3(HVelocity.x, VVelocity, HVelocity.y);

            Rigidbody.velocity = velocity;
        }
    }

}
