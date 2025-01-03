using System.Collections.Generic;
using UnityEngine;
using Quasar.Gravity;
using System;

namespace Quasar.Tiling
{
    [RequireComponent(typeof(MeshFilter), typeof(MeshRenderer), typeof(MeshCollider))]
    public class SpaceTileView : MonoBehaviour
    {
        [SerializeField, Range(1, 1000)]
        private int gridRows = 100, gridCols = 100;
        [SerializeField, Range(0.01f, 5)]
        private float gridInterval = 1;
        public Material LineMaterial;

        private MeshFilter meshFilter;
        private MeshRenderer meshRenderer;
        private MeshCollider meshCollider;
        private Dictionary<IGravityMaker, HashSet<SpacePoint>> currGravity =
            new Dictionary<IGravityMaker, HashSet<SpacePoint>>();

        public SpaceTile Tile { get; private set; }

        private void Awake()
        {
            Initialize();
            TilingController.Instance.RegisterSpaceTile(this);
        }
         
        private void OnValidate()
        {
            Initialize();
        }

        public void SetTileGridValues(int rows, int cols, float interval)
        {
            gridRows = rows;
            gridCols = cols;
            gridInterval = interval;
        }

        private void Initialize()
        {
            meshFilter = GetComponent<MeshFilter>();
            meshRenderer = GetComponent<MeshRenderer>();
            meshCollider = GetComponent<MeshCollider>();

            if (LineMaterial != null)
            {
                meshRenderer.material = LineMaterial;
            }
            else
            {
                Debug.LogError("LineMaterial is not assigned.");
            }
            Tile = new SpaceTile(transform.position, gridCols, gridRows, gridInterval);
            transform.position = Tile.RootPosition;
            meshFilter.sharedMesh = TileMeshUtility.CreateSpaceTileMesh(Tile);
        }

        public void AddGravityMakers(List<IGravityMaker> gravityMakers)
        {
            // Reset Gravity
            foreach (HashSet<SpacePoint> affectedSpacePoints in currGravity.Values)
            {
                UpdateMeshPoints(affectedSpacePoints, true);
                foreach (SpacePoint point in affectedSpacePoints)
                {
                    point.ResetGravity();
                }
            }
            currGravity.Clear();

            HashSet<SpacePoint> affectedPoints = new HashSet<SpacePoint>();

            // Add new Gravity makers and their points
            foreach(IGravityMaker gravityMaker in gravityMakers)
            {
                HashSet<SpacePoint> gmAffectedPoints = AddGravity(gravityMaker);
                currGravity[gravityMaker] = gmAffectedPoints;
                affectedPoints.UnionWith(gmAffectedPoints);
            }

            UpdateMeshPoints(affectedPoints, false);
        }

        private HashSet<SpacePoint> AddGravity(IGravityMaker gm)
        {
            // Get gravity maker position and range
            Vector3 gravityMakerPos = gm.ObjectTransform.position;
            Vector2 gravityCenter = new Vector2(gravityMakerPos.x, gravityMakerPos.z);
            int discreteRange = Mathf.RoundToInt(gm.GravityRange);

            // Get potentially affected points
            HashSet<SpacePoint> affectedPoints = Tile.GetPointsAround(gravityCenter, discreteRange);
            float maxRange = gm.GravityRange * Tile.Interval;
            

            //apply gravity to points acc. to range and strength of the source
            foreach (var point in affectedPoints)
            {
                Vector2 pointPos = XZ(Tile.GetPointPosition(point.Row, point.Col));
                float distanceToStar = Vector2.Distance(gravityCenter, pointPos);
                float distanceRatio = 1 - distanceToStar / maxRange;
                point.TakeGravity(Mathf.Pow(distanceRatio, 2) * gm.GravityForce);
            }

            return new HashSet<SpacePoint>(affectedPoints);

            Vector2 XZ(Vector3 v3)
                => new Vector2(v3.x, v3.z);
        }

        private void UpdateMeshPoints(HashSet<SpacePoint> points, bool undo)
        {
            Mesh mesh = meshFilter.mesh;
            Vector3[] vertices = mesh.vertices;

            // Update the vertices based on the gravity of the corresponding SpacePoints
            foreach (var point in points)
            {
                int vertexIdx = Tile.GetPointVertexIdx(point.Row, point.Col);
                vertices[vertexIdx] += point.Gravity * (undo ? Vector3.up : Vector3.down);
            }

            mesh.vertices = vertices;
            mesh.RecalculateNormals();
            mesh.RecalculateBounds();
            meshCollider.sharedMesh = mesh;
        }

        private Vector3 GetPointNormal(int row, int col)
            => meshFilter.mesh.normals[Tile.GetPointVertexIdx(row, col)];


        public Tuple<float, Vector3> GetYandNormalAtPos(Vector3 position)
        {
            // Get neighbor points of the parameter position
            HashSet<SpacePoint> neighborPoints = Tile.GetPointsAround(
                new Vector2(position.x, position.z), 1.5f / Tile.Interval);
            if (neighborPoints.Count == 0)
            {
                return null;
            }

            float weightedY = 0;
            Vector3 weightedNormal = Vector3.zero;
            float totalWeight = 0;

            foreach (SpacePoint point in neighborPoints)
            {
                Vector3 pointPos = Tile.GetPointPosition(point.Row, point.Col);
                float distance = Vector2.Distance(new Vector2(position.x, position.z), new Vector2(pointPos.x, pointPos.z));

                // Calculate weight based on the inverse distance (closer points have higher weight)
                float weight = 1.0f / Mathf.Max(distance, 0.0001f); // Avoid division by zero

                // Accumulate the weighted Y value and normals
                weightedY += pointPos.y * weight;
                weightedNormal += GetPointNormal(point.Row, point.Col) * weight;

                // Accumulate the total weight for normalization
                totalWeight += weight;
            }

            // Normalize the weighted averages
            float finalY = weightedY / totalWeight;
            Vector3 finalNormal = weightedNormal.normalized; // Normalize the final normal

            return new Tuple<float, Vector3>(finalY, finalNormal);
        }

    }
}
