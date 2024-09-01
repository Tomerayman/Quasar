using NaughtyAttributes;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Quasar.DEBUG
{
    public class DebugUIController : MonoBehaviour
    {
        [field: SerializeField]
        public bool DebugUiActive { get; private set; } = true;

        [SerializeField]
        private FPSCounter fpsCounter;

        


        private void Start()
        {
            
        }

        private void Init()
        {
            fpsCounter.enabled = DebugUiActive;
        }

        [Button]
        public void ToggleFPSCounter()
        {
            fpsCounter.gameObject.SetActive(!fpsCounter.enabled);
            fpsCounter.enabled = !fpsCounter.enabled;
        }
    }

}


