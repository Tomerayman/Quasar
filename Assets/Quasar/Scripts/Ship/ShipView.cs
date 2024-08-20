using Quasar.Tiling;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Quasar.Ship
{
    public class ShipView : MonoBehaviour
    {
    
           

        // ====== PRIVATE API: ====== //

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
    }
}
