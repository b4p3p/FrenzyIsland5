using Assets.Engine.ComputerGraphics.Bezier;
using Assets.Engine.Other;
using Engine.ComputerGraphics.Component;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Vectrosity;


namespace Assets.Engine.SimpleArrow
{
    public class GroundQuiver
    {
        public static List<Path> ListPath = new List<Path>();

        public static void Init()
        {
            VectorLine.canvas3D.name = "LineContainer3D";
        }

        public static Path CreateBezierPath(IslandBehaviour island, Vector3 startPos, 
                                            string name = "undefined", int pointLenght = 4,
                                            float x_range = 50, float z_range = 50, float step = 0.2f)
        {          
            float terrainSize = island.TerrainSize;
            Vector3[] points = new Vector3[(int)pointLenght];
            float x, z;

            points[0] = new Vector3(startPos.x, 0,
                                    startPos.z);
            for (int i = 1; i < pointLenght; i++)
            {
                do
                {
                    x = Randomize.NextFloat(startPos.x,
                                            startPos.x + x_range);
                    z = Randomize.NextFloat(startPos.z - z_range,
                                            startPos.z + z_range);

                } while (island.IsOnLand(x, z) == false ||
                         island.IsInMainIsland(x, z) == false);
                points[i] = new Vector3(x, 0, z);
            }
            Vector3[] bezier = BezierPath.GetBezierApproximation(points, 256);
            Path newArrow = GroundQuiver.CreatePath(bezier, name);
            newArrow.CreateCheckPoint(step);
            return newArrow;
        }

        internal static bool GoOnNextCheckPoint(Path path)
        {

            path.LastCheckPoint = new Vector3(path.FirstCheckPoint.x, 
                                              path.FirstCheckPoint.y, 
                                              path.FirstCheckPoint.z);

            path.CheckPoints.RemoveFirst();

            if (path.CheckPoints.Count == 0)
            {
                GroundQuiver.RemovePath(path);

                return true;
            }

            return false;
        }
        
        private static Path CreatePath(Vector3[] listPoint, string name)
        {
            float fixHeight = 0.2f;
            List<Vector3> risLis = new List<Vector3>();
            IslandBehaviour island = GameObject.FindObjectOfType<IslandBehaviour>();

            for (int i = 0; i < listPoint.Length - 1; i++)
            {
                //y = mx + b

                Vector3 startPoint = listPoint[i];
                Vector3 endPoint = listPoint[i+1];
                Vector3 target = endPoint - startPoint;
                float m = target.z / target.x;

                if ( m == 0) // y=k - horizzontal
                {
                    for (float x = Math.Min(startPoint.x, endPoint.x); x <= Math.Max(startPoint.x, endPoint.x); x++)
                    {
                        risLis.Add(new Vector3(x , island.GetRealHeight(startPoint.z, x) + fixHeight, startPoint.z));
                    }
                } else
                if ( m == Mathf.Infinity || m == Mathf.NegativeInfinity) //x=k - vertical
                {
                    for (float z = Math.Min(startPoint.z, endPoint.z); z <= Math.Max(startPoint.z, endPoint.z); z++)
                    {
                        risLis.Add(new Vector3(startPoint.x, island.GetRealHeight(z, startPoint.x) + fixHeight, z));
                    }
                } else
                {
                    float distance = Vector3.Distance(startPoint, endPoint);
                    float x = startPoint.x;
                    float z = startPoint.z;
                    for (int d = 0; d < distance; d++)
                    {
                        Vector3 v = new Vector3(x, island.GetRealHeight(z, x) + fixHeight, z);
                        risLis.Add(v);
                        if (startPoint.x < endPoint.x)
                        {
                            x += Mathf.Cos(Mathf.Atan(m));
                            z += Mathf.Sin(Mathf.Atan(m));
                        }else
                        {
                            x -= Mathf.Cos(Mathf.Atan(m));
                            z -= Mathf.Sin(Mathf.Atan(m));
                        }
                    }
                }

            }

            Arrow arrow = new Arrow(risLis, name);
            Path path = new Path(arrow);
            ListPath.Add(path);
            return path;

        }

        public static void RemovePath(Path path)
        {
            VectorLine.Destroy(ref path.Arrow.pathLine);
            ListPath.Remove(path);
        }

        private static Material GetMaterial()
        {
            Material material = Resources.LoadAssetAtPath<Material>("Assets/Engine/SimpleArrow/Material/Arrow");
            return material;
        }

        
        
    }
}
