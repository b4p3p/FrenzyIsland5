using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.Engine.ComputerGraphics.FloodFill
{
    class Node
    {
        public int row { get; set; }
        public int col { get; set; }
        public float Value { get; set; }
        public List<Node> ListEntryPointParents { get; set; }
        public Node Parent { get; set; }
        public int CountEntryPoint { get { return countEntryPoint; } }

        private int countEntryPoint = 0;

        public Node(int row, int col, Node parent = null )
        {
            this.row = row;
            this.col = col;
            this.Parent = parent;
            ListEntryPointParents = new List<Node>();
        }

        internal void AddEntryPoint(Node parent)
        {
            ListEntryPointParents.Add(parent);
            countEntryPoint++;
        }

        internal Node Left()
        {
            return new Node(row, col -1, this);
        }

        internal Node Bottom()
        {
            return new Node(row - 1, col, this);
        }

        internal Node Right()
        {
            return new Node(row, col +1, this);
        }

        internal Node Top()
        {
            return new Node(row + 1, col, this);
        }
    }
}
