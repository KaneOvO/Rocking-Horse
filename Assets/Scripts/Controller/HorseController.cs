using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using GameSystem.Input;
using Cinemachine;

namespace Character
{
    public class HorseController : MonoBehaviour
    {
        public float MaxSpeed = 10f;
        public float Acceleration = 10f;
        [Space(5)]
        public float BoosterSpeed = 6f;
        public float BoosterAcceleration = 6f;
        public float BoosterTime = 2.5f;
        public float MaxEnergy = 100f;
        [Space(5)]
        public float MaxRotation = 45f;
        public float JumpSpeed = 10f;

        public float Gravity = 9.8f;

        public CinemachineVirtualCamera VirtualCamera;
        public Animator HorseAnimator;

        private int CurrentTrack;
        private float StunTime = 0;
        private float CurrentBoostTime = 0;
        private float CurrentAcceleration = 0;
        private float VVelocity;
        private float TargetX;
        private Vector2 Direction;
        private Vector2 HVelocity;
        private Rigidbody Rigidbody;
        private Controller Controller;

        private bool IsOnGround = true;

        public float CurrentEnergy { get; private set; } = 0;

        private void Start()
        {
            CurrentEnergy = MaxEnergy;

            Direction = Vector2.up;
            HVelocity = Vector2.zero;
            Rigidbody = this.GetComponent<Rigidbody>();

            foreach(Controller controller in this.GetComponents<Controller>())
            {
                if(controller.Enabled)
                {
                    Controller = controller;
                    break;
                }
            }

            InputLayer.AddAccelerateEventListener(Controller.CID, OnAccelerateUpdate);
            InputLayer.AddJumpEventListener(Controller.CID, OnJump);
            InputLayer.AddBoosterEventListener(Controller.CID, OnUseBooster);
            InputLayer.AddChangeLaneEventListener(Controller.CID, OnChangeLane);

            if(TrackManager.Instance == null)
            {
                TargetX = this.transform.position.x;
            }
            else
            {
                CurrentTrack = TrackManager.Instance.GetCurrentTrackIndex(this.transform.position);
                TargetX = TrackManager.Instance.GetCoordinate(CurrentTrack);
            }
        }
        private void OnJump(float value)
        {
            if(IsOnGround || !GameManager.IsGameBegin)
            {
                VVelocity += JumpSpeed * value;
                HorseAnimator.SetTrigger("Jump");
            }
        }
        private void OnUseBooster()
        {
            if (CurrentEnergy < 100f || !GameManager.IsGameBegin) return;
            CurrentBoostTime += BoosterTime;
            CurrentEnergy -= 100f;
            HorseAnimator.SetTrigger("Booster");
        }
        private void OnChangeLane(Vector2 direction)
        {
            if (TrackManager.Instance == null || !GameManager.IsGameBegin) return;
            if (direction.x < 0)
            {
                if (TrackManager.Instance.CanSwitchLeft(CurrentTrack))
                {
                    TargetX = TrackManager.Instance.SwitchLeft(CurrentTrack);
                    CurrentTrack--;
                }
            }
            else
            {
                if (TrackManager.Instance.CanSwitchRight(CurrentTrack))
                {
                    TargetX = TrackManager.Instance.SwitchRight(CurrentTrack);
                    CurrentTrack++;
                }
            }
        }
        private void OnAccelerateUpdate(float value)
        {
            if (!GameManager.IsGameBegin) return;
            value = value * 2f - 1f;
            value = Mathf.Clamp(value, -1f, 1f);
            CurrentAcceleration = value * Acceleration;
        }

        private void Update()
        {
            if (!GameManager.IsGameBegin)
            {
                Rigidbody.velocity = Vector3.zero;
                return;
            }
            IsOnGround = Physics.Raycast(this.transform.position, Vector3.down, 0.65f, LayerMask.GetMask("Ground"));

            if (IsOnGround && VVelocity < 0)
            {
                VVelocity = Mathf.Min(Rigidbody.velocity.y, -1f);
            }
            else
            {
                VVelocity -= Gravity * Time.deltaTime;
            }

            float acceleration = IsOnGround ? CurrentAcceleration : (CurrentAcceleration * 0.15f);
            float maxSpeed = MaxSpeed;

            float xMovement = HVelocity.magnitude * Mathf.Sqrt(2) / 2 * Time.fixedDeltaTime;
            float xDiff = this.transform.position.x - TargetX;


            if (xMovement >= Mathf.Abs(xDiff))
            {
                Direction = Vector2.up;
                if(Mathf.Abs(xDiff) > Time.fixedDeltaTime)
                {
                    Vector3 newPos = this.transform.position;
                    newPos.x = TargetX;
                    Rigidbody.MovePosition(newPos);
                }
            }
            else if (xDiff > 0)
            {
                Direction = new Vector2(-1, 1).normalized;
            }
            else
            {
                Direction = new Vector2(1, 1).normalized;
            }

            if (CurrentBoostTime > 0)
            {
                CurrentBoostTime -= Time.deltaTime;
                maxSpeed += BoosterSpeed;
                acceleration = Mathf.Max(0, acceleration);
                acceleration += BoosterAcceleration;
            }

            float frameAccelerate = (HVelocity.magnitude > maxSpeed ? -Acceleration : acceleration) * Time.deltaTime;

            if (frameAccelerate > 0)
            {
                HVelocity += Direction * frameAccelerate;
                if(HVelocity.magnitude > 0) HVelocity = HVelocity.magnitude > maxSpeed ? HVelocity * maxSpeed / HVelocity.magnitude : HVelocity;
            }
            else
            {
                frameAccelerate = Mathf.Abs(frameAccelerate);
                if (HVelocity.magnitude < frameAccelerate) HVelocity = Vector2.zero;
                else if(HVelocity.magnitude > 0) HVelocity = HVelocity * (HVelocity.magnitude - frameAccelerate) / HVelocity.magnitude;
            }

            if (StunTime > 0)
            {
                StunTime -= Time.deltaTime;
                VVelocity = 0;
            }

            HVelocity = Direction * HVelocity.magnitude;

            Vector3 velocity = new Vector3(HVelocity.x, VVelocity, HVelocity.y);

            Rigidbody.velocity = StunTime > 0 ? Vector3.zero : velocity;

            if(VirtualCamera != null) VirtualCamera.m_Lens.FieldOfView = 40 + 
                    Mathf.Clamp(HVelocity.magnitude * HVelocity.magnitude, 0, 400f) / 20f;

            HorseAnimator.SetFloat("Velocity", StunTime > 0 ? 0 : HVelocity.magnitude);
            HorseAnimator.SetFloat("BoosterTime", CurrentBoostTime);
        }

        public void OnCrossingBarrier(float energyAddValue)
        {
            CurrentEnergy += energyAddValue;
            CurrentEnergy = Mathf.Clamp(CurrentEnergy, 0, MaxEnergy);
        }
        public void OnHitBarrier()
        {
            StunTime = 1;
        }
    }

}
