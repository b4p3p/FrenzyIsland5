using Engine.ComputerGraphics.Component;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Engine.ComputerGraphics.FloodFill
{
    class Scanner
    {
        private FloodFill floodResult;
        private IslandBehaviour island;

        public List<Shape> ListShape;
        public Shape MainIsland;

        public LinkedList<Node> ListEntryPoint { get { return floodResult.list_entry_point; } }

        public Scanner(FloodFill floodResult, IslandBehaviour island)
        {
            this.island = island;
            this.floodResult = floodResult;
            ListShape = new List<Shape>();
            Scan();
        }

        private void Scan()
        {
            int cont = 0;
            while( ListEntryPoint.Count > 0 )
            {
                Node n = ListEntryPoint.First.Value;
                ListEntryPoint.RemoveFirst();

                if (floodResult.IsOutOfBounds(n.row, n.col)) continue;

                List<Vector3> listVertex = new List<Vector3>();
                
                Elaborate(listVertex, n);
                
                if ( listVertex.Count > 20)
                    ListShape.Add( new Shape(listVertex , island ));
                
                cont += listVertex.Count;
            }

            MainIsland = ListShape.OrderByDescending( s=> s.Vertex).First<Shape>();
            MainIsland.IsMainIsland = true;
            ListShape.Remove(MainIsland);
        }

        private void Elaborate(List<Vector3> listVertex , Node n)
        {
            listVertex.Add( new Vector3(n.col, island.GetRealHeight(n.row, n.col) + 5, n.row));
            floodResult.matrix_entry_point[n.row, n.col] = null;
            ConnectedNode(listVertex, n);
        }

        private void ConnectedNode(List<Vector3> listVertex , Node n)
        {
            int row;
            int col; 

            //top
            row = n.row + 1;    col = n.col;
            ConnectedNode(listVertex, row, col);
            //top right
            row = n.row + 1;    col = n.col + 1;
            ConnectedNode(listVertex, row, col);
            //right
            row = n.row;        col = n.col + 1;
            ConnectedNode(listVertex, row, col);
            //bottom right
            row = n.row - 1;      col = n.col + 1;
            ConnectedNode(listVertex, row, col);
            //bottom
            row = n.row - 1;    col = n.col;
            ConnectedNode(listVertex, row, col);
            //bottom left
            row = n.row - 1;    col = n.col - 1;
            ConnectedNode(listVertex, row, col);
            //left
            row = n.row;        col = n.col - 1;
            ConnectedNode(listVertex, row, col);
            //top left
            row = n.row + 1;    col = n.col - 1;
            ConnectedNode(listVertex, row, col);

        }

        private void ConnectedNode(List<Vector3> listVertex , int row , int col) {
            Node newEntryPoint = GetEntryPoint(row, col);
            if (newEntryPoint != null) {
                Elaborate(listVertex, newEntryPoint);
            }
        }

        private Node GetEntryPoint(int row , int col)
        {
            if (floodResult.IsOutOfBounds(row, col))              return null;
            if (floodResult.matrix_entry_point[row, col] == null) return null;
            if (floodResult.matrix_entry_point[row, col].CountEntryPoint == 0) return null;
            
            return floodResult.matrix_entry_point[row, col];
        }
    }
}
