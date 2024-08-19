
using Quasar.Views;
using System;
using UnityEngine;

namespace Quasar.Gravity
{
    public interface IGravityMaker
    {
        public float GravityRange { get; }
        public float GravityForce { get; }
        public Transform ObjectTransform {get;}
        
    }

}

