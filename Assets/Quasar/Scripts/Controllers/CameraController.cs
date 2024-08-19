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
            float cameraOffset = Vector3.Distance(m_cameraTransform.position - cameraToShipVector, shipViewTransform.position);
            Vector3 posTarget = shipViewTransform.position +
                shipViewTransform.up * cameraToShipVector.y +
                shipViewTransform.forward * cameraToShipVector.z;
            Vector3 rotTarget = new Vector3(m_cameraTransform.eulerAngles.x, shipViewTransform.eulerAngles.y, m_cameraTransform.eulerAngles.z);
                
            float step = cameraMoveLerpSpeed * Time.deltaTime * Mathf.Max(ShipController.Instance.GetShipVelocity().magnitude, 1);
            m_cameraTransform.position = Vector3.Lerp(m_cameraTransform.position, posTarget, step);
            m_cameraTransform.rotation = Quaternion.Lerp(m_cameraTransform.rotation, Quaternion.Euler(rotTarget), step);
            if (cameraOffset > movementThreshold)
            {
                
            }

        }

        #endregion Private
    }
}
