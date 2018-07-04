using System;
using System.Collections.Generic;
using DirectedWeightedGraph;
using Flow;

namespace EdmondsKarp
{
	public class EdmondsKarp : FlowOptimizer
	{

		public static void Maina()
		{
			// Construct the Capacity Graph
			int s = 0;
			int t = 5;
			int[,] edges =
			{{0,1,16},{0,2,13},{1,3,12},{2,1,4},{2,4,14},{3,2,9},
			{3,5,20},{4,3,7},{4,5,4}};
			//{{0,1,5},{0,2,7},{1,2,3},{2,3,8},{3,1,2},{1,4,6},{4,3,3},{3,5,4},{4,5,7}};

			Graph graph = new Graph(edges);
			graph.Display();

			// construct the initial network.
			FlowGraph network = new FlowGraph(graph, s, t);

			// Apply Edmonds Karp Algorithm.
			EdmondsKarp optimizer = new EdmondsKarp();
			optimizer.InteractiveOptimumFlow(network);

			// Display the optimum flow
			System.Console.WriteLine("Flow is " + network.ComputeFlow());
		}

		// compute residual path from source to sink using breadth first search
		// This way the Edmonds & Karp-Algorithm ensures that the shortest path
		// is chosen in each iteration and thus ensures polynomial complexity.
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
			Queue<int> nextNodes = new Queue<int>();
			nextNodes.Enqueue(network.Source);
			while (nextNodes.Count > 0)
			{
				int v = nextNodes.Dequeue();
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
							nextNodes.Enqueue(edge.To());
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

