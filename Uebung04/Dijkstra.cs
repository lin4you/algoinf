using System;
using System.Collections.Generic;
using DirectedWeightedGraph;
using IndexedPriorityQueue;

namespace Dijkstra
{
	public class Dijkstra
	{

		public static void Main()
		{
			String pairPath = "./blatt04_aufg1_a.txt";

			Console.WriteLine("processing " + pairPath);
			List<int[]> edges = TupleReader.ReadTriples(pairPath);

			Graph graph = new Graph(edges);
			// Display the graph
			graph.Display();

			int vStart = 0; 		//compute shortest paths from this node

			double[] pi;	//shortest known path lengths
			int[] pred;		//predeceesor nodes for these paths

			// Find the shortest path using the Dijkstra algorithm.
			ShortestPathsHeap(graph, vStart, out pi, out pred);

			// Output shortest paths
			DisplayShortestPaths(vStart, pi, pred);
			
			double[] pi2;	//shortest known path lengths
			int[] pred2;		//predeceesor nodes for these paths

//			// Check Again with Array implementation
			//ShortestPathsArray(graph, vStart, out pi2, out pred2);

//			// Output shortest paths
			//DisplayShortestPaths(vStart, pi2, pred2);
		}


		public static void ShortestPathsHeap(Graph graph, int vStart,
			out double[] pi, out int[] pred)
		{
			ShortestPaths(graph, vStart, new PriorityHeap(graph.Nodes()), out pi, out pred);
		}

		public static void ShortestPathsArray(Graph graph, int vStart,
			out double[] pi, out int[] pred)
		{
			ShortestPaths(graph, vStart, new PriorityArray(graph.Nodes()), out pi, out pred);
		}

		public static void ShortestPaths(Graph graph, int vStart,
			IndexedPriorityQueue.IndexedPriorityQueue reachableNodes,
			out double[] pi, out int[] pred)
		{
			// idea:
			// greedy approach,
			// always extend a shortest path tree by the minimum reachable node
			// reachable nodes and shortest paths lengths are efficiently stored in a heap

			pi = new double[graph.Nodes()];				// shortest known path lengths
			pred = new int[graph.Nodes()];				// predeceesor nodes for these paths
			for (int v = 0; v < graph.Nodes(); v++){
				pi[v] = System.Double.PositiveInfinity;
				pred[v] = -1;
			}
			pi[vStart] = 0.0;
			reachableNodes.Insert(vStart, pi[vStart]);
			while (!reachableNodes.Empty())
			{
				int v = reachableNodes.DeleteMin();		// nearest reachable node
				List<Edge> edges = graph.Edges(v);
				for (int e = 0; e < edges.Count; e++)	// test edges from v
				{
					int w = edges[e].To();
					double weight = edges[e].Weight();
					if (pi[w] > pi[v] + weight)			// new vertex v leads to shorter path
					{
						pi[w] = pi[v] + weight;
						pred[w] = v;
						if (reachableNodes.Contains(w)) reachableNodes.Change(w, pi[w]);
						else reachableNodes.Insert(w, pi[w]);
					}
				}
			}
		}

		public static void DisplayShortestPaths(int vStart, double[] pi, int[] pred) {
			System.Console.WriteLine("Shortest Paths from " + vStart);
			for (int v = 0; v < pi.Length; v++)
			{
				System.Console.Write("length " + pi[v] + ": " + v);
				int w = v;
				while (pred[w] != -1)
				{
					System.Console.Write(" <- " + pred[w]);
					w = pred[w];
				}
				System.Console.WriteLine();
			}
		}
	}
}
