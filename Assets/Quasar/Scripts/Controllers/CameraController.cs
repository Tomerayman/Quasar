using Quasar.Ship;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Quasar.PlayerCamera
{
    public class CameraController : MonoBehaviour
    {

        [Header("Design parameters")]
        [SerializeField] private Vector3 cameraToShipVector = new Vector3(0, 15, 0);
        [SerializeField] private float movementThreshold = 0.1f;
        [SerializeField] private float cameraMoveLerpSpeed = 2f;
        //[Range(0f, 50f)]
        //[SerializeField] private float maxCameraRotationDegreesOnPull = 15f;

        [Header("Object Links")]
        [SerializeField] private Camera cameraView;
        [SerializeField] private Transform shipViewTransform;
        private Transform m_cameraTransform;

        #region MonoB
        private void Awake()
        {
            m_cameraTransform = cameraView.transform;
            m_cameraTransform.position = shipViewTransform.position + cameraToShipVector;
        }

        private void FixedUpdate()
        {
            LerpFollowShip();
        }


        #endregion MonoB

        #region Private

        private void LerpFollowShip()
        {
            Vector3 cameraOffset = cameraToShipVector;

            // Lower camera based on pull:
            
            //float cameraRotation = 0;
            //if (ShipController.Instance.NormalizedPullStrength > 0.001)
            //{
            //    cameraRotation = ShipController.Instance.NormalizedPullStrength * -maxCameraRotationDegreesOnPull;
            //    cameraOffset = Quaternion.Euler(cameraRotation, 0, 0) * cameraOffset;
            //}
            
            
            // Immediate position target for lerping
            Vector3 posTarget = shipViewTransform.position +
                shipViewTransform.up * cameraOffset.y +
                shipViewTransform.forward * cameraOffset.z;

            // Immediate rotation target for lerping
            Quaternion rotTarget = Quaternion.Euler(m_cameraTransform.eulerAngles.x,
                shipViewTransform.eulerAngles.y, m_cameraTransform.eulerAngles.z);

            // Lerp step, adjusted by ship's velocity                
            float step = cameraMoveLerpSpeed * Time.deltaTime * Mathf.Max(ShipController.Instance.GetShipVelocity().magnitude, 1);

            // LERP
            m_cameraTransform.position = Vector3.Lerp(m_cameraTransform.position, posTarget, step);
            m_cameraTransform.rotation = Quaternion.Lerp(m_cameraTransform.rotation, rotTarget, step);
        }

        #endregion Private
    }
}
