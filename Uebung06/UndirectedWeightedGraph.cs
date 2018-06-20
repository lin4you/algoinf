//**************************************
// undirected weighted Graphs
// implementation via adjacency lists
//**************************************

using System;
using System.Collections.Generic;

namespace UndirectedWeightedGraph
{
	public class Edge : IComparable		// Can be sorted according to the weight
	{
		private int v, w;
		private double weight; 

		// creates an edge
		public Edge(int v, int w, double weight)
		{
			this.v = v;
			this.w = w;
			this.weight = weight;
		}
		// changes the weights of an edge
		public void SetWeight(double weight)
		{
			this.weight = weight;
		}
		
		// values of an edge
		public int Other(int v)
		{
			if (this.v == v) return w;
			return this.v;
		}
		public int Some()
		{
			return v;
		}
		public double Weight()
		{
			return weight;
		}
		// Output
		public void Output()
		{
			System.Console.WriteLine(v + "-" + w + "-" + weight);
		}
		// Comparison, implementation of Interface IComparable
		public int CompareTo(Object obj)
		{
			if (obj is Edge)
			{
				Edge other = (Edge) obj;
				if (this.weight < other.Weight()) return -1;
				if (this.weight == other.Weight()) return 0;
				if (this.weight > other.Weight()) return 1;
			}
			throw new ArgumentException("Object is not an Edge.");
		}
	}

	public class Graph
	{
		private int V;
		private List<Edge>[] edges;

		// Creates an empty graph with V nodes
		public Graph(int V)
		{
			if (V < 0) throw new ArgumentException("Number of vertices should not be negative\n");
			this.V = V;
			edges = new List<Edge>[V];
			for (int v=0; v<V; v++)
				edges[v] = new List<Edge>();
		}

		// Prints the graph
		public void Display()
		{
			for (int v = 0; v < this.V; v++)
			{
				System.Console.Write(v + ": ");
				for (int e = 0; e < this.edges[v].Count; e++)
				{
					System.Console.Write(edges[v][e].Other(v) + " (" + edges[v][e].Weight() + ") ");
				}
				System.Console.WriteLine();
			}
		}
		
		// Adds an edge by means of the values
		public void AddEdge(int v, int w, double value)
		{
			if (Edge (v, w) == null)
			{
				Edge edge = new Edge (v, w, value);
				edges [v].Add (edge);
				edges [w].Add (edge);
			}
		}
		
		// Returns number of nodes
		public int Nodes()
		{
			return V;
		}
		
		// Returns first index
		public int Base()
		{
			return 0;
		}
		
		// Returns all edges connected to node (readonly)
		public List<Edge> Edges(int node)
		{
			return edges[node];
		}
		
		//Returns all edges contained in the graph
		public List<Edge> AllEdges()
		{
			List<Edge> edgeList = new List<Edge>();
			for (int v = 0; v < V; v++)
				for (int e = 0; e < edges[v].Count; e++)
				{
					int w = edges[v][e].Other(v);
					if (w > v)
					// Attention: undirected graph, every edge should appear only once
					{
						Edge edge = new Edge(v, w, edges[v][e].Weight());
						edgeList.Add(edge);
					}
				}
			return edgeList;
		}

		// Returns the Edge between the two given input nodes
		// null if no such edge exists.
		public Edge Edge(int v, int w)
		{
			if (v < 0 || v > V) return null;
			if (w < 0 || w > V) return null;

			for (int e = 0; e < edges[v].Count; e++)
				if (edges[v][e].Other(v) == w) return edges[v][e];

			return null;
		}

		// returns weight of edge if existing, infinity if not existing
		public double Weight(int v, int w)
		{
			Edge edge = Edge(v, w);
			if (edge == null) return System.Double.PositiveInfinity;
			return edge.Weight();
		}
	}
}
