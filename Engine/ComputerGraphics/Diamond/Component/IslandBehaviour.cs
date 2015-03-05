using UnityEngine;
using System.Collections;
using terraformerIsland.DiamondAlghorithm;
using System;
using System.Linq;
using UnityEditor;
using System.Collections.Generic;
using Assets.Engine.ComputerGraphics.FloodFill;
using Assets.Engine.Other;

public class IslandBehaviour : MonoBehaviour
{
    [Range(0f, 1f)] public float HeightRatio = 0.1f;
    [Range(0.1f, 0.9f)] public float PercentageWater = 0.2f;

    public double Roughness = 0.2;
    public DiamondSeederIslandBehaviour diamondSeeder;
    public int TerrainSize = 512;
    
    private DiamondMatrix diamondMatrix;
    private GameObject water;
    private Terrain terrain;
    private TerrainData terrainData;
    private Vector3 sizeTerrain;            //500,50,500
    private int size_tData;                 //513

    private Scanner scannerWater = null;
    private Scanner scannerField = null;

    private float terrainHeight { get { return TerrainSize * HeightRatio; } }

    private float heightUnderWater { get { return -terrainHeight * PercentageWater; } }

    public Vector3 Center 
    { 
        get { 
            float x = TerrainSize / 2;
            float z = TerrainSize / 2;
            return new Vector3(x, GetRealHeight(z, x), z); 
        }
    }

	void Start () {

        terrain = GetComponent<Terrain>();
        terrainData = terrain.terrainData;
        size_tData = terrain.terrainData.heightmapHeight;
        sizeTerrain = terrain.terrainData.size;
        terrain.GetComponent<TerrainCollider>().terrainData = terrainData;


        Scan();
	}

    internal void NewIsland()
    {
        GC.Collect();

        DateTime start;
        DateTime end;

        SetTerrain();

        start = DateTime.Now;
        diamondMatrix = new DiamondMatrix(TerrainSize, Roughness, diamondSeeder.GetDiamonSeeder());
        diamondMatrix.Elaborate();
        end = DateTime.Now;

        Debug.Log("Elaborate in " + (end - start).TotalMilliseconds + "ms");

        start = DateTime.Now;

        double max = diamondMatrix.GetMaxValue();
        float[,] heights = diamondMatrix.ToFloatHeights();
        double max2 = (from float f in heights
                       select f).Max();

        terrainData.SetHeights(0, 0, new float[terrainData.heightmapWidth, terrainData.heightmapHeight]);
        terrainData.SetHeights(0, 0, heights);
        end = DateTime.Now;

        Scan();
    }

    private void Scan()
    {
        FloodFill floodWater = new FloodFill(terrainData.GetHeights(0, 0, TerrainSize + 1, TerrainSize + 1), PercentageWater, 0, 0);
        scannerWater = new Scanner(floodWater, this);

        FloodFill floodField = new FloodFill(terrainData.GetHeights(0, 0, TerrainSize + 1, TerrainSize + 1), PercentageWater, 256, 256);
        scannerField = new Scanner(floodField, this);
    }

    public float GetRealHeight(float row , float col)
    {
        //col is X of matrix
        //row is Z of matrix

        float maxHeight = sizeTerrain.y;
        RaycastHit hit;
        Ray ray = new Ray(new Vector3(col, maxHeight, row), Vector3.down);
        if (terrain.GetComponent<Collider>().Raycast(ray, out hit, 2.0f * maxHeight))
        {
            return hit.point.y;
        }
        return -1;
    }

    private void OnDrawGizmos()
    {
        if (terrainData == null) return;
        GizmosManager.DrawHeightMap(this);
        GizmosManager.DrawScanner(scannerWater, false);
        GizmosManager.DrawScanner(scannerField, false);
        GizmosManager.DrawScanner(scannerField, true);
    }
  
    private void SetTerrain()
    {
        water = GameObject.Find("Water");
        if ( water == null ) {
            //TODO load prefab
        }

        double sizeFactor = Math.Log(TerrainSize, 2);
        
        terrainData.size = new Vector3(TerrainSize, terrainHeight, TerrainSize);
        terrain.transform.position = new Vector3(0, heightUnderWater, 0);
        terrainData.heightmapResolution = TerrainSize + 1;
        water.transform.position = new Vector3(TerrainSize / 2, 0, TerrainSize / 2);
        water.transform.localScale = new Vector3(water.transform.position.x / 5 , 0 , water.transform.position.z / 5);

    }

    private Dictionary<int, float[,]> terrainBackup = new Dictionary<int,float[,]>();

    public void SetFlatTerrainAround(GameObject obj)
    {
        var renderer = obj.GetComponent<Renderer>();
        float size = renderer.bounds.size.x * 1.2f;

        #region debug

        Debug.Log("Size: " + size);

        Vector3 pos = obj.transform.position;

        Debug.DrawLine(pos - Vector3.up * 100 + Vector3.forward * size,
                       pos + Vector3.up * 100 + Vector3.left * size,
                       Color.red, 2);
        Debug.DrawLine(pos - Vector3.up * 100 + Vector3.forward * size,
                       pos + Vector3.up * 100 + Vector3.right * size,
                       Color.red, 2);
        Debug.DrawLine(pos - Vector3.up * 100 + Vector3.back * size,
                       pos + Vector3.up * 100 + Vector3.left * size,
                       Color.red, 2);
        Debug.DrawLine(pos - Vector3.up * 100 + Vector3.back * size,
                       pos + Vector3.up * 100 + Vector3.right * size,
                       Color.red, 2);

        #endregion debug

        int hmWidth = terrain.terrainData.heightmapWidth;
        int hmHeight = terrain.terrainData.heightmapHeight;

        Ray ray = new Ray(obj.transform.position + Vector3.up , Vector3.down);
        RaycastHit hit = new RaycastHit();
        
        if (Physics.Raycast(ray, out hit, 100))
        {
            Debug.Log("hit on " + hit.collider.gameObject.name);

            if (hit.collider.gameObject.name.Equals("Terrain"))
            {
                int startx = (int)(pos.x - size / 1.5f);
                int startz = (int)(pos.z - size / 1.5f);

                Vector3 posHit = hit.point;
                float[,] heights;
                try
                {
                    heights = terrain.terrainData.GetHeights(
                        startx, startz,
                        (int)(size * 1.5), (int)(size * 1.5)
                    );

                    if ( terrainBackup.ContainsKey(obj.GetHashCode() ) )
                    {
                        float[,] heightsBak = terrainBackup[obj.GetHashCode()];
                        terrainBackup[ obj.GetHashCode()] = heights;
                    }
                    else
                    {
                        terrainBackup.Add(obj.GetHashCode(), heights);
                    }

                }
                catch (Exception e)
                {
                    Debug.Log("Error size");
                    return;
                }

                //calculate avg
                var m = (from float s in heights
                         select s).Sum() / (heights.GetLength(0) * heights.GetLength(1) );

                Debug.Log(m);

                float height = heights[heights.GetLength(0) / 2,
                                       heights.GetLength(1) / 2];

                Debug.Log("Y: " + hit.point.y + "test: " + height);
                Debug.DrawLine(hit.point + Vector3.down * 100,
                               hit.point + Vector3.up * 100, Color.yellow, 2);

                for (int i = 0; i < heights.GetLength(0); i++)
                {
                    for (int j = 0; j < heights.GetLength(1); j++)
                    {
                        heights[i, j] = m;
                    }
                }

                terrain.terrainData.SetHeights(startx, startz, heights);
            }
            else
            {
                Debug.DrawLine(obj.transform.position + Vector3.up,
                               obj.transform.position + Vector3.down * 5,
                               Color.magenta , 5);
            }
        }
    }

    internal bool IsInMainIsland(float x , float z)
    {
        bool ris = scannerField.MainIsland.Contains(x, z);
        return ris;
    }

    internal bool IsOnLand(float x , float z)
    {
        float ris = GetRealHeight(z, x);
        return ris > 0;
    }

    
}
