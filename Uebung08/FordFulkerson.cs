using System.Collections.Generic;
using DirectedWeightedGraph;
using Flow;

namespace FordFulkerson
{
	public class FordFulkerson : FlowOptimizer
	{

		public static void Main(string[] args)
		{
			string path = (args.Length > 0) ? args [0] : "blatt8_aufg2.txt";

			// Construct the Capacity Graph
			List<int[]> edges = TupleReader.ReadTriples(path);

			Graph graph = new Graph(edges);
			graph.Display();

			int s = 0;
			int t = graph.Nodes() - 1;

			// Apply Edmonds-Karp Algorithm.
			{
				System.Console.WriteLine("\nEdmonds-Karp:\n");
				FlowGraph network = new FlowGraph(graph, s, t);
				FlowOptimizer optimizer = new EdmondsKarp.EdmondsKarp();
				optimizer.InteractiveOptimumFlow(network);

				// Display the optimum flow
				System.Console.WriteLine("Flow is " + network.ComputeFlow());
			}

			// Apply Ford-Fulkerson Algorithm.
			{
				System.Console.WriteLine("\nFord-Fulkerson:\n");
				FlowGraph network = new FlowGraph(graph, s, t);
				FlowOptimizer optimizer = new FordFulkerson();
				optimizer.InteractiveOptimumFlow(network);

				// Display the optimum flow
				System.Console.WriteLine("Flow is " + network.ComputeFlow());
			}
		}
		
		// compute residual path from source to sink using depth first search
		public override List<int> ComputeResidualPath(FlowGraph network)
		{
			int V = network.Nodes();
			bool[] visited = new bool[V];	// marks already visited nodes.
			int[] pred = new int[V];		// marks the predecessor of the
			// current node for the output path
			// initialize
			for (int v = 0; v < V; v++) visited[v] = false;
			for (int v = 0; v < V; v++) pred[v] = -1;
			visited[network.Source] = true;
			Stack<int> nextNodes = new Stack<int>();
			nextNodes.Push(network.Source);
			while (nextNodes.Count > 0)
			{
				int v = nextNodes.Pop();
				foreach (Edge edge in network.Edges(v))
				{
					// we are only interested in paths with nonzero weights.
					if ((edge.Weight() > 0) && !visited[edge.To()])
					{
						if (edge.To() != network.Target)
						{
							// If we have not yet reached the target, continue the
							// search.
							visited[edge.To()] = true;
							pred[edge.To()] = v;
							nextNodes.Push(edge.To());
						}
						else
						{
							// If we have reached the target, we extract the path.
							List<int> path = new List<int>();
							path.Add(network.Target);
							while (v != network.Source)
							{
								path.Add(v);
								v = pred[v];
							}
							path.Add(network.Source);
							// ... and return it
							path.Reverse();
							return(path);
						}
					}
				}
			}
			// no path found, return null
			return null;
		}

	}
}

