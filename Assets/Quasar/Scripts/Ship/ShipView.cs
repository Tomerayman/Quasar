using Quasar.Tiling;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Quasar.Ship
{
    public class ShipView : MonoBehaviour
    {
        [Header("Design parameters")]
        [SerializeField] private float shipRotationSpeed = 3f;

        [Header("Object links")]
        [SerializeField] private ShipController shipController;

        public Vector3 DirectionTarget { get; set; } = Vector3.forward;


        public void LerpRotationTowards(Vector3 direction, float relativeSpeed)
        {
            transform.forward = Vector3.Lerp(transform.forward, direction, relativeSpeed * shipRotationSpeed * Time.deltaTime);
        }

        private void Update()
        {
            Vector3 shipPos = transform.position;
            var tup = TilingController.Instance.GetClosestTile(shipPos).GetYandNormalAtPos(shipPos);
            if (tup != null)
            {
                transform.position = new Vector3(shipPos.x, tup.Item1, shipPos.z);
                Vector3 alignedTarget = -Vector3.Cross(Vector3.Cross(DirectionTarget, tup.Item2), tup.Item2);
                if (Vector3.Angle(transform.forward, DirectionTarget) > 0.1f)
                {
                    LerpRotationTowards(alignedTarget, shipRotationSpeed);
                }
            }
        }

        // ====== PRIVATE API: ====== //

        private void OnMouseDown()
        {
            shipController.ShipViewTouchStart();
        }

        private void OnMouseUp()
        {
            shipController.ShipViewTouchEnd();
        }

        private void OnMouseExit()
        {
            shipController.ShipViewHoverExit();
        }
    }
}
