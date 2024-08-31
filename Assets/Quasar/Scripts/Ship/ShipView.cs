using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Quasar.Ship
{
    public class ShipView : MonoBehaviour
    {
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


        // ====== PRIVATE API: ====== //

        private void Start()
        {
            tailRotateCR = StartCoroutine(RotateTail());
        }

        private void OnMouseDown()
        {
            ShipController.Instance.ShipViewTouchStart();
        }

        private void OnMouseUp()
        {
            ShipController.Instance.ShipViewTouchEnd();
        }

        private void OnMouseExit()
        {
            ShipController.Instance.ShipViewHoverExit();
        }

        private IEnumerator RotateTail()
        {
            while (true)
            {
                backPart.Rotate(Vector3.up * rotateFactor);
                yield return new WaitForEndOfFrame();
            }
        }


    }
}
