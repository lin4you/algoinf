using System;
using System.Collections.Generic;
using DirectedWeightedGraph;
using Flow;
using EdmondsKarp;

namespace BipartiteMatching
{
	public class BipartiteMatching
	{

		public static void Maina(string[] args)
		{
			string path = (args.Length > 0) ? args [0] : "blatt8_aufg2.txt";

			// Construct the graph
			List<int[]> edges = TupleReader.ReadPairs(path);
			UndirectedGraph.Graph graph = new UndirectedGraph.Graph(edges);
			// display the graph
			graph.Display();

			// identify the bipartition
			bool[] partition = Bipartition(graph);
			if(partition == null) {
				System.Console.WriteLine("The graph is not bipartite!");
				return;
			}

			// show the bipartition
			DisplayPartition(partition);

			// Find the Bipartite Matching
			List<DirectedWeightedGraph.Edge> matching = Matching(graph, partition);
			DisplayMatching(matching);
		}

		public static void DisplayPartition(bool[] partition) {
			List<int> L = new List<int>();
			List<int> R = new List<int>();
			for(int v = 0; v < partition.Length; v++) {
				if(partition[v]) {
					L.Add(v);
				} else {
					R.Add(v);
				}
			}
			System.Console.Write("L: ");
			foreach(int v in L) System.Console.Write(v + ", ");
			System.Console.WriteLine();
			System.Console.Write("R: ");
			foreach(int v in R) System.Console.Write(v + ", ");
			System.Console.WriteLine();
		}

		// Identifies a Bipartition in a bipartite Graph via depth first search
		// and returns a boolean array where the vth entry is true if and only if
		// the respective node is element of the left set in the bipartition and
		// false if and only if it is part of the right set in the bipartition.
		// If the graph is not bipartite, this method return null.
		public static bool[] Bipartition(UndirectedGraph.Graph graph) {
			bool[] partition = new bool[graph.Nodes()];
			
			// stores vertex is discovered or not
			bool[] discovered = new bool[graph.Nodes()];

			// stores color 0 or 1 of each vertex in BFS
			//bool[] color = new bool[N];

			// mark source vertex as discovered and
			// set its color to 0
			discovered[0] = true;
			partition[0] = false;

			//List<int> nodes = graph.DFS_recursive();
			// start DFS traversal from any node as graph
			// is connected and undirected
			if (DFS(graph, 1, discovered, partition))
			{
				return partition;
			}

			return null;
		}
		
		static bool DFS(UndirectedGraph.Graph graph, int v, bool[] discovered, bool[] color)
		{
			// do for every edge (v -> u)
			foreach (int u in graph.AllNodes(v))
			{
				// if vertex u is explored for first time
				if (discovered[u] == false)
				{
					// mark current node as discovered
					discovered[u] = true;
					// set color as opposite color of parent node
					color[u] = !color[v];

					// if DFS on any subtree rooted at v
					// we return false
					if (DFS(graph, u, discovered, color) == false)
						return false;
				}
				// if the vertex is already been discovered and
				// color of vertex u and v are same, then the
				// graph is not Bipartite
				else if (color[v] == color[u]) {
					return false;
				}
			}

			return true;
		}

		public static void DisplayMatching(List<Edge> edges) {
			System.Console.WriteLine("Matching:");
			foreach ( Edge e in edges ) {
				System.Console.WriteLine("{" + e.From() + "," + e.To() + "}");
			}
		}

		// Computes a bipartite matching in the given graph with the given partition of nodes.
		// The partition is given in form of a boolean array as returned by the Partition
		// function.
		public static List<Edge> Matching(UndirectedGraph.Graph graph, bool[] partition) {
			List<Edge> matching = new List<Edge>();
			//TODO: Implement

			DirectedWeightedGraph.Graph g = new DirectedWeightedGraph.Graph(graph.Nodes()+2);

			int s = g.Nodes() - 2;
			int t = g.Nodes() - 1;
			
			for (int i = 0; i < graph.Nodes(); i++)
			{
				List<int> nodes = graph.AllNodes(i);
				for (int j = 0; j < nodes.Count; j++)
				{
					g.AddEdge(i, nodes[j], 1);
					
					if (partition[i])
					{
						g.AddEdge(i, t, 1);
					}
					else
					{
						g.AddEdge(s, i, 1);
					}
				}
			}
			
			
			FlowGraph network = new FlowGraph(g, s, t);
			FlowOptimizer optimizer = new EdmondsKarp.EdmondsKarp();
			optimizer.InteractiveOptimumFlow(network);

			var path = optimizer.ComputeResidualPath(network);

			if (path == null)
			{
				return null;
			}
			
			for (int i = 1; i < path.Count; i++)
			{
				matching.Add(new Edge(path[i-1], path[i], 1));
			}

			// Display the optimum flow
			//System.Console.WriteLine("Flow is " + network.ComputeFlow());
			
			return matching;
		}
	}
}

