using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace terraformerIsland.DiamondAlghorithm
{
    public class DiamondCell
    {
        private DiamondMatrix diamondMatrix;

        public int Level {
            get {
                if (_level > -1) return _level;
                int center = diamondMatrix.Center;
                int X = Math.Abs(center - Column);
                int Y = Math.Abs(center - Row);
                int max = Math.Max(X, Y);
                _level = center - max;
                
                return _level; 
            }
            set { _level = value; }
        }
        private int _level = -1;

        public string Debug 
        { 
            get
            {
                //return Math.Min(Row, Column);
                return "";
            }
        }

        public double Value { get { return _value; } }
        private double _value = 0;

        public double Radius { 
            get
            {
                //eucledian
                double x = Math.Abs(diamondMatrix.Center - Row);
                double y = Math.Abs(diamondMatrix.Center - Column);
                double radius = Math.Sqrt(Math.Pow(x, 2) + Math.Pow(y, 2));
                return radius;
            }
        }

        public int Row { get; set; }

        public int Column { get; set; }

        public bool IsEmpty { get; set; }

        public DiamondCell(DiamondMatrix diamondMatrix, int r, int c, int v)
        {
            this.diamondMatrix = diamondMatrix;
            _value = v;
            Row = r;
            Column = c;
            IsEmpty = true;
        }

        public void SetValue( double v )
        {
            _value = v;
            IsEmpty = false;
        }

        public double RadiusPercentageComplement 
        { 
            get
            {
                return 1 - ( Radius / diamondMatrix.MaxRadius );
            }
        }
    }
}
