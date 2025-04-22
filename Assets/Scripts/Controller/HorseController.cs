using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using GameSystem.Input;
using Cinemachine;
using static UnityEngine.Rendering.DebugUI;
using NPC;
using UnityEngine.UIElements;
using UnityEngine.VFX;
using Autodesk.Fbx;

namespace Character
{
    public class HorseController : MonoBehaviour
    {
        public SkinnedMeshRenderer smr;
        public Material legsMaterial;
        public Material noLegsMaterial;
        public GameObject runSmear;
        public VisualEffect runDustVFX;
        public bool isPlayerHorse = false;
        public bool canMove = true;
        public float MaxSpeed = 10f;
        public float Acceleration = 10f;
        [Space(5)]
        public float BoosterSpeed = 6f;
        public float BoosterAcceleration = 6f;
        public float BoosterTime = 2.5f;
        public float MaxEnergy = 100f;
        [Space(5)]
        public float RotationRevision = 30f;
        public float BaseDriftTime = 0.6f;
        public float JumpSpeed = 10f;

        public float Gravity = 9.8f;

        public Camera VirtualCamera;
        public Animator HorseAnimator;
        public GameObject WrongWayNote;
        public Collider Collider;

        [HideInInspector]
        public Vector2 HVelocity;
        [HideInInspector]
        public Vector2 Direction;
        [HideInInspector]
        public float VVelocity;
        [HideInInspector]
        public int playerIndex;

        private int CurrentTrack;
        private float DriftRotation = 0;
        private float DriftTime = 0;
        private float StunTime = 0;
        private float SlowedTime = 0;
        private float CurrentBoostTime = 0;
        private float CurrentAcceleration = 0;
        private float TargetX;
        private float RotationSpeed;
        private Rigidbody Rigidbody;
        private Controller Controller;

        [HideInInspector]
        public float NextCheckPointDistance;
        [HideInInspector]
        public float SmallestDistance;
        [HideInInspector]
        public int CheckPointIndex;
        [HideInInspector]
        public Vector2 AdditionalForce;

        private int ResetIndex;
        private Vector3 ResetPoint;

        private bool IsOnGround = true;

        public static List<HorseController> Horses = new List<HorseController>();

        public float CurrentEnergy { get; private set; } = 0;
        public int Ranking { get; private set; } = 0;
        public float CurrentSpeed => HVelocity.magnitude;
        public PathPoint NextTargtet => NPCMap.GetAt(CheckPointIndex + 1);

        private PlayerItem playerItem;
        public GameObject horseUI;


        private static float Last_Ranking_UpdateTime = 0;
        private const float RANKING_UPDATE_INTERVAL = 0.1f;
        private const float Bounding_K = 0.1f;

        [SerializeField]
        private GameObject BoostTrailPrefab;

        public float ItemPickUpCooldown;

        [Header("Camera Culling")]
        [SerializeField]
        private GameObject speedLines;

        [SerializeField]
        private GameObject wrongWayIndicator;

        [HideInInspector]
        public int currentNode;
        public int currentLap;

        public void SetCullingLayer(int index)
        {
            speedLines.layer = index + 12;
            wrongWayIndicator.layer = index + 12;
            switch(index)
            { 
                case 0:
                    VirtualCamera.cullingMask = LayerMask.GetMask("Default", "Water", "Ground", "P1");
                    break;
                case 1:
                    VirtualCamera.cullingMask = LayerMask.GetMask("Default", "Water", "Ground", "P2");
                    break;
                case 2:
                    VirtualCamera.cullingMask = LayerMask.GetMask("Default", "Water", "Ground", "P3");
                    break;
                case 3:
                    VirtualCamera.cullingMask = LayerMask.GetMask("Default", "Water", "Ground", "P4");
                    break;
                default:
                    Debug.Log("Bad index in SetCullingLayer func");
                    break;

            }

        }

        public void PassedNode(int index)
        {
            //Debug.LogWarning(playerIndex.ToString() + ", " + index);

            currentNode++;

            if(playerIndex == 0)
            {
                Debug.LogWarning(playerIndex.ToString() + ", " + currentNode);
            }
        }

        public void Lapped()
        {
            currentNode = 0;
            currentLap++;
            if(currentLap > 2)
            {
                Debug.Log("player: " + playerIndex.ToString() + " won the race");
            }
        }


        public void ResetPos()
        {
            CheckPointIndex = ResetIndex;
            Rigidbody.position = ResetPoint;
        }

        private void Awake()
        {
            Horses.Add(this);
            runDustVFX.gameObject.SetActive(false);
        }
        private void OnDestroy()
        {
            Horses.Remove(this);
        }

        private void OnCollisionEnter(Collision collision)
        {
            if (collision.gameObject.CompareTag("Wall"))
            {
                Vector3 normal = collision.GetContact(0).normal;
                Vector2 normal2D = new Vector2(normal.x, normal.z);

                Vector2 newDir = Vector2.Reflect(HVelocity.normalized, normal2D);
                newDir += HVelocity.normalized * 0.5f;
                Direction = newDir.normalized;
                HVelocity = newDir.normalized * HVelocity.magnitude * 0.3f;

                //Debug.Log($"new Dir:{HVelocity}");
            }
        }

        private void Start()
        {
            CurrentEnergy = MaxEnergy;
            canMove = true;

        float arc = transform.eulerAngles.y / 180 * Mathf.PI;
            Direction = new Vector2(Mathf.Sin(arc), Mathf.Cos(arc));
            HVelocity = Vector2.zero;
            Rigidbody = this.GetComponent<Rigidbody>();

            foreach (Controller controller in this.GetComponents<Controller>())
            {
                if (controller.Enabled)
                {
                    Controller = controller;
                    break;
                }
            }

            InputLayer.AddAccelerateEventListener(Controller.CID, OnAccelerateUpdate);
            InputLayer.AddJumpEventListener(Controller.CID, OnJump);
            InputLayer.AddBoosterEventListener(Controller.CID, OnUseItem);
            InputLayer.AddRotateEventListener(Controller.CID, OnRotate);
            //InputLayer.AddChangeLaneEventListener(Controller.CID, OnChangeLane);
            InputLayer.AddDriftEventListener(Controller.CID, OnDrift);

            if (TrackManager.Instance == null)
            {
                TargetX = this.transform.position.x;
            }
            else
            {
                CurrentTrack = TrackManager.Instance.GetCurrentTrackIndex(this.transform.position);
                TargetX = TrackManager.Instance.GetCoordinate(CurrentTrack);
            }

            playerItem = GetComponent<PlayerItem>();

            //Carter addition
            currentNode = 0;
            currentLap = 1;
        }
        private void OnDrift()
        {
            if (!GameManager.IsGameBegin || !IsOnGround) return;
            if (Mathf.Abs(RotationSpeed) < 25f || HVelocity.magnitude < MaxSpeed * 0.75f) return;

            DriftTime = BaseDriftTime;
            DriftRotation = RotationSpeed > 0 ? 180f : -180f;
        }
        private void OnJump(float value)
        {
            if (!GameManager.IsGameBegin) return;
            if (IsOnGround && VVelocity < 0.5f)
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

        public void UseBooster()
        {
            if (!GameManager.IsGameBegin) return;
            CurrentBoostTime += BoosterTime;
            HorseAnimator.SetTrigger("Booster");
            //BoostTrailInstance = Instantiate(BoostTrailPrefab, this.gameObject.transform, false);
            //BoostTrailPrefab.SetActive(true);
            speedLines.SetActive(true);
            MusicManager.Instance?.mainTrackMusicSource?.PlayOneShot(MusicManager.Instance.boostAudio);
        }

        private void OnUseItem()
        {
            playerItem.UseCurrItem();
        }

        private void OnRotate(float value)
        {
            if (!GameManager.IsGameBegin) return;
            if (value == 0 && DriftTime > 0) return;

            RotationSpeed = value;

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

        private static void UpdateRanking()
        {
            if (Last_Ranking_UpdateTime <= Time.realtimeSinceStartup - RANKING_UPDATE_INTERVAL)
            {
                List<HorseController> ranking = new(Horses);
                Last_Ranking_UpdateTime = Time.realtimeSinceStartup;
                ranking.Sort(SortHorse);
                ranking.Reverse();

                for (int i = 0; i < ranking.Count; i++)
                {
                    ranking[i].Ranking = i;
                }
            }
        }
        private static int SortHorse(HorseController a, HorseController b)
        {
            //if (a.CheckPointIndex == b.CheckPointIndex)
            //{
            //    return a.NextCheckPointDistance.CompareTo(b.NextCheckPointDistance);
            //}
            //return a.CheckPointIndex.CompareTo(b.CheckPointIndex);
            if(a.currentLap > b.currentLap)
            {
                return a.currentLap.CompareTo(b.currentLap);
            }
            return a.currentNode.CompareTo(b.currentNode);
        }

        private void Update()
        {
            UpdateRanking();

            if (!canMove)
            {
                Rigidbody.velocity = Vector3.zero;
                HorseAnimator.SetFloat("Velocity", 0f);
                return;
            }

            if (!GameManager.IsGameBegin)
            {
                Rigidbody.velocity = Vector3.zero;
                return;
            }

            // //Just for test
            // if (Input.GetKeyDown(KeyCode.T))
            // {
            //     OnHitBlackHole();
            // }

            // Check Point Update
            //PathPoint nextPoint = NPCMap.GetAt(CheckPointIndex + 1);
            //PathPoint currentPoint = NPCMap.GetAt(CheckPointIndex);

            PathPoint nextPoint = NPCMap.GetAt(currentNode + 1);
            PathPoint currentPoint = NPCMap.GetAt(currentNode);

            Vector3 toCurrent = currentPoint.transform.position - this.transform.position;
            Vector3 toNext = nextPoint.transform.position - this.transform.position;

            NextCheckPointDistance = -(toNext - toCurrent * 2).magnitude;

            Vector3 forward = transform.TransformDirection(Vector3.forward);

            //if (NextCheckPointDistance > SmallestDistance)
            //{
            //    SmallestDistance = NextCheckPointDistance;
            //}
            //else if (NextCheckPointDistance < SmallestDistance - 10)
            //{
            //    SmallestDistance = NextCheckPointDistance + 10;
            //}
            if (Vector3.Dot(forward, toNext) < 0)
            {
                WrongWayNote.SetActive(true);
            }
            else
            {
                WrongWayNote.SetActive(false);
            }



            //if (WrongWayNote.activeSelf && NextCheckPointDistance >= SmallestDistance - 7.5f)
            //{
            //    WrongWayNote.SetActive(false);
            //}

            //if (toCurrent.magnitude * 1.35f < toNext.magnitude)
            //{
            //    CheckPointIndex++;
            //    ResetIndex = CheckPointIndex;
            //    ResetPoint = this.transform.position;
            //    SmallestDistance = -9999;
            //    //if (playerIndex == 0)
            //    //{
            //    //    Debug.LogWarning(playerIndex.ToString() + ", " + nextPoint.gameObject.name);
            //    //}
            //}

            RaycastHit hit;

            IsOnGround = Physics.Raycast(this.transform.position, Vector3.down, out hit, 0.65f, LayerMask.GetMask("Ground"));

            if (IsOnGround && VVelocity < 0)
            {
                VVelocity = Mathf.Min(Rigidbody.velocity.y, -0.35f);
            }
            else
            {
                VVelocity -= Gravity * Time.deltaTime;
            }

            float frameRotation = RotationSpeed;
            if (DriftTime > 0)
            {
                DriftTime -= Time.deltaTime;
                frameRotation = DriftRotation + RotationSpeed;
                DriftRotation *= 1 - 0.9f * Time.deltaTime;
            }

            if (!IsOnGround) frameRotation *= 0.1f;
            float frameAngle = Mathf.Atan2(Direction.y, Direction.x) - frameRotation / 180 * Mathf.PI * Time.deltaTime;
            Direction = new Vector2(Mathf.Cos(frameAngle), Mathf.Sin(frameAngle));

            //this.transform.localEulerAngles = new Vector3(0, -frameAngle / Mathf.PI * 180 + 90, 0);

            if (IsOnGround && hit.collider != null)
            {
                Quaternion groundRotation = Quaternion.FromToRotation(Vector3.up, hit.normal);
                Quaternion forwardRotation = Quaternion.Euler(0, -frameAngle / Mathf.PI * 180 + 90, 0);
                Quaternion targetRotation = groundRotation * forwardRotation;

                float rotationSpeed = 3f;
                transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * rotationSpeed);
            }

            float acceleration = IsOnGround ? CurrentAcceleration : (CurrentAcceleration * 0.15f);
            float maxSpeed = MaxSpeed;

            // This code is for changing lane
            /*float xMovement = HVelocity.magnitude * Mathf.Sqrt(2) / 2 * Time.fixedDeltaTime;
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
            }*/

            if (CurrentBoostTime > 0)
            {
                CurrentBoostTime -= Time.deltaTime;
                maxSpeed += BoosterSpeed;
                acceleration = Mathf.Max(0, acceleration);
                acceleration += BoosterAcceleration;
                if (CurrentBoostTime <= 0)
                {
                    BoostTrailPrefab.SetActive(false);
                    speedLines.SetActive(false);
                }
            }

            if (HVelocity.magnitude > 0.1f && Direction.magnitude > 0.1f)
            {
                Vector2 vDir = HVelocity.normalized;
                Vector2 dir = Direction.normalized;

                float k = (vDir - dir).magnitude * 2;
                if (DriftTime > 0) k *= 0;

                HVelocity *= (1 - k * Time.deltaTime);

                float vAngle = Mathf.Atan2(vDir.y, vDir.x) / Mathf.PI * 180;
                float angle = Mathf.Atan2(dir.y, dir.x) / Mathf.PI * 180;

                float angleDiff = vAngle - angle;
                while (angleDiff > 180) angleDiff -= 360f;
                while (angleDiff < -180) angleDiff += 360f;

                float rotationRevision = DriftTime > 0 ? RotationRevision * 2f : RotationRevision;
                float deltaAngle = rotationRevision * Time.deltaTime;

                if (DriftTime > 0)
                {
                    DriftTime += Mathf.Clamp(Mathf.Abs(angleDiff), 0, 60) / 60f * Time.deltaTime * 0.6f;
                }

                if (Mathf.Abs(angleDiff) < deltaAngle)
                {
                    vAngle = angle;
                }
                else if (angleDiff > 0)
                {
                    vAngle -= deltaAngle;
                }
                else
                {
                    vAngle += deltaAngle;
                }

                float result = vAngle / 180f * Mathf.PI;
                HVelocity = new Vector2(Mathf.Cos(result), Mathf.Sin(result)) * HVelocity.magnitude;
            }

            float frameAccelerate = (HVelocity.magnitude > maxSpeed ? -Acceleration : acceleration) * Time.deltaTime;

            if (frameAccelerate > 0)
            {
                HVelocity += Direction * frameAccelerate;
                if (HVelocity.magnitude > 0) HVelocity = HVelocity.magnitude > maxSpeed ? HVelocity * maxSpeed / HVelocity.magnitude : HVelocity;
            }
            else
            {
                frameAccelerate = Mathf.Abs(frameAccelerate);
                if (HVelocity.magnitude < frameAccelerate) HVelocity = Vector2.zero;
                else if (HVelocity.magnitude > 0) HVelocity = HVelocity * (HVelocity.magnitude - frameAccelerate) / HVelocity.magnitude;
            }

            if (StunTime > 0)
            {
                StunTime -= Time.deltaTime;
                VVelocity = 0;
            }

            if (SlowedTime > 0)
            {
                SlowedTime -= Time.deltaTime;
            }

            //HVelocity = Direction * HVelocity.magnitude;

            float afm = AdditionalForce.magnitude;
            if (afm < frameAccelerate)
            {
                AdditionalForce = Vector2.zero;
            }
            else
            {
                AdditionalForce = AdditionalForce.normalized * (afm - frameAccelerate);
            }

            Vector3 hv = new Vector3(HVelocity.x, 0, HVelocity.y);
            Vector3 vv = new Vector3(0, VVelocity, 0);
            Vector3 av = new Vector3(AdditionalForce.x, 0, AdditionalForce.y);

            if (IsOnGround)
            {
                Quaternion groundRotation = Quaternion.FromToRotation(Vector3.up, hit.normal);
                hv = groundRotation * hv;
                if(hv.y < 0)
                {
                    hv.y *= 0.35f;
                }
            }

            Vector3 velocity = hv + vv;

            //Rigidbody.velocity = StunTime > 0 ? Vector3.zero : velocity;

            if (StunTime > 0)
            {
                Rigidbody.velocity = Vector3.zero;
            }
            else if (SlowedTime > 0)
            {
                Rigidbody.velocity = velocity * 0.3f;
            }
            else
            {
                Rigidbody.velocity = velocity;
            }
            Rigidbody.velocity += av;

            if (VirtualCamera != null) VirtualCamera.fieldOfView = 50 +
                    Mathf.Clamp(HVelocity.magnitude * HVelocity.magnitude, 0, 400f) / 20f;

            float currentVelocity = StunTime > 0 ? 0 : HVelocity.magnitude;

            HorseAnimator.SetFloat("Velocity", currentVelocity);
            HorseAnimator.SetFloat("BoosterTime", CurrentBoostTime);
            HorseAnimator.SetFloat("StunTime", StunTime);
            HorseAnimator.SetFloat("Direction", RotationSpeed);

            if(currentVelocity < 12)
            {
                SwitchMaterial(legsMaterial);
                runSmear.SetActive(false);
                runDustVFX.gameObject.SetActive(false);
            }
            else
            {
                SwitchMaterial(noLegsMaterial);
                runSmear.SetActive(true);
                runDustVFX.gameObject.SetActive(true);
            }

            if (ItemPickUpCooldown >= 0)
            {
                ItemPickUpCooldown -= Time.deltaTime;
            }
        }

        public void OnCrossingBarrier(float energyAddValue)
        {
            CurrentEnergy += energyAddValue;
            CurrentEnergy = Mathf.Clamp(CurrentEnergy, 0, MaxEnergy);
        }
        public void OnHitBarrier()
        {
            StunTime = 2;
        }

        public void OnHitChick()
        {
            HorseAnimator.SetTrigger("HitChick");
            StunTime = 1f;
            MusicManager.Instance?.mainTrackMusicSource?.PlayOneShot(MusicManager.Instance.horseHitAudio);
        }

        public void OnHitBlackHole()
        {
            HorseAnimator.SetTrigger("HitBlackHole");
            StunTime = 3f;
            MusicManager.Instance?.mainTrackMusicSource?.PlayOneShot(MusicManager.Instance.horseFallAudio);
        }

        public void OnHitDustDevil()
        {
            HorseAnimator.SetTrigger("HitChick");
            StunTime = 1;
            MusicManager.Instance?.mainTrackMusicSource?.PlayOneShot(MusicManager.Instance.horseHitAudio);
        }

        public void OnHitManure()
        {
            SlowedTime += 3;
        }

        public void OnLassoHitTarget()
        {
            Debug.Log(gameObject.name + " use Lasso");
        }

        public void OnHitByLasso()
        {
            SlowedTime += 2;

            Debug.Log(gameObject.name + " hit by lasso");
        }

        public void SwitchMaterial(Material legMat)
        {
            var mats = smr.materials;
            mats[1] = legMat;
            smr.materials = mats;
        }
        public void DisableMovement()
        {
            canMove = false;

            if (TryGetComponent<Rigidbody>(out var rb))
            {
                rb.velocity = Vector3.zero;
                rb.angularVelocity = Vector3.zero;
                rb.constraints = RigidbodyConstraints.FreezeAll;
            }

            HorseAnimator.SetFloat("Velocity", 0f);
        }

    }

}
