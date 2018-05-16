using System;
using System.Collections.Generic;
using System.Text;

namespace Graphsuche
{
    public class Graph
    {
        private readonly uint vertices = 0;
        private readonly List<uint>[] adj;

        public Graph(uint vertices)
        {
            this.vertices = vertices;
            
            adj = new List<uint>[vertices];

            for (int v = 0; v < vertices; v++)
            {
                adj[v] = new List<uint>();
            }
        }

        public Graph(List<uint>[] edges)
        {
            //retrieve the number of node as the maximum index used in an endge.

            foreach (var edge in edges)
            {
                if (edge.Count != 2)
                {
                    throw new ArgumentException("Not all entries in the given edge list were tuples");
                }

                if (edge[0] > this.vertices)
                {
                    this.vertices = edge[0];
                }
                
                if (edge[1] > this.vertices)
                {
                    this.vertices = edge[1];
                }
            }

            vertices++;
            
            adj = new List<uint>[vertices];
            for (int v = 0; v < vertices; v++)
            {
                adj[v] = new List<uint>();
            }

            foreach (var edge in edges)
            {
                AddEdge(edge[0], edge[1]);
            }
        }

        public void AddEdge(uint edge1, uint edge2)
        {
            if (!adj[edge1].Contains(edge2))
            {
                adj[edge1].Add(edge2);
            }

            if (!adj[edge2].Contains(edge1))
            {
                adj[edge2].Add(edge1);
            }
        }
    }
}