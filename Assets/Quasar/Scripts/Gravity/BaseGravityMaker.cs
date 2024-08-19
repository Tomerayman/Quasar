using Quasar.Gravity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Quasar.Gravity
{ 
    public abstract class BaseGravityMaker : MonoBehaviour, IGravityMaker
    {
        [field: SerializeField]
        public float GravityRange { get; protected set; } = 1;
        [field: SerializeField]
        public float GravityForce { get; protected set; } = 1;
        public Transform ObjectTransform { get => transform; }

        protected virtual void Awake()
        {
            GravityController.Instance.RegisterGravityMaker(this);
        }
    }
}


