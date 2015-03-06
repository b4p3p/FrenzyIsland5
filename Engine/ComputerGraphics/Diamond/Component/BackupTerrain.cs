using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Engine.ComputerGraphics.Component
{
    public class BackupTerrain
    {
        public GameObject obj;
        public float[,] terrainData;
        public int startX;
        public int startZ;
        public int width;
        public int height;

        public BackupTerrain( GameObject obj, float[,] terrainData , 
                              int startX , int startZ , 
                              int width, int height )
        {
            this.obj = obj;
            this.terrainData = terrainData;
            this.startX = startX;
            this.startZ = startZ;
            this.width = width;
            this.height = height;
        }
    }
}
