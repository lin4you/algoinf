using System;
using System.Collections.Generic;
using DirectedWeightedGraph;

namespace BellmanFord
{
	public class BellmanFord
	{

		public static void Main()
		{
			String pairPath = "./blatt04_aufg1_b.txt";

			Console.WriteLine("processing " + pairPath);
			List<int[]> edges = TupleReader.ReadTriples(pairPath);
			
			Graph graph = new Graph(edges);

			// Display the graph
			graph.Display();

			int vStart = 0; 	//compute shortest paths from this node

			// Apply Bellman-Ford algorithm
			double[] pi;		//shortest known path lengths
			int[] pred;			//predeceesor nodes for these paths

			if(!ShortestPaths(graph, vStart, out pi, out pred))
			{
				System.Console.WriteLine("There is a negative cycle!");
			} else {
				// Output shortest paths
				DisplayShortestPaths(vStart, pi, pred);
			}
		}

		// Returns true if there is no negative cycle
		public static bool ShortestPaths(Graph graph, int vStart,
			out double[] pi, out int[] pred)
		{
			int V = graph.Nodes();
			pi = new double[V];					//shortest known path lengths
			pred = new int[V];					//predeceesor nodes for these paths
			for (int v = 0; v < V; v++){
				// TODO: Here we need to initialize pi and pred.
				pi[v] = System.Double.PositiveInfinity;
				pred[v] = -1;
			}
			pi[vStart] = 0;

			List<Edge> edges = graph.AllEdges();
			// Apply the inner loop once for every node.
			for (int v = 0; v < V; v++)
			{	
				foreach (Edge edge in edges)	//test edges all edges
				{
					// TODO: In this inner loop we need to update
					//       pi and pred.

					if (v != edge.To() && v != edge.From())
					{
						continue;
					}
					
					int w = edge.To();
					double weight = edge.Weight();
					if (pi[w] > pi[v] + weight)			// new vertex v leads to shorter path
					{
						pi[w] = pi[v] + weight;
						pred[w] = v;
					}
				}
			}
			
			// TODO: Test whether there is a negative cycle and return false
			//       if such a cycle exists.
			
			foreach (Edge edge in edges)	//test edges all edges
			{
				int v = edge.From();
				int w = edge.To();
				double weight = edge.Weight();
				if (pi[w] > pi[v] + weight)			// new vertex v leads to shorter path
				{
					return false;
				}
			}
			
			return true;
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
