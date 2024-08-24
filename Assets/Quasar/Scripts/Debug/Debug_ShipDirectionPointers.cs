using Quasar.Math;
using Quasar.Ship;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;


namespace Quasar.DEBUG
{
    public class Debug_ShipDirectionPointers : MonoBehaviour
    {
        public Transform velocityArrow;
        public Transform directionTargetArrow;
        [MinMaxSlider(0.3f, 5)]
        public Vector2 arrowScaleMinMax = new Vector2(0.3f, 5);

        private ShipView shipView;
        private Rigidbody ship_rb;

        private void Awake()
        {
            shipView = transform.parent.GetComponent<ShipView>();
            ship_rb = shipView.GetComponent<Rigidbody>();
        }

        private void Update()
        {
            if (!enabled) return;

            PointAndScaleArrow(velocityArrow, ship_rb.velocity, 0.1f, 4);
            PointAndScaleArrow(directionTargetArrow, ShipController.Instance.ShootVector, 0.1f, ShipController.Instance.ShipShootForce);
        }

        private void PointAndScaleArrow(Transform arrow, Vector3 target, float minMgtd, float maxMgtd)
        {
            if (target.magnitude > 0.001f)
            {
                arrow.gameObject.SetActive(true);

                arrow.forward = target;

                float zScale = QMath.LinearRemap(target.magnitude, minMgtd, maxMgtd, arrowScaleMinMax.x, arrowScaleMinMax.y);

                arrow.localScale = new Vector3(arrow.localScale.x, arrow.localScale.y, zScale);
            }
            else
            {
                arrow.gameObject.SetActive(false);
            }
        }
    }
}

