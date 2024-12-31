using Quasar.Controllers;
using Quasar.Tiling;
using Quasar.UI.Game;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Quasar.Ship
{
    public class ShipController : SingletonController<ShipController>
    {
    
        [Header("Ship parameters")]
        [SerializeField] private float maxPullDistance = 2f;
        [field: SerializeField] public float ShipShootForce { get; private set; } = 5f;
        [SerializeField] private float shipRotationSpeed = 3f;



        [Header("Object links")]
        [SerializeField] private ShipView shipView;
        [SerializeField] private Rigidbody shipRb;
        [SerializeField] private SlingBarView slingBarView;

        private Plane m_slingPullPlane;
        private Coroutine m_PullCR = null;
        private InputController m_inputController;

        public bool IsClickingShip { get; private set; }
        public bool IsMidPull { get; private set; }

        public Vector3 ShootVector { get; private set; } = Vector3.zero;
        public float NormalizedPullStrength { get => Mathf.Clamp01(ShootVector.magnitude / maxPullDistance); }
        public Vector3 DirectionTarget { get; set; } = Vector3.forward;

        public Vector3 GetShipVelocity()
            => shipRb.velocity;

        private readonly Queue<Vector3> pullDirectionHistory = new Queue<Vector3>();
        private const int maxHistoryFrames = 10; // Adjust based on desired smoothness


        // ====== PRIVATE API: ====== //
        protected override void Awake()
        {
            base.Awake();
            IsMidPull = false;
        }

        private void Start()
        {
            m_inputController = InputController.Instance;
        }


        private void Update()
        {

            Vector3 alignedTarget = DirectionTarget;

            if (Vector3.Angle(shipView.transform.forward, alignedTarget) > 0.1f)
            {
                LerpRotationTowards(alignedTarget, Mathf.Max(1, shipRb.velocity.magnitude));
            }
            shipView.FloorShip();


            if (!IsMidPull)
            {
                DirectionTarget = GetShipVelocity().normalized;
            }
        }

        public void LerpRotationTowards(Vector3 direction, float relativeSpeed)
        {
            shipView.transform.forward = Vector3.Lerp(shipView.transform.forward, direction, shipRotationSpeed * Time.deltaTime);
        }

        public void ShipViewTouchStart()
            => IsClickingShip = true;

        public void ShipViewTouchEnd()
        {
            IsClickingShip = false;
            IsMidPull = false;
        }

        public void ShipViewHoverExit()
        {
            if (IsClickingShip && !IsMidPull)
            {
                m_PullCR = StartCoroutine(PullSling());
            }
        }

        private IEnumerator PullSling()
        {
            if (IsMidPull)
                yield break;

            IsMidPull = true;
            slingBarView.SetBarVisible(true);
            Vector3 shipPos, pullPos = Vector3.zero;

            while (IsMidPull)
            {
                shipPos = shipView.transform.position;
                pullPos = ScreenToSlingPullPlanePos();
                Vector3 currentPull = shipPos - pullPos;

                // Enforce max pull distance
                if (currentPull.magnitude > maxPullDistance)
                {
                    pullPos = shipPos + (pullPos - shipPos).normalized * maxPullDistance;
                    currentPull = shipPos - pullPos;
                }

                // Add current pull to history
                pullDirectionHistory.Enqueue(currentPull);
                if (pullDirectionHistory.Count > maxHistoryFrames)
                {
                    pullDirectionHistory.Dequeue();
                }

                // Compute the averaged pull direction
                Vector3 averagedPull = Vector3.zero;
                foreach (var pull in pullDirectionHistory)
                {
                    averagedPull += pull;
                }
                averagedPull /= pullDirectionHistory.Count;

                // Update ship's aim direction and UI
                ShootVector = averagedPull;
                DirectionTarget = ShootVector.normalized;
                slingBarView.UpdatePosition();
                shipView.TailChargeOpen(ShootVector.magnitude);

                Debug.Log(DirectionTarget);
                yield return null;
            }

            // Shoot the ship using the averaged pull vector
            ShootShip(ShootVector);
            slingBarView.SetBarVisible(false);
            shipView.TailChargeOpen(0);
        }


        public void ShootShip(Vector3 shootVector)
        {
            shipRb.velocity = ShipShootForce * shootVector;
        }

        private Vector3 ScreenToSlingPullPlanePos()
        {
            Ray ray = Camera.main.ScreenPointToRay(InputController.CurrMainTouchScreenPos);
            float enter;
            m_slingPullPlane = new Plane(Vector3.up, shipView.transform.position);
            if (m_slingPullPlane.Raycast(ray, out enter))
            {
                return ray.GetPoint(enter);
            }
            return Vector3.zero;
        }

        private float PullRelativeForce(Vector3 pullVector)
            => pullVector.magnitude / maxPullDistance;
    
    }
}
