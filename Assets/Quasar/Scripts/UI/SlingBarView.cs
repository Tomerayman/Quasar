using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Quasar.UI.Game
{
    public class SlingBarView : MonoBehaviour
    {
        [SerializeField] private RectTransform barMask;
        [SerializeField] private RectTransform barTip;
        [SerializeField] private Texture2D barTexture;

        private RectTransform barRect;
        private Camera cam;
        private Vector2 OriginPos;
        private float maxPullLength;
        private float spriteToBarRatio;

        private void Awake()
        {
            barRect = GetComponent<RectTransform>();
            OriginPos = barRect.position;
            maxPullLength = barMask.rect.height;

            spriteToBarRatio = ((float)barMask.rect.height) / barTexture.height;

            cam = Camera.main;
        }

        public void SetBarVisible(bool visible)
        {
            gameObject.SetActive(visible);
        }

        public void UpdatePosition()
        {
            Vector2 viewTouchPos = InputController.CurrMainTouchScreenPos;
            Vector2 pullVector = viewTouchPos - OriginPos;
            if (pullVector.magnitude > maxPullLength)
            {
                pullVector = pullVector.normalized * maxPullLength;
            }

            float angle = Vector2.SignedAngle(Vector2.down, pullVector);
            barRect.eulerAngles = Vector3.forward * angle;

            barMask.sizeDelta = new Vector2(barMask.rect.width,
                Mathf.Min(pullVector.magnitude, maxPullLength));

            barTip.position = OriginPos + pullVector;
            barTip.gameObject.GetComponent<Image>().color = SampleBarColor(pullVector.magnitude);
        }

        private Color SampleBarColor(float pullMagnitude)
        {

            float y = pullMagnitude / spriteToBarRatio;
            Vector2 sampleCoordinates = new Vector2(barTexture.width / 2, y);
            return barTexture.GetPixel(
                (int)barTexture.width / 2, (int)y);
        }


    }

}

