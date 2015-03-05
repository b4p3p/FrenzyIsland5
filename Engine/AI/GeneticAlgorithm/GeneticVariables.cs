using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.Engine.AI.GeneticAlgorithm
{
    public class GeneticVariables
    {
        public float MixingRatio;
        public float PercentageMutation;
        public int MaxMutation;
        public int MinMutation;

        public GeneticVariables()
        {

        }

        public override string ToString()
        {
            return "MixRatio: " + MixingRatio + " " + "PercentageMutation: " + PercentageMutation + " " +
                   "MaxMutation: " + MaxMutation + " " + "MinMutation: " + MinMutation; 
        }
    }
}
