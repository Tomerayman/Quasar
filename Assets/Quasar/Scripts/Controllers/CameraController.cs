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
        [SerializeField, Range(5, 60)] private float cameraDownAngle = 20f;
        [SerializeField] private float movementThreshold = 0.1f;
        [SerializeField] private float cameraMoveLerpSpeed = 2f;
        //[Range(0f, 50f)]
        //[SerializeField] private float maxCameraRotationDegreesOnPull = 15f;

        [Header("Object Links")]
        [SerializeField] private Camera cameraView;
        [SerializeField] private Transform shipViewTransform;
        private Transform m_cameraTransform;
        private float m_baseCameraXTilt;

        #region MonoB
        private void Awake()
        {
            m_cameraTransform = cameraView.transform;
            m_cameraTransform.position = shipViewTransform.position + cameraToShipVector;
            m_baseCameraXTilt = m_cameraTransform.eulerAngles.x;
        }

        private void FixedUpdate()
        {
            LerpCameraToShip();
        }


        #endregion MonoB

        #region Private


        private void LerpCameraToShip()
        {
            // Calculate the current offset from the ship to the camera
            Vector3 currOffset = (m_cameraTransform.position - shipViewTransform.position).normalized * cameraToShipVector.magnitude;

            // Calculate the target offset based on the ship's orientation
            Vector3 targetOffset = 
                shipViewTransform.up * cameraToShipVector.y +
                shipViewTransform.forward * cameraToShipVector.z;

            // Determine the step angle for rotating the offset
            float stepAngle = cameraMoveLerpSpeed * Time.deltaTime * 
                              Vector3.Angle(currOffset, targetOffset);
            
            // Calculate the new offset using Vector3.RotateTowards
            Vector3 newOffset = Vector3.RotateTowards(currOffset, targetOffset, stepAngle * Mathf.Deg2Rad, 0);

            // Update the camera's position
            m_cameraTransform.position = shipViewTransform.position + newOffset;

            // Update the camera's rotation to look at the ship, maintaining its position on the screen
            m_cameraTransform.rotation = Quaternion.LookRotation(shipViewTransform.position - m_cameraTransform.position, Vector3.up);
            m_cameraTransform.eulerAngles -= Vector3.right * cameraDownAngle;

        }

        #endregion Private
    }
}
