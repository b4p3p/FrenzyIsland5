using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Vectrosity;
using Assets.Engine.ComputerGraphics.Bezier;
using Assets.Engine.SimpleArrow;

public class cmdTestArrow : MonoBehaviour {

    Terrain terrain;
    TerrainData tData;
    Vector3 center;
    Path path = null;

	void Start () {

        GroundQuiver.Init();

        terrain = GameObject.Find("Terrain").GetComponent<Terrain>();
        tData = terrain.terrainData;
        center = new Vector3( 513 / 2, 0, 513 / 2);

	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void OnClick()
    {
        IslandBehaviour island = GameObject.FindObjectOfType<IslandBehaviour>();

        //List<Vector3> listPoint1 = new List<Vector3>();
        //listPoint1.Add(new Vector3(center.x, 30, center.z - 50));
        //listPoint1.Add(new Vector3(center.x, 30, center.z + 50));
        //GroundQuiver.CreatePath(listPoint1.ToArray(), "test1 - vertical");

        //List<Vector3> listPoint2 = new List<Vector3>();
        //listPoint2.Add(new Vector3(center.x - 50, 60, center.z));
        //listPoint2.Add(new Vector3(center.x + 50, 60, center.z));
        //GroundQuiver.CreatePath(listPoint2.ToArray(), "test2 - Horizzontal");

        //List<Vector3> listPoint4 = new List<Vector3>();
        //listPoint4.Add(new Vector3(center.x + 50, 60, center.z + 50));
        //listPoint4.Add(new Vector3(center.x - 50, 60, center.z - 50));
        //GroundQuiver.CreatePath(listPoint4.ToArray(), "test - / - Down");

        //List<Vector3> lstBackSlashDown = new List<Vector3>();
        //lstBackSlashDown.Add(new Vector3(center.x - 50, 60, center.z + 50));
        //lstBackSlashDown.Add(new Vector3(center.x + 50, 60, center.z - 50));
        //GroundQuiver.CreatePath(lstBackSlashDown.ToArray(), "test - \\ - Down");

        path = GroundQuiver.CreateBezierPath(island, island.Center, "testPath");

    }

    //public void SendMessage(string message)
    //{
    //    CreateLine();
    //}

    //public void CreateLine()
    //{
    //    float[,] heights = terrain.terrainData.GetHeights(0, 0, tData.heightmapWidth, tData.heightmapHeight);

    //    heights[(int)center.x, (int)center.z] = 1;
    //    for (int r = -2; r <= 2; r++)
    //        for (int c = -2; c <= 2; c++)
    //        {
    //            if (r == 0 && c == 0) continue;
    //            heights[(int)center.x + r, (int)center.z + c] = 0.8f;
    //        }

    //    tData.SetHeights(0, 0, heights);

    //    List<Vector3> listPoint1 = new List<Vector3>();
    //    listPoint1.Add(new Vector3(1, 1, 1));
    //    listPoint1.Add(new Vector3(1, 2, 2));
    //    listPoint1.Add(new Vector3(1, 4, 3));
    //    listPoint1.Add(new Vector3(1, 1, 5));

    //    GroundQuiver.CreateArrow(listPoint1, "linea 1");
    //}
}
