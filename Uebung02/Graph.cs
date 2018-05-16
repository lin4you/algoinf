//**************************************
// undirected Graphs
// implementation via adjacency lists
// assumption: 
// nodes are enumerated by numbers 0 .. Nodes()-1
// after creation, the number of nodes is fixed
//**************************************

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace UndirectedGraph
{
	public class Graph
	{
		readonly private int V;
		readonly private List<int>[] adj;

		// Creates an empty graph with V nodes
		public Graph(int V){
			if (V < 0) throw new ArgumentException("Number of vertices should not be negative\n");
			this.V = V;
			adj = new List<int>[V];
			for (int v = 0; v < V; v++)
				adj[v] = new List<int>();
		}

		// Creates a graph from a list of tuples
		public Graph(List<int[]> edges){
			// retrieve the number of nodes as the maximum index used in an edge.
			this.V = 0;
			foreach ( int[] tuple in edges ) {
				if(tuple.Length != 2) throw new ArgumentException("Not all entries in the given edge list were tuples");
				if(tuple[0] > this.V) this.V = tuple[0];
				if(tuple[1] > this.V) this.V = tuple[1];
			}
			this.V++;
			// initialize the adjacency list
			adj = new List<int>[V];
			for (int v = 0; v < V; v++)
				adj[v] = new List<int>();
			// add the edges
			foreach ( int[] tuple in edges ) {
				AddEdge(tuple[0], tuple[1]);
			}
		}

		// Creates a graph from a multidimensional array
		public Graph(int[,] edges){
			// retrieve the number of nodes as the maximum index used in an edge.
			this.V = 0;
			for (int e = 0; e < edges.GetLength(0); e++) {
				if(edges[e, 0] > this.V) this.V = edges[e, 0];
				if(edges[e, 1] > this.V) this.V = edges[e, 1];
			}
			this.V++;
			// initialize the adjacency list
			adj = new List<int>[V];
			for (int v = 0; v < V; v++)
				adj[v] = new List<int>();
			// add the edges
			for (int e = 0; e < edges.GetLength(0); e++) {
				AddEdge(edges[e, 0], edges[e, 1]);
			}
		}

		// Creates a graph as copy of G
		public Graph(Graph G, int additionalNodes = 0){
			this.V = G.Nodes() + additionalNodes;
			adj = new List<int>[this.V];
			for (int v = 0; v < G.V; v++)
			{
				adj[v] = new List<int>();
				for(int e = 0; e < G.adj[v].Count; e++)
					adj[v].Add(G.adj[v][e]);
			}
			for (int v = G.V; v < this.V; v++)
			{
				adj[v] = new List<int>();
			}
		}

		// Prints the graph
		public void Display(){
			for (int v = 0; v < V; v++)
			{
				System.Console.Write(v + ": ");
				for (int e = 0; e < adj[v].Count; e++)
					System.Console.Write(adj[v][e] + " ");
				System.Console.WriteLine();
			}
		}

		// Adds an edge
		public void AddEdge(int v, int w)
		{
			if (!adj[v].Contains(w))
				adj[v].Add(w);
			if (!adj[w].Contains(v))
				adj[w].Add(v);
		}

		// Deletes an edge
		public void DeleteEdge(int v, int w)
		{
			adj[v].Remove(w);
			adj[w].Remove(v);
		}

		// Returns first node
		public int Base()
		{
			if (V == 0)  throw new ApplicationException("Graph is empty\n");
			return 0;
		}

		// Returns number of nodes
		public int Nodes()
		{
			return V;
		}

		// Returns first node connected to node v
		public int NextNode(int v)
		{
			return adj[v][0];
		}

		// Returns all nodes connected to node v (readonly)
		public List<int> AllNodes(int v)
		{
			return adj[v];
		}

		// Returns node degree
		public int Degree(int v)
		{
			return adj[v].Count;
		}

		// Returns whether graph is connected
		public bool Connected()
		{
			// Trivially connected
			if (V < 2) return true;
			// Try to visit all nodes using depth first search
			List<int> visitedNodes = DFS_Stack();
			// if we have visited all nodes, this graph is connected
			return visitedNodes.Count == V;
		}

		// Returns all nodes retrieved using a depth first search
		// starting at node v.
		public List<int> DFS_Stack(int v0 = 0)
		{
			bool[] visited = new bool[V];
			List<int> visitedNodes = new List<int>();
			Stack<int> nextNodes = new Stack<int>();
			nextNodes.Push(v0);
			while(nextNodes.Count > 0)
			{
				int v = nextNodes.Pop();
				if(!visited[v])
				{
					visited[v] = true;
					visitedNodes.Add(v);
					List<int> neighbors = AllNodes(v);
					foreach (int w in neighbors)
					{
						nextNodes.Push(w);
					}
				}
			}
			return visitedNodes;
		}

		public List<int> DFS_recursive(int v0 = 0)
		{
			bool[] visited = new bool[V];
			List<int> visitedNodes = new List<int>();
			DFS_recursion(v0, visited, visitedNodes);
			return visitedNodes;
		}

		private void DFS_recursion(int v, bool[] visited, List<int> visitedNodes)
		{
			visited[v] = true;
			visitedNodes.Add(v);
			List<int> neighbors = AllNodes(v);
			foreach (int w in neighbors)
			{
				if(!visited[w])
				{
					DFS_recursion(w, visited, visitedNodes);
				}
			}
		}

		public int Size()
		{
			int maxSize = 0;
			var components = Components();

			foreach (var component in components)
			{
				var size = DFS_Stack(component).Count;
				
				if (size > maxSize)
				{
					maxSize = size;
				}
			}

			return maxSize;
		}

		public List<int> Components()
		{
			var startNodes = new List<int>();
			
			var visitedNodes = new List<int>();
			for (int i = 0; i < V; i++)
			{
				if (visitedNodes.Contains(i)) continue;
				
				var nodes = DFS_Stack(i);
				startNodes.Add(nodes.First());
				visitedNodes.AddRange(visitedNodes);
			}

			return startNodes;
		}
	}
}
