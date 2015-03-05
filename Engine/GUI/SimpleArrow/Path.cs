using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Engine.SimpleArrow
{
    public class Path
    {
        public Path()
        {
            LastCheckPoint = Vector3.zero;
        }

        public Path(Arrow arrow)
        {
            this.arrow = arrow;
            LastCheckPoint = Vector3.zero;
        }

        public Vector3 LastCheckPoint;
        public LinkedList<Vector3> CheckPoints = null;

        public Arrow Arrow
        {
            get { return arrow; }
            set
            {
                if (arrow != null)
                {
                    GroundQuiver.RemovePath(this);
                }
                arrow = value;
            }
        }
        private Arrow arrow = null;

        public void CreateCheckPoint(float step)
        {
            CheckPoints = new LinkedList<Vector3>();

            for (float dist = 0.0f; dist <= 1.0; dist += step)
            {
                CheckPoints.AddLast(arrow.pathLine.GetPoint3D01(dist));
            }
        }

        public Vector3 FirstCheckPoint
        {
            get
            {
                if (CheckPoints.Count == 0) return Vector3.zero;
                return CheckPoints.First.Value;
            }
        }
    }
}
