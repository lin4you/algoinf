//**************************************
// directed weighted Graphs
// implementation via adjacency lists
//**************************************

using System;
using System.Collections.Generic;

namespace DirectedWeightedGraph
{
	public class Edge
	{
		readonly private int v, w;
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
		public int From()
		{
			return v;
		}
		public int To()
		{
			return w;
		}
		public double Weight()
		{
			return weight;
		}
		public void Output()
		{
			System.Console.Write(v + " " + w + " (" + weight + ")");
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
		readonly protected int V;
		readonly protected List<Edge>[] adj;

		// Creates an empty graph with V nodes
		public Graph(int V)
		{
			if (V < 0) throw new ArgumentException("Number of vertices should not be negative\n");
			this.V = V;
			adj = new List<Edge>[V];
			for (int v = 0; v < V; v++)
				adj[v] = new List<Edge>();
		}

		// Creates a graph from a list of triples
		public Graph(List<int[]> edges){
			// retrieve the number of nodes as the maximum index used in an edge.
			this.V = 0;
			foreach ( int[] triple in edges ) {
				if(triple.Length != 3) throw new ArgumentException("Not all entries in the given edge list were triples");
				if(triple[0] > this.V) this.V = triple[0];
				if(triple[1] > this.V) this.V = triple[1];
			}
			this.V++;
			// initialize the adjacency list
			adj = new List<Edge>[V];
			for (int v = 0; v < V; v++)
				adj[v] = new List<Edge>();
			// add the edges
			foreach ( int[] triple in edges ) {
				AddEdge(triple[0], triple[1], triple[2]);
			}
		}

		// Creates a graph from a multidimensional array
		public Graph(int[,] edges){
			if(edges.GetLength(1) != 3)
				throw new ArgumentException("Expected an N x 3 matrix containing start index, end index and weight in each row.");
			// retrieve the number of nodes as the maximum index used in an edge.
			this.V = 0;
			for (int e = 0; e < edges.GetLength(0); e++) {
				if(edges[e, 0] > this.V) this.V = edges[e, 0];
				if(edges[e, 1] > this.V) this.V = edges[e, 1];
			}
			this.V++;
			// initialize the adjacency list
			adj = new List<Edge>[V];
			for (int v = 0; v < V; v++)
				adj[v] = new List<Edge>();
			// add the edges
			for (int e = 0; e < edges.GetLength(0); e++) {
				AddEdge(edges[e, 0], edges[e, 1], edges[e, 2]);
			}
		}

		// Prints the graph
		public void Display()
		{
			for (int v = 0; v < this.V ; v++)
			{
				System.Console.Write(v + ": ");
				for (int e = 0; e < this.adj[v].Count; e++)
					System.Console.Write(adj[v][e].To() + " (" + adj[v][e].Weight() + ") ");
				System.Console.WriteLine();
			}
		}
		// Adds an edge by means of the values
		public void AddEdge(int v, int w, double value)
		{
			if (Edge (v, w) == null) 
			{
				Edge edge = new Edge (v, w, value);
				adj [v].Add (edge);
			}
		}

		// Returns first index
		public int Base()
		{
			return 0;
		}

		// Returns number of nodes
		public int Nodes()
		{
			return V;
		}
		// Returns all edges connected to node (readonly)
		public List<Edge> Edges(int v)
		{
			return adj[v];
		}
		// Returns the Edge between the two given input nodes
		// null if no such edge exists.
		public Edge Edge(int v, int w)
		{
			if (v < 0 || v > V) return null;
			if (w < 0 || w > V) return null;
			foreach (Edge edge in adj[v])
			{
				if (edge.To() == w) return edge;
			}
			return null;
		}
		// Returns all edges of the graph
		public List<Edge> AllEdges()
		{
			List<Edge> edgeList = new List<Edge>();
			for (int v = 0; v < V; v++)
				edgeList.AddRange(adj[v]);
			return edgeList;
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
