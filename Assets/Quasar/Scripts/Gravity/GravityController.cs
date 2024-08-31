using Quasar.Controllers;
using Quasar.Tiling;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Quasar.Gravity
{

    public class GravityController : SingletonController<GravityController>
    {
        protected override void Awake()
        {
            base.Awake();
        }

        protected void Update()
        {
            ApplyGravityToTiles(GravityModel.GravityMakers, TilingModel.TileViews);
        }

        public void RegisterGravityMaker(IGravityMaker gravityMaker)
        {
            GravityModel.Instance.RegisterGravityMaker(gravityMaker);
        }


        public void ApplyGravityToTiles(ICollection<IGravityMaker> gravityMakers, ICollection<SpaceTileView> tiles)
        {
            foreach (SpaceTileView tileView in tiles)
            {
                Bounds tileBounds = tileView.Tile.GetTileBounds();
                List<IGravityMaker> tileGravityMakers = new List<IGravityMaker>();
                foreach (IGravityMaker gravityMaker in gravityMakers)
                {
                    if (GravityMakerAffectsTile(gravityMaker, tileBounds))
                    {
                        tileGravityMakers.Add(gravityMaker);
                    }
                }
                if (tileGravityMakers.Count > 0)
                {
                    tileView.AddGravityMakers(tileGravityMakers);
                }
            }
        }


        /// <summary>
        /// Determines if a GravityMaker's range affects a SpaceTile by checking the shortest distance from the GravityMaker to the tile's bounds.
        /// </summary>
        /// <param name="gravityMaker">The gravity maker to check.</param>
        /// <param name="tile">The space tile to check against.</param>
        /// <returns>True if the GravityMaker's range intersects with the SpaceTile, false otherwise.</returns>
        public static bool GravityMakerAffectsTile(IGravityMaker gravityMaker, Bounds tileBounds)
        {
            // Get the position of the gravity maker
            Vector3 gravityMakerPosition = gravityMaker.ObjectTransform.position;

            // Calculate the closest point on the tile bounds to the gravity maker
            Vector3 closestPoint = tileBounds.ClosestPoint(gravityMakerPosition);

            // Calculate the distance from the gravity maker to the closest point
            float distance = Vector3.Distance(gravityMakerPosition, closestPoint);

            // Check if the distance is less than or equal to the gravity range
            return distance <= gravityMaker.GravityRange;
        }
    }
}
