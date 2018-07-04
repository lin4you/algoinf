using System;
using System.Collections.Generic;
using UndirectedGraph;
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
			//TODO: implement
			return partition;
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
			return matching;
		}
	}
}

