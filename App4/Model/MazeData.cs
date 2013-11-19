using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;

namespace App4.Model
{
    public class MazeModel
    {
        private bool[] edges;

        public int HorizontalVertex { get; private set; }

        public int VerticalVertex { get; private set; }

        public int row;
        public int column;
        public int[][] mapArray;

        public MazeModel(int horizontalvertex, int verticalvertex)
        {
            column = horizontalvertex * 2 + 1;
            row = verticalvertex * 2 + 1;
            HorizontalVertex = horizontalvertex;
            VerticalVertex = verticalvertex;
            edges = Enumerable
                .Range(0, horizontalvertex * (verticalvertex * 2 - 1) - 1)
                .Select(x =>
                    x % horizontalvertex != horizontalvertex - 1 ||
                    x / horizontalvertex % 2 != 0
                ).ToArray();
            generateMaze();
            mapArray = new int[row][];

            for (int i = 0; i < row; i++)
            {
                mapArray[i] = new int[column];
            }

            for (int i = 0; i < row; i++)
            {
                mapArray[i][column - 1] = -1;
                mapArray[i][0] = -1;
            }

            for (int j = 0; j < column; j++)
            {
                mapArray[row - 1][j] = -1;
                mapArray[0][j] = -1;
            }
        }
        private bool getEdgeValue(int edgeindex)
        {
            if (edgeindex >= edges.Length || edgeindex < 0)
            {
                return false;
            }
            else
            {
                return edges[edgeindex];
            }
            //try
            //{
            //    return edges[edgeindex];
            //}
            //catch
            //{
            //    return false;
            //}
        }

        private void setEdgeValue(int edgeindex, bool value)
        {
            if (edgeindex >= edges.Length || edgeindex < 0)
            {
            }
            else
            {
                edges[edgeindex] = value;
            }
            //try
            //{
            //    edges[edgeindex] = value;
            //}
            //catch { }
        }

        private int nodeToEdgeIndex(int x, int y, int dir)
        {
            if (x == 0 && dir == 1) return -1;
            if (x % HorizontalVertex == HorizontalVertex - 1 && dir == 2) return -1;
            if (y == 0 && dir == 0) return -1;
            if (y >= HorizontalVertex * (VerticalVertex - 1) && dir == 3) return -1;

            return (y * 2 + ((dir == 1 || dir == 2) ? 0 : (dir == 0 ? -1 : 1))) * HorizontalVertex + x - (dir == 1 ? 1 : 0);
        }

        private int nodeindexToEdgeIndex(int nodeindex, int dir)
        {
            int x;
            int y;
            nodeIndexToNode(nodeindex, out x, out y);
            return nodeToEdgeIndex(x, y, dir);
        }

        private void nodeIndexToNode(int nodeindex, out int x, out int y)
        {
            x = nodeindex % HorizontalVertex;
            y = nodeindex / HorizontalVertex;
        }

        private int nodeToNodeIndex(int x, int y)
        {
            return y * HorizontalVertex + x;
        }

        private void edgeIndexToNodeIndex(int edgeindex, out int node1, out int node2)
        {
            int x1 = 0, y1 = 0, x2 = 0, y2 = 0;
            if (edgeindex / HorizontalVertex % 2 != 0)
            {
                x1 = x2 = edgeindex % HorizontalVertex;
                y1 = edgeindex / (HorizontalVertex * 2);
                y2 = y1 + 1;
            }
            else
            {
                x1 = edgeindex % HorizontalVertex;
                x2 = x1 + 1;
                y1 = y2 = edgeindex / (HorizontalVertex * 2);
            }
            node1 = nodeToNodeIndex(x1, y1);
            node2 = nodeToNodeIndex(x2, y2);
        }

        private int edgeIndexToNodeIndex(int edgeindex, int node)
        {
            int node1, node2;
            edgeIndexToNodeIndex(edgeindex, out node1, out node2);
            return node1 == node ? node2 : node1;
        }

        private IEnumerable<int> getNodeEdges(int nodeIndex, List<int> blocklist)
        {
            return Enumerable
                .Range(0, 4)
                .Select(x => nodeindexToEdgeIndex(nodeIndex, x))
                .Where(x => getEdgeValue(x) && !blocklist.Contains(x));
        }

        private void generateMaze()
        {
            List<int> blockEdges = new List<int>();
            while (blockEdges.Count < HorizontalVertex * VerticalVertex - 1)
            {
                var nodes = Enumerable.Range(0, HorizontalVertex * VerticalVertex)
                    .Where(x => getNodeEdges(x, blockEdges).Count() > 1).ToList()
                    .Select(x => new { k = Guid.NewGuid(), v = x })
                    .OrderBy(x => x.k)
                    .Select(x => x.v).ToList();
                if (nodes.Count == 0) return;
                int node = nodes.First();
                var tedges = getNodeEdges(node, blockEdges).ToList()
                    .Select(x => new { k = Guid.NewGuid(), v = x })
                    .OrderBy(x => x.k)
                    .Select(x => x.v).ToList();
                if (tedges.Count == 0) return;
                int edge = tedges.First();
                var edgesbackup = edges.ToArray();
                setEdgeValue(edge, false);
                if (!testMaze())
                {
                    blockEdges.Add(edge);
                    edges = edgesbackup;
                }
            }
        }

        private bool testMaze()
        {
            var found = new List<int>() { 0 };
            findNode(0, found);
            return found.Count() == HorizontalVertex * VerticalVertex;
        }

        private void findNode(int nodeindex, List<int> found)
        {
            var nextNodes = getNodeEdges(nodeindex, new List<int>())
                .Select(x => edgeIndexToNodeIndex(x, nodeindex))
                .Where(x => !found.Contains(x))
                .ToList();
            foreach (int item in nextNodes)
            {
                found.Add(item);
            }
            foreach (int item in nextNodes)
            {
                findNode(item, found);
            }
        }
        public void setArray()
        {
            for (int i = 0; i < VerticalVertex; i++)
            {
                for (int j = 0; j < HorizontalVertex; j++)
                {
                    //Console.Write("X");
                    if (j != HorizontalVertex - 1)
                    {
                        mapArray[i * 2 + 1][j * 2 + 1] = 0;
                        if (getEdgeValue(nodeToEdgeIndex(j, i, 2)))
                        {
                            mapArray[i * 2 + 1][j * 2 + 2] = 0;
                        }
                        else
                        {
                            mapArray[i * 2 + 1][j * 2 + 2] = 1;
                        }
                        //Console.Write(getEdgeValue(nodeToEdgeIndex(j, i, 2)) ? "-" : " ");
                    }
                }
                //Console.WriteLine();
                for (int j = 0; j < HorizontalVertex; j++)
                {
                    if (i != VerticalVertex - 1)
                    {
                        mapArray[i * 2 + 1][j * 2 + 1] = 0;
                        if (getEdgeValue(nodeToEdgeIndex(j, i, 3)))
                        {
                            mapArray[i * 2 + 2][j * 2 + 1] = 0;
                        }
                        else
                        {
                            mapArray[i * 2 + 2][j * 2 + 1] = 1;
                        }
                        mapArray[i * 2 + 2][j * 2 + 2] = 1;
                        //Console.Write(getEdgeValue(nodeToEdgeIndex(j, i, 3)) ? "|" : " ");
                        //Console.Write(" ");
                    }
                }
                //Console.WriteLine();
            }
        }

        //public void DisplayNum()
        //{
        //    for (int i = 0; i < row; i++)
        //    {
        //        for (int j = 0; j < column; j++)
        //        {
        //            Console.Write(mapArray[i][j] + " ");
        //        }
        //        Console.WriteLine();
        //    }
        //}
    }
}