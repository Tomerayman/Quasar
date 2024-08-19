using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Quasar.Ship
{
    public class SlingView : MonoBehaviour
    {
        public void SetSlingPos(Vector3 pos)
        {
            transform.position = pos;
        }

    }
}

