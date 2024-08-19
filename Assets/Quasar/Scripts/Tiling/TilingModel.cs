using Quasar.Models;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Quasar.Tiling
{
    public class TilingModel : SingletonModel<TilingModel>
    {
        public static HashSet<SpaceTileView> TileViews { get; private set; } = new HashSet<SpaceTileView>();


        protected override void Awake()
        {
            base.Awake();
        }

        public void RegisterTileView(SpaceTileView tileView)
        {
            TileViews.Add(tileView);
        }


    }
}