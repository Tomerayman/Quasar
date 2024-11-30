using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Quasar.Ship
{
    public class ShipView : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IPointerExitHandler
    {

        [field: SerializeField] public float HeightFromFloor { get; private set; } = 1;
        [field: SerializeField] public float GravityFactor { get; private set; } = 5;
        
        [SerializeField] private Transform backPart;
        [SerializeField] private List<Transform> tailQuadrants;
        [SerializeField, Tooltip("The distance factor for the tail quadrants charge out (x) and bac (y)")]
        private Vector2 chargeFactors = Vector2.one * 0.2f;

        private float rotateFactor = 1;
        private Coroutine tailRotateCR;

        private static List<Vector3> quadrantDirections = new List<Vector3>
        { 
            Vector3.forward, Vector3.right, Vector3.back, Vector3.left
        };

        public void TailChargeOpen(float pullPower)
        {
            for (int i = 0; i < tailQuadrants.Count; i++)
            {
                
                Vector3 offset = backPart.rotation * quadrantDirections[i] * chargeFactors.x + 
                    backPart.up * chargeFactors.y;
                tailQuadrants[i].position = backPart.position + offset * pullPower;
            }
            rotateFactor = 1 + pullPower;
        }

        public void OnShot(float shotPower)
        {
            IEnumerator LerpBackTail(float power, float duration)
            {
                float time = 0;
                while (time <= duration)
                {
                    yield return new WaitForEndOfFrame();
                }
            }
        }

        public Vector3 FloorShip()
        {
            if (Physics.Raycast(transform.position, Vector3.down * 100, out RaycastHit hit))
            {
                Vector3 flooredShipPos = new Vector3(transform.position.x, hit.point.y, transform.position.z) +
                    Vector3.up * HeightFromFloor;

                transform.position = Vector3.MoveTowards(transform.position, flooredShipPos, Time.deltaTime * GravityFactor);
            }
            return transform.position;
        }


        // ====== PRIVATE API: ====== //

        private void Start()
        {
            tailRotateCR = StartCoroutine(RotateTail());
        }

        private IEnumerator RotateTail()
        {
            while (true)
            {
                backPart.Rotate(Vector3.up * rotateFactor);
                yield return new WaitForEndOfFrame();
            }
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            ShipController.Instance.ShipViewHoverExit();
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            ShipController.Instance.ShipViewTouchStart();
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            ShipController.Instance.ShipViewTouchEnd();
        }
    }
}
