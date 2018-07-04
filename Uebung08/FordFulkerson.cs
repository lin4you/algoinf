using System;
using System.Collections.Generic;
using DirectedWeightedGraph;
using Flow;
using EdmondsKarp;

namespace FordFulkerson
{
	public class FordFulkerson : FlowOptimizer
	{

		public static void Maina(string[] args)
		{
			string path = (args.Length > 0) ? args [0] : "blatt8_aufg1.txt";

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
		
		List<int> DFS_Stack(int root, int target, FlowGraph network)
		{
			bool[] visited = new bool[network.Nodes()];
			List<int> visitedNodes = new List<int>();
			Stack<int> nextNodes = new Stack<int>();
			nextNodes.Push(root);
			while(nextNodes.Count > 0)
			{
				int v = nextNodes.Pop();
				if(!visited[v])
				{
					visited[v] = true;
					visitedNodes.Add(v);

					if (v == target)
					{
						return visitedNodes;
					}
					
					List<Edge> neighbors = network.Edges(v);
					foreach (Edge e in neighbors)
					{
						if (e.From() == v)
						{
							nextNodes.Push(e.To());
						}
					}
				}
			}
			
			return null;
		}

		// compute residual path from source to sink using depth first search
		public override List<int> ComputeResidualPath(FlowGraph network)
		{
			double flow = 0;
 
			List<int> path = DFS_Stack(network.Source, network.Target, network);
 
			while (path != null && path.Count > 0)
			{
				double minCapacity = double.MaxValue;

				for (int i = 1; i < path.Count; i++)
				{
					double capacity = network.Weight(path[i - 1], path[i]);
					if (capacity < minCapacity)
					{
						minCapacity = capacity; 
					}
				}

				network.Augment(path); //AugmentPath(path, minCapacity);
				flow += minCapacity;
 
				path = DFS_Stack(network.Source, network.Target, network);
			}


			return path;
			//FindMinCut(nodeSource);
			
			return null;
			
			
			/*bool[] visited = new bool[network.Nodes()];
			List<int> visitedNodes = new List<int>();
			Stack<int> nextNodes = new Stack<int>();
			nextNodes.Push(network.Source);
			while(nextNodes.Count > 0)
			{
				network.Augment();
				int v = nextNodes.Pop();
				if(!visited[v])
				{
					visited[v] = true;
					visitedNodes.Add(v);
					List<Edge> neighbors = network.Edges(v);
					foreach (Edge w in neighbors)
					{
						nextNodes.Push(w.To());
					}
				}
			}

			if (visitedNodes.Count == 0)
			{
				return null;
			}
			
			return visitedNodes;
			
			foreach (var edge in network.AllEdges())
			{
				
			}*/
			/*
			 *  setze Fluss f(u,v) = 0
				berechne das residuale Netz Gf
				while es gibt augmentierenden Pfad p in Gf do
					cf(p) := min{cf(u,v)|(u,v) in p} 
					for all Kanten (u,v) in p do
						if (u,v) ∈ E then
							f (u, v ) := f (u, v ) + cf (p)
						else if (v,u) ∈ E then
							f (v , u) := f (v , u) − cf (p)
						end if 
					end for
					berechne Gf 
				end while
			 */
			//TODO: implement this method
			// if no path is found, return null
			return null;
		}

	}
}

