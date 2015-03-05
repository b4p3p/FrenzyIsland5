using Assets.Engine.Other;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

//http://www.playfuljs.com/realistic-terrain-in-130-lines/

namespace terraformerIsland.DiamondAlghorithm
{
    public class DiamondMatrix
    {
        public DiamondMatrix(int size, double roughness, 
                             DiamondSeeder seeder)
        {
            this.roughness = roughness;
            this.diamondSeeder = seeder;

            Selector = new Selector(this);
            Size = size + 1;
            matrix = new DiamondCell[Size, Size];

            InitDiamondCell();

            seeder.Seed(this);

        }

        public double Roughness { get { return roughness; } }
        private double roughness;

        private DiamondSeeder diamondSeeder;

        internal Selector Selector { get; set; }

        private DiamondCell[,] matrix { get; set; }

        public double SumHeight { get { return sumHeight; } }
        private double sumHeight = 0;
        
        public int Size { get; set; }

        public int Center { 
            get
            {
                return (Size - 1) / 2;
            }
        }

        private void InitDiamondCell()
        {
            for (int r = 0; r < Size; r++)
            {
                for (int c = 0; c < Size; c++)
                {
                    matrix[r,c] = new DiamondCell(this,r,c,0);
                }
            }
        }

        public void SetValue(int r , int c, double v)
        {
            matrix[r, c].SetValue(v);
        }

        public DiamondCell[,] GetMatrix()
        {
            return matrix;
        }

        public DiamondCell GetCell(int r, int c)
        {
            return matrix[r, c];
        }

        public void Elaborate()
        {
            diamondSquareAlgorithm(this);
        }

        private void diamondSquareAlgorithm(DiamondMatrix matrix)
        {
            int startCol = 0;
            int startRow = 0;
            int cols = matrix.Size;
            int rows = matrix.Size;

            apply(matrix, startRow, startCol, rows, cols);
        }

        private void apply(DiamondMatrix matrix, 
            int startRow, int startCol,
            int rows, int cols)
        {
            if (rows == 0 || cols == 0) return;
            
            Elaborate(matrix, startRow, startCol, rows, cols);

            int subMatrixRows = rows / 2 + 1;
            int subMatrixCols = cols / 2 + 1;

            if (subMatrixRows < 3 && subMatrixCols < 3 ) return;

            apply(matrix, startRow, startCol, subMatrixRows, subMatrixCols);

            apply(matrix, startRow + subMatrixRows - 1, startCol, subMatrixRows, subMatrixCols);

            apply(matrix, startRow, startCol + subMatrixCols - 1 , subMatrixRows, subMatrixCols);

            apply(matrix, startRow + subMatrixRows - 1, startCol + subMatrixCols - 1, subMatrixRows, subMatrixCols);
 
        }

        private void Elaborate(DiamondMatrix matrix, int startRow, int startCol, int rows, int cols)
        {
            matrix.Selector.Set(startRow, startCol, rows, cols);

            //Control
            //if (matrix.Selector.TopLeft.IsEmpty) matrix.Selector.TopLeft.SetValue(0);
            //if (matrix.Selector.TopRight.IsEmpty) matrix.Selector.TopRight.SetValue(0);
            //if (matrix.Selector.BottomLeft.IsEmpty)matrix.Selector.BottomLeft.SetValue(0);
            //if (matrix.Selector.BottomRight.IsEmpty)matrix.Selector.BottomRight.SetValue(0);
            
            //calculate
            if (matrix.Selector.MiddleTop.IsEmpty)
            {
                double val = (matrix.Selector.TopLeft.Value + matrix.Selector.TopRight.Value) / 2;
                val = Noise(val, rows);
                matrix.Selector.MiddleTop.SetValue( Normalize(val) );
            }
            
            if (matrix.Selector.MiddleRight.IsEmpty)
            {
                double val = ((matrix.Selector.TopRight.Value + matrix.Selector.BottomRight.Value) / 2);
                val = Noise(val, rows);
                matrix.Selector.MiddleRight.SetValue( Normalize ( val ) );
            }
                
            if (matrix.Selector.MiddleBottom.IsEmpty)
            {
                double val = ((matrix.Selector.BottomLeft.Value + matrix.Selector.BottomRight.Value) / 2);
                val = Noise(val, rows);
                matrix.Selector.MiddleBottom.SetValue( Normalize ( val ) );
            }

            if (matrix.Selector.MiddleLeft.IsEmpty)
            {
                double val = (matrix.Selector.TopLeft.Value + matrix.Selector.BottomLeft.Value) / 2;
                val = Noise(val, rows);
                matrix.Selector.MiddleLeft.SetValue( Normalize ( val) );
            }
            
            double avg = ((matrix.Selector.MiddleBottom.Value +
                           matrix.Selector.MiddleTop.Value +
                           matrix.Selector.MiddleLeft.Value +
                           matrix.Selector.MiddleRight.Value) / 4);

            double newValue = Normalize( Noise( avg ,  rows ) );

            //TODO parametrizzare la funzione di rumore sul centro
            if (matrix.Selector.Center.IsEmpty)
                matrix.Selector.Center.SetValue( newValue );

            sumHeight += matrix.Selector.Center.Value;
        }

        private double Noise(double value, double size)
        {
            //max noise in max size
            double sizePercentage = size / this.Size;

            if (size == 3) return value;

            double noise = Randomize.NextDouble(0, sizePercentage);

            if (Randomize.Coin()) value += noise; else value -= noise;

            return value;

            //double scale = roughness * rows;
            //return rnd.NextDouble() * scale * 2 - scale;

            //double scale = (rows * roughness) / 3;                          //0.1
            //double val = (((rnd.NextDouble() * 2) - 1) / 5) * scale;
            //return val;
            
            // [-0.1;0.1]
            //return ((rnd.NextDouble() * 2) - 1) / 200;
            //return ((rnd.NextDouble() * 2) - 1) / 10;

            
            //return 0;
        }

        //private double NoiseBorder(double value , double rows)
        //{
        //    return Noise(value, rows);
        //}

        private double Normalize(double v)
        {
            //return v;

            if (v > 1) return 1;
            if (v < 0) return 0;
            return v;
        }

        internal byte[] ToArray255()
        {
            double maxValue = GetMaxValue();
            byte[] ris = new byte[ Size * Size ];
            int cont = 0;

            for (int r = 0; r < Size; r++)
            {
                for (int c = 0; c < Size; c++)
                {
                    ris[cont] = Convert.ToByte ( 255 * matrix[r, c].Value );
                    cont++;
                }
            }

            return ris;
        
        }

        internal float[,] ToFloatHeights()
        {
            float[,] ris = new float[Size, Size];

            for (int r = 0; r < Size; r++)
            {
                for (int c = 0; c < Size; c++)
                {
                    ris[r,c] = Convert.ToSingle( matrix[r, c].Value );
                }
            }

            return ris;
        }

        public double GetMaxValue()
        {
            return (from DiamondCell item in matrix
                    select item.Value).Max();
        }

        public double MaxRadius 
        { 
            get
            {
                return matrix[0, 0].Radius;
            }
        }

        public override string ToString()
        {
            StringBuilder str = new StringBuilder();

            AppendLineSeparator(str , Size);
            for (int r = 0; r < Size; r++)
            {
                for (int c = 0; c < Size; c++)
                {
                    str.Append( " | " + matrix[r, c].Value.ToString("0.000") + " | " );
                }
                str.AppendLine();
                AppendLineSeparator(str, Size);
            }
            return str.ToString();
        }

        private void AppendLineSeparator( StringBuilder str , int size )
        {
            str.AppendLine();
            str.AppendLine();
            for (int i = 0; i < size; i++)
            {
                str.Append("-----------");
            }
            str.AppendLine();
            str.AppendLine();
        }

        internal string ToStringSeeders()
        {
            int startRow = 0;
            StringBuilder str = new StringBuilder();
            foreach (DiamondCell item in diamondSeeder.listKeyCell)
	        {
                if ( item.Row != startRow )
                {
                    startRow = item.Row;
                    str.AppendLine();
                }
                str.Append( "|" + item.Value.ToString("0.00000000") + "|" );
	        }
            return str.ToString();
        }
    }
}



/// <summary>
/// InitDiamondCell for SubMatrix
/// </summary>
/// <param name="diamondMatrix"></param>
/// <param name="rowStart"></param>
/// <param name="rowEnd"></param>
/// <param name="colStart"></param>
/// <param name="colEnd"></param>
//private void InitDiamondCell(DiamondMatrix diamondMatrix, int rowStart, int rowEnd, int colStart, int colEnd)
//{
//    int row = rowStart;
//    for (int r = 0; r < Size; r++)
//    {
//        int col = colStart;
//        for (int c = 0; c < Size; c++)
//        {
//            matrix[r, c] = diamondMatrix.GetCell(row, col);
//            col++;
//        }
//        row++;
//    }
//}
