using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Engine.ComputerGraphics.Bezier
{
    class BezierPath
    {
        public static Vector3[] GetBezierApproximation(Vector3[] controlPoints, int outputSegmentCount)
        {
            Vector3[] points = new Vector3[outputSegmentCount + 1];
            for (int i = 0; i <= outputSegmentCount; i++)
            {
                double t = (double)i / outputSegmentCount;
                points[i] = GetBezierPoint(t, controlPoints, 0, controlPoints.Length);
            }
            return points;
        }

        private static Vector3 GetBezierPoint(double t, Vector3[] controlPoints, int index, int count)
        {
            if (count == 1)
                return controlPoints[index];
            Vector3 P0 = GetBezierPoint(t, controlPoints, index, count - 1);
            Vector3 P1 = GetBezierPoint(t, controlPoints, index + 1, count - 1);
            return new Vector3 ( (float)( (1 - t) * P0.x + t * P1.x ) , 0f , (float) ( (1 - t) * P0.z + t * P1.z ));
        }
    }
}
