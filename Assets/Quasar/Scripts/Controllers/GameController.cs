using Quasar.Ship;
using Quasar.Tiling;
using Quasar.Views;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Quasar.Controllers
{
    public class GameController : SingletonController<GameController>
    {

        public ShipView shipView;

        protected override void Awake()
        {
            base.Awake();
        }

        protected virtual void Start()
        {

        }

        private void Update()
        {

        }
    }

}

