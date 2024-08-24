using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Quasar.Math
{
    public static class QMath
    {
        public static float LinearRemap(float val, float originMin, float originMax, float targetMin, float targetMax)
        {
            // clamp val to origin range to avoid irregularities
            val = Mathf.Clamp(val, originMin, originMax);

            // get origin range val ratio in [0, 1]
            float originRatio = (val - originMin) / (originMax - originMin);

            // map ratio to target range
            float result = targetMin + (targetMax - targetMin) * originRatio;
            return result;
        }
    }

}

