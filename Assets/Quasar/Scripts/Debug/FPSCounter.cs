using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace Quasar.DEBUG
{
    public class FPSCounter : MonoBehaviour
    {
        public TMP_Text display_Text;
        private int avgFrameRate;

        public void Update()
        {
            if (this.enabled)
            {
                float current = Time.frameCount / Time.time;
                avgFrameRate = (int)current;
                display_Text.text = avgFrameRate.ToString() + " FPS";
            }
        }

    }
}

