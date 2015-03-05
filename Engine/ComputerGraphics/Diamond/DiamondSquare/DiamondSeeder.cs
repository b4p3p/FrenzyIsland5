using Assets.Engine.Other;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using terraformerIsland.DiamondAlghorithm.Utils;

namespace terraformerIsland.DiamondAlghorithm
{
    public class DiamondSeeder
    {
        //private double index_sigma_x;
        //private double index_sigma_y;
        //private double index_mu_x;
        //private double index_mu_y;

        private double sigma_x;
        private double sigma_y;
        private double mu_x = 0;
        private double mu_y = 0;
        private double pearson = 0;
        private double max_normal;

        internal LinkedList<DiamondCell> listKeyCell;
        private int tileSize;
        private DiamondMatrix diamondMatrix;

        public DiamondSeeder(int tileSize ) 
        {
            //this.index_sigma_x =  index_sigma_x;
            //this.index_sigma_y = index_sigma_y;
            //this.index_mu_x = index_mu_x;
            //this.index_mu_y = index_mu_y;
            //this.pearson = pearson;

            this.tileSize = tileSize;

        }

        public void Seed(DiamondMatrix diamondMatrix)
        {
            this.diamondMatrix = diamondMatrix;

            //this.sigma_x = index_sigma_x * (diamondMatrix.Size - 1);
            //this.sigma_y = index_sigma_y * (diamondMatrix.Size - 1);
            //this.mu_y = index_mu_y * (diamondMatrix.Size - 1);
            //this.mu_x = index_mu_x * (diamondMatrix.Size - 1);
            //max_normal = multivariate_normal_distribution(mu_x, mu_y);

            listKeyCell = GetListKeyCell(diamondMatrix);
            foreach (DiamondCell item in listKeyCell) 
                item.SetValue( NewValue (item) );
        }

        private  LinkedList<DiamondCell> GetListKeyCell(DiamondMatrix diamondMatrix)
        {
            LinkedList<DiamondCell> ris = new LinkedList<DiamondCell>();
            for (int r = 0; r < diamondMatrix.Size; r+=tileSize)
                for (int c = 0; c < diamondMatrix.Size; c+=tileSize)                
                    ris.AddLast(diamondMatrix.GetCell(r, c));    
            return ris;
        }

        private List<DiamondCell> OrderByLevel(List<DiamondCell> list)
        {
            return (from DiamondCell d in list
                    orderby d.Level
                    select d).ToList<DiamondCell>();
        }

        private double NewValue( DiamondCell item )
        {
            double ris = 0;

            //TODO Tuning function
            double r = item.Radius;
            double max_r = diamondMatrix.MaxRadius;

            if (item.Level == 0) return 0;  //border 0

            double fortune = Randomize.NextDouble(0, diamondMatrix.Roughness);
            double radius = item.RadiusPercentageComplement * (1 - diamondMatrix.Roughness);

            radius = Math.Pow( (radius + diamondMatrix.Roughness), 4);
            
            ris = fortune + radius;
                  
            if (ris < 0) ris = 0;
            if (ris > 1) ris = 1;

            return ris; 

            // http://fooplot.com/#W3sidHlwZSI6MCwiZXEiOiIoKChhdGFuKHgtMSkpLyhwaS8yKSkpIiwiY29sb3IiOiIjMDAwMDAwIn0seyJ0eXBlIjoxMDAwLCJ3aW5kb3ciOlsiLTEuMDIxODMyNTM1OTk5OTg3NiIsIjE5LjI5MDY2NzQ2Mzk5OTk2NiIsIi02LjU2NzU3NjEyMDk5OTk4IiwiNS45MzI0MjM4Nzg5OTk5ODUiXX1d
            //(( ( atan ( x - 1 ) ) / (pi/2) ))
            //double probability =  Math.Atan ( 100 - item.RadiusPercentage - 1 ) / 
            //                    ( Math.PI / 2 );
            //if (probability < 0) probability = 0;
            //if (probability > 1) probability = 1;

            ////if (item.Level == 0) return 0;

            ////double fn = multivariate_normal_distribution(item.Row, item.Column);
            ////double prob = normalize_multivariate(fn);
            ////double dice = rnd.NextDouble();
            ////double compl = 1 - prob;

            ////if (prob > dice)
            ////{
            ////    //ris = rnd.NextDouble(); 
            ////    ris = dice;
            ////}
            ////else
            ////{
            ////    ris = prob;
            ////}

            //ris = item.Level * rnd.NextDouble();

            //double random = rnd.NextDouble();
            //ris = random * ( 1 - item.RadiusPercentage); 
        }

        public double multivariate_normal_distribution(double x , double y )
        {
            // http://en.wikipedia.org/wiki/Multivariate_normal_distribution#Properties

            double ground    = 1/ ( 2*Math.PI * sigma_x * sigma_y * Math.Sqrt(1 - Math.Pow(pearson,2)));
            double exponentA = -1 / (2 * (1- Math.Pow(pearson,2) ));
            double arg0 = Math.Pow(x - mu_x , 2 ) / Math.Pow( sigma_x , 2);
            double arg1 = Math.Pow(y - mu_y , 2 ) / Math.Pow( sigma_y , 2);
            double arg2 = (2 * pearson * ( x - mu_x) * ( y - mu_y) ) / (sigma_x * sigma_y);
            return ground * Math.Pow ( Math.E , exponentA * (arg0 + arg1 - arg2 ));
        }

        public double normalize_multivariate ( double val )
        {
            return val / max_normal;
        }
    }
}
