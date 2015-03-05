using System.Collections.Generic;
using System.Linq;
using System.Text;
using sys = System;
using UnityEngine;


namespace Assets.Engine.Other
{
    internal class Randomize
    {
        internal static double NextDouble( double min , double max )
        {
            double r = Random.value * (max - min) + min;
            return r;
        }

        public static float Next()
        {
            return UnityEngine.Random.value;
        }

        internal static float NextFloat(float min, float max)
        {
            double r = Random.value * (max - min) + min;
            return (float)r;
        }

        internal static int NextInt(int min, int max)
        {
            float r = Random.value * (max - min) + min;
            return sys.Convert.ToInt32 ( r );
        }

        internal static bool Coin()
        {
            float coin = Random.value;
            return coin < 0.5 ? false : true;
        }
    }
}
