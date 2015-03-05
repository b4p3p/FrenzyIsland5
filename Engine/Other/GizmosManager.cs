using Assets.Engine.ComputerGraphics.FloodFill;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEditor;
using UnityEngine;

namespace Assets.Engine.Other
{
    class GizmosManager
    {
        private static System.Random random = new System.Random();

        internal static void DrawHeightMap(IslandBehaviour island)
        {
            int TerrainSize = island.TerrainSize;
            DiamondSeederIslandBehaviour diamondSeeder = island.diamondSeeder;
            Terrain terrain = island.GetComponent<Terrain>();
            float[,] heights = terrain.terrainData.GetHeights(0, 0, TerrainSize +1, TerrainSize+1);

            //Debug.Log("Dim heights " + heights.GetLength(0) + "x" + heights.GetLength(1));
            //Debug.Log("height 0   0   " + terrain.terrainData.GetHeight(0, 0));
            //Debug.Log("height 255 255 " + terrain.terrainData.GetHeight(512, 512));
            //Debug.Log("TerrainSize    " + TerrainSize);

            GameObject chkHeightMap = GameObject.Find("chkHeighMap");
            if (chkHeightMap == null) return;
            UIToggle chkHeightMap_Toggle = chkHeightMap.GetComponent<UIToggle>();

            if (chkHeightMap_Toggle.value)
            {
                Gizmos.color = Color.red;

                for (int r = 0; r <= TerrainSize; r += diamondSeeder.TileSize)
                {
                    for (int c = 0; c <= TerrainSize; c += diamondSeeder.TileSize)
                    {
                        DrawSingleHeight(heights, island, terrain.terrainData, r, c);
                    }
                }
            }
        }

        private static void DrawSingleHeight(float[,] heights, IslandBehaviour island, TerrainData terrainData,  
                                             int row, int col)
        {
            //row is Z of matrix
            //col is X of matrix

            float height = terrainData.GetHeight(row, col);

            Gizmos.DrawLine(new Vector3(col, island.GetRealHeight(row, col), row),
                            new Vector3(col, island.GetRealHeight(row, col) + 20, row));

            Handles.Label(new Vector3(col + 1, island.GetRealHeight(row, col) + 22, row),
                                       "R: " + island.GetRealHeight(row, col).ToString() + "\n" +
                                       "H: " + height + "\n" +
                                       "T: " + heights[row,col]);
        }

        internal static void DrawScanner(Scanner scanner, bool OnlyMainIsland)
        {
            if (!OnlyMainIsland)
            {
                if (scanner == null) return;
                foreach (Shape s in scanner.ListShape)
                {
                    for (int i = 0; i < s.listVertex.Count - 1; i++)
                    {
                        Gizmos.color = SelectColor(s.listVertex.Count % 10);
                        if (Vector3.Distance(s.listVertex[i], s.listVertex[i + 1]) > 1.5) continue;
                        Gizmos.DrawLine(s.listVertex[i], s.listVertex[i + 1]);
                    }
                }
            }else
            {
                if (scanner == null || scanner.MainIsland == null) return;
                for (int i = 0; i < scanner.MainIsland.listVertex.Count - 1; i++)
                {
                    Gizmos.color = SelectColor(scanner.MainIsland.listVertex.Count % 10);
                    if (Vector3.Distance(scanner.MainIsland.listVertex[i], scanner.MainIsland.listVertex[i + 1]) > 1.5) continue;
                    Gizmos.DrawLine(scanner.MainIsland.listVertex[i], scanner.MainIsland.listVertex[i + 1]);
                }
            }
        }

        private static Color SelectColor ( int seed )
        {
            switch (seed)
            {
                case 0:
                    return Color.red;
                case 1:
                    return Color.gray;
                case 2:
                    return Color.blue;
                case 3:
                    return Color.cyan;
                case 4:
                    return Color.green;
                case 5:
                    return Color.magenta;
                case 6:
                    return Color.yellow;
                case 7:
                    return Color.green;
                case 8:
                    return Color.red;
                case 9:
                    return Color.white;

            }
            return Color.black;
        }
    }
}
