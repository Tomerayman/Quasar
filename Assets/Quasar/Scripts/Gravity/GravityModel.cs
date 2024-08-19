using Quasar.Models;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Quasar.Gravity
{
    public class GravityModel : SingletonModel<GravityModel>
    {
        public static HashSet<IGravityMaker> GravityMakers { get; private set; } = new HashSet<IGravityMaker>();

        protected override void Awake()
        {
            base.Awake();
        }

        public void RegisterGravityMaker(IGravityMaker gravityMaker)
        {
            GravityMakers.Add(gravityMaker);
        }
    }
}
