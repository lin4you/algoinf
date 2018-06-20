using System;
using System.Collections.Generic;
using UndirectedWeightedGraph;
using IndexedPriorityQueue;

namespace Prim
{
	public class Prim
	{

//		public static void Main()
//		{
//			// Set Graph
//			int V = 7;
//			int[,] edges =
//			{{0,1,3},{0,2,2},{0,3,3},{1,3,2},{1,4,1},{2,5,2},{2,6,7},
//				{3,4,2},{3,5,3},{3,6,4},{4,6,1},{5,6,3}};
//			
//			Graph graph = new Graph(V);
//			for (int i=0;i<edges.GetLength(0);i++)
//			{
//				graph.AddEdge(edges[i,0],edges[i,1],edges[i,2]);
//			}

//			// Display the graph
//			graph.Display();

//			// Apply the algorithm of Prim to find a minimum spanning tree.
//			List<Edge> spanningTree = SpanningTree(graph);

//			// Output spanning tree
//			System.Console.WriteLine("Spanning tree:");
//			double sum = 0.0;
//			foreach (Edge edge in spanningTree)
//			{
//				edge.Output();
//				sum += edge.Weight();
//			}
//			System.Console.WriteLine("with length: " + sum);
//		}

		public static List<Edge> SpanningTree(Graph graph)
		{
			return SpanningTree(graph, 0);
		}

		public static List<Edge> SpanningTree(Graph graph, int startNode)
		{

			// idea: 
			// greedy approach,
			// iteratively grow spanning tree by respective shortest connection
			// reachable nodes are efficiently stored in a heap
			PriorityHeap distances = new PriorityHeap(graph.Nodes());
			// these are the edges that connect a new node with our already
			// constructed spanning tree.
			Edge[] connectingEdge = new Edge[graph.Nodes()];

			// store all reachable nodes with their distance.
			foreach (Edge edge in graph.Edges(startNode))
			{
				int w = edge.Other(startNode);
				distances.Insert(w, edge.Weight());
				connectingEdge[w] = edge;
			}

			// for all other nodes store an infinite distance.
			for (int v = 0; v < graph.Nodes(); v++)
			{
				if (v == startNode) continue;
				if (!distances.Contains(v))
				{
					distances.Insert(v, System.Double.PositiveInfinity);
				}
			}

			// these are the already selected edges
			List<Edge> spanningTree = new List<Edge>();

			while (!distances.Empty())
			{
				// get the nearest node that is not yet in the spanning tree.
				int v = distances.DeleteMin();
				// add its connecting edge to the spanning tree.
				spanningTree.Add(connectingEdge[v]);
				// consider all edges from v for the next step of the algorithm.
				foreach (Edge edge in graph.Edges(v))
				{
					int w = edge.Other(v);
					// If we still need the node at the other end of that
					// edge and the edge weight is better than our currently
					// stored edge, store the new edge.
					if (distances.Contains(w) && edge.Weight() < distances.GetKey(w))
					{
						distances.Change(w, edge.Weight());
						connectingEdge[w] = edge;
					}
				}
			}

			return spanningTree;
		}
	}
}
