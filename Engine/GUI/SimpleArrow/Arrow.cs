using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using Vectrosity;

namespace Assets.Engine.SimpleArrow
{

    public class Arrow
    {
        public VectorLine pathLine;
        
        private List<Vector3> listPoint;
        private GameObject container;
        
        internal Arrow(List<Vector3> listPoint, string name)
        {
            Material material = Resources.LoadAssetAtPath<Material>("Assets/Engine/GUI/SimpleArrow/Material/Arrow");
            VectorLine.canvas3D.name = "LineContainer3D";

            bool loop = false; //issue with loop = true
            pathLine = new VectorLine(name, new Vector3[listPoint.Count], material, 2.0f, LineType.Continuous);
            pathLine.MakeSpline(listPoint.ToArray(), listPoint.Count - 1, loop);
            pathLine.Draw3D();
            pathLine.Draw3DAuto();
            
        }

    }

}
