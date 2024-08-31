using UnityEngine;
using System.Collections.Generic;
using System;
using System.Linq;
using System.Drawing;

namespace Quasar.Tiling
{
    [Serializable]
    public class SpaceTile
    {
        public Vector3 RootPosition { get; private set; } // Root position of spacePoint[0, 0]
        public float Interval { get; private set; } // Space between points in the XZ plane
        public SpacePoint[,] SpacePoints { get; private set; }


        public SpaceTile(Vector3 rootPosition, int width, int height, float interval)
        {
            RootPosition = rootPosition;
            Interval = interval;
            SpacePoints = new SpacePoint[height, width];

            InitializeSpacePoints();
        }

        private void InitializeSpacePoints()
        {
            for (int row = 0; row < SpacePoints.GetLength(0); row++)
            {
                for (int col = 0; col < SpacePoints.GetLength(1); col++)
                {
                    SpacePoints[row, col] = new SpacePoint(row, col, 0.0f); // Initialize with default gravity
                }
            }
        }

        public Vector3 GetPointPosition(int row, int col)
        {
            // Calculate the position of a point based on its row and column indices
            return new Vector3(
                RootPosition.x + col * Interval,
                RootPosition.y - SpacePoints[row, col].Gravity,
                RootPosition.z + row * Interval
            ); 
        }

        public int GetPointVertexIdx(int row, int col)
            => row * SpacePoints.GetLength(1) + col;

        /// <summary>
        /// Gives the world positions of the first point ([0, 0]) and last point ([rows -1, cols -1]) as bounds for the tile.
        /// </summary>
        /// <returns>Tuple of positions - item1 is first point pos, item2 is last. </returns>
        public Bounds GetTileBounds()
        {
            Vector3 last = GetPointPosition(SpacePoints.GetLength(0) - 1, SpacePoints.GetLength(1) - 1);

            // Create a bounding box for the tile
            Bounds tileBounds = new Bounds();
            tileBounds.SetMinMax(RootPosition, new Vector3(last.x, RootPosition.y, last.z));
            return tileBounds;
        }

        //public Vector3 GetPointNormal(int row, int col)
        //{
        //    // Ensure point is within bounds
        //    if (row < 0 || row >= SpacePoints.GetLength(0) || col < 0 || col >= SpacePoints.GetLength(1))
        //    {
        //        return Vector3.down; // Return a default normal if out of bounds
        //    }

        //    // Get the position of the current point
        //    Vector3 currentPoint = GetPointPosition(row, col);

        //    // Get the positions of neighboring points
        //    Vector3 left = (col > 0) ? GetPointPosition(row, col - 1) : currentPoint;
        //    Vector3 right = (col < SpacePoints.GetLength(1) - 1) ? GetPointPosition(row, col + 1) : currentPoint;
        //    Vector3 down = (row > 0) ? GetPointPosition(row - 1, col) : currentPoint;
        //    Vector3 up = (row < SpacePoints.GetLength(0) - 1) ? GetPointPosition(row + 1, col) : currentPoint;

        //    // Calculate vectors for cross product
        //    Vector3 vec1 = right - left;
        //    Vector3 vec2 = up - down;

        //    // Compute the NEGATIVE normal using the cross product
        //    Vector3 normal = -Vector3.Cross(vec1, vec2).normalized;

        //    return normal;
        //}

        public HashSet<SpacePoint> GetPointsAround(Vector2 xzPos, float range)
        {
            HashSet<SpacePoint> points = new HashSet<SpacePoint>();

            // Calculate the range in world units
            float rangeInWorldUnits = range * Interval;

            // Calculate the row and column indices of the xzPos
            int colStart = Mathf.Max(0, Mathf.FloorToInt((xzPos.x - RootPosition.x - rangeInWorldUnits) / Interval));
            int colEnd = Mathf.Min(SpacePoints.GetLength(1) - 1, Mathf.CeilToInt((xzPos.x - RootPosition.x + rangeInWorldUnits) / Interval));

            int rowStart = Mathf.Max(0, Mathf.FloorToInt((xzPos.y - RootPosition.z - rangeInWorldUnits) / Interval));
            int rowEnd = Mathf.Min(SpacePoints.GetLength(0) - 1, Mathf.CeilToInt((xzPos.y - RootPosition.z + rangeInWorldUnits) / Interval));

            for (int row = rowStart; row <= rowEnd; row++)
            {
                for (int col = colStart; col <= colEnd; col++)
                {
                    // Check if the point is within the circular range
                    Vector2 pointXZ = new Vector2(GetPointPosition(row, col).x, GetPointPosition(row, col).z);
                    if (Vector2.Distance(xzPos, pointXZ) <= rangeInWorldUnits)
                    {
                        points.Add(SpacePoints[row, col]);
                    }
                }
            }

            return points;
        }

        //public Tuple<Vector3, Vector3> GetWorldPosAndNormalAt(Vector2 xzPos)
        //{
        //    List<SpacePoint> neighborPoints = GetPointsAround(xzPos, 1);
            
        //    if (neighborPoints.Count == 0)
        //    {
        //        return new Tuple<Vector3, Vector3>(new Vector3(xzPos.x, 0, xzPos.y), Vector3.up);
        //    }

        //    float yVal = neighborPoints.Select(p => GetPointPosition(p.Row, p.Col).y).Average();
        //    Vector3 pos = new Vector3(xzPos.x, yVal, xzPos.y);
        //    Vector3 normal = Vector3.zero;
        //    neighborPoints.ForEach(p => normal += GetPointNormal(p.Row, p.Col)); 
        //    normal /= neighborPoints.Count;
        //    return new Tuple<Vector3, Vector3>(pos, normal);
        //}
    }

}