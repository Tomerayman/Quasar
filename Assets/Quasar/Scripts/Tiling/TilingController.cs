using Quasar.Controllers;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Quasar.Tiling
{
    public class TilingController : SingletonController<TilingController>
    {
        protected override void Awake()
        {
            base.Awake();

        }

        public void RegisterSpaceTile(SpaceTileView tileView)
        {
            TilingModel.Instance.RegisterTileView(tileView);
        }

        public SpaceTileView GetClosestTile(Vector3 position)
        {
            SpaceTileView closestTile = null;
            float minDistance = Mathf.Infinity;
            foreach (SpaceTileView tile in TilingModel.TileViews)
            {
                float sqrDistance = tile.Tile.GetTileBounds().SqrDistance(position);
                if (tile.Tile.GetTileBounds().SqrDistance(position) < minDistance)
                {
                    minDistance = sqrDistance;
                    closestTile = tile;
                }
            }
            return closestTile;
        }
    }

}

