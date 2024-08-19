using Quasar.Tiling;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Quasar.Ship
{
    public class ShipController : MonoBehaviour
    {
    
        [Header("Ship parameters")]
        [SerializeField] private float maxPullDistance = 2f;
        [SerializeField] private float shipShootForce = 5f;

        [Header("Object links")]
        [SerializeField] private ShipView shipView;
        [SerializeField] private Rigidbody shipRb;
        [SerializeField] private SlingView slingView;

        private Plane m_slingPullPlane;
        private Coroutine m_PullCR = null;
        private InputController m_inputController;

        public static ShipController Instance { get; private set; }
        public bool IsClickingShip { get; private set; }
        public bool IsMidPull { get; private set; }

        public Vector3 GetShipVelocity()
            => shipRb.velocity;


        // ====== PRIVATE API: ====== //

    

        private void Awake()
        {
            Instance = this;
            IsMidPull = false;
            m_slingPullPlane = new Plane(Vector3.up, transform.position);
        }

        private void Start()
        {
            m_inputController = InputController.Instance;
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
                m_PullCR = StartCoroutine(PullSling());
        }

        private IEnumerator PullSling()
        {
            if (IsMidPull)
                yield break;
            IsMidPull = true;
            Vector3 shipPos, pullPos, shootVector = Vector3.zero;
            while (IsMidPull)
            {
                shipPos = shipView.transform.position;
                pullPos = ScreenToSlingPullPlanePos();
                if (Vector3.Distance(shipPos, pullPos) > maxPullDistance)
                    pullPos = shipPos + (pullPos - shipPos).normalized * maxPullDistance;
                shootVector = shipPos - pullPos;
                slingView.SetSlingPos(pullPos);
                // aim ship:
                //shipView.LerpRotationTowards(shootVector, PullRelativeForce(shootVector));
                shipView.DirectionTarget = shootVector;
                yield return null;
            }
            ShootShip(shootVector);
        }

        public void ShootShip(Vector3 shootVector)
        {
            shipRb.AddForce(shipShootForce * shootVector, ForceMode.Impulse);
        }

        private Vector3 ScreenToSlingPullPlanePos()
        {
            Ray ray = Camera.main.ScreenPointToRay(InputController.CurrMainTouchScreenPos);
            float enter;
            if (m_slingPullPlane.Raycast(ray, out enter))
                return ray.GetPoint(enter);
            return Vector3.zero;
        }

        private float PullRelativeForce(Vector3 pullVector)
            => pullVector.magnitude / maxPullDistance;
    
    }
}
