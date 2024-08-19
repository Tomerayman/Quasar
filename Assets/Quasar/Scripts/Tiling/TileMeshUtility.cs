using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Quasar.Tiling
{
    public static class TileMeshUtility
    {
        public static Mesh CreateSpaceTileMesh(SpaceTile tile)
        {
            Mesh mesh = new Mesh();
            List<Vector3> vertices = new List<Vector3>();
            List<int> indices = new List<int>();
            List<Vector2> uvs = new List<Vector2>();

            int width = tile.SpacePoints.GetLength(1) -1;
            int height = tile.SpacePoints.GetLength(0) -1;

            // Generate vertices and UVs
            for (int row = 0; row <= height; row++)
            {
                for (int col = 0; col <= width; col++)
                {
                    float xPos = col * tile.Interval;
                    float zPos = row * tile.Interval;
                    vertices.Add(new Vector3(xPos, 0, zPos));
                    uvs.Add(new Vector2(row, col));
                }
            }

            // Generate triangles
            for (int row = 0; row < height; row++)
            {
                for (int col = 0; col < width; col++)
                {
                    int current = row * (width + 1) + col;
                    int next = current + (width + 1);

                    indices.Add(current + 1);
                    indices.Add(current);
                    indices.Add(next);

                    indices.Add(current + 1);
                    indices.Add(next);
                    indices.Add(next + 1);
                }
            }

            // Assign vertices, uvs, and indices to the mesh
            mesh.SetVertices(vertices);
            mesh.SetUVs(0, uvs);
            mesh.SetTriangles(indices, 0);
            mesh.RecalculateNormals();
            return mesh;
        }
    }

}

