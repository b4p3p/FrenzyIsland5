using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace terraformerIsland.DiamondAlghorithm
{
    class Selector
    {
        private DiamondMatrix diamondMatrix;
        private int startRow;
        private int startCol;
        private int rows;
        private int cols;

        public Selector(DiamondMatrix diamondMatrix)
        {
            this.diamondMatrix = diamondMatrix;
            startRow = 0;
            startCol = 0;
            rows = diamondMatrix.Size;
            cols = diamondMatrix.Size;
        }

        public DiamondCell TopLeft
        {
            get
            {
                int r = startRow;
                int c = startCol;
                return diamondMatrix.GetMatrix()[r, c];
            }
        }

        public DiamondCell TopRight
        {
            get
            {
                int r = startRow;
                int c = startCol + cols;
                return diamondMatrix.GetMatrix()[r, c - 1];
            }
        }

        public DiamondCell BottomLeft
        {
            get
            {
                int r = startRow + rows;
                int c = startCol;
                return diamondMatrix.GetMatrix()[r - 1, c];
            }
        }

        public DiamondCell BottomRight
        {
            get
            {
                int r = startRow + rows;
                int c = startCol + cols;
                return diamondMatrix.GetMatrix()[r - 1, c - 1];
            }
        }

        public DiamondCell MiddleTop 
        { 
            get
            {
                int r = startRow;
                int c = (startCol + cols / 2);
                return diamondMatrix.GetMatrix()[r, c];
            }
        }

        public DiamondCell MiddleRight
        {
            get
            {
                int r = (startRow + rows / 2);
                int c = (startCol + cols);
                return diamondMatrix.GetMatrix()[r, c - 1];
            }
        }

        public DiamondCell MiddleBottom
        {
            get
            {
                int r = startRow + rows;
                int c = (startCol + cols / 2);
                return diamondMatrix.GetMatrix()[r - 1, c];
            }
        }

        public DiamondCell MiddleLeft
        {
            get
            {
                int r = (startRow + rows / 2);
                int c = startCol;
                return diamondMatrix.GetMatrix()[r, c];
            }
        }

        public DiamondCell Center
        {
            get
            {
                int r = (startRow + rows / 2);
                int c = (startCol + cols / 2);
                return diamondMatrix.GetMatrix()[r, c];
            }
        }

        internal void Set(int startRow, int startCol, int rows, int cols)
        {
            this.startRow = startRow;
            this.startCol = startCol;
            this.rows = rows;
            this.cols = cols;
        }
    }
}
