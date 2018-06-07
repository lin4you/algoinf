using System;
using System.Collections.Generic;
using UndirectedGraph;

namespace Euler
{
	public class Euler
	{

		public static void Main()
		{
			// Set Graph
			//int[,] edges = 
			//{{0,1},{0,2},{0,3},{0,5},{1,2},{1,4},{1,7},{3,5},{4,7},
			//	{5,6},{5,7},{6,8},{6,9},{6,10},{7,8},{8,9},{8,10}};
//			{{0,1},{0,2},{0,3},{0,4},{1,2},{1,5},{1,6},{3,4},{3,6},
//				{3,7},{5,6},{6,8},{7,8},{7,9},{7,10},{8,9},{8,10}};
			
			String pairPath = "./blatt2_aufgabe1_b_graph.txt";

			Console.WriteLine("processing " + pairPath);
			List<int[]> edges = TupleReader.ReadPairs(pairPath);
		
			
			Graph graph = new Graph(edges);
			
			Console.WriteLine($"Size: {graph.Size()}");

			foreach (var component in graph.Components())
			{
				var size = graph.DFS_Stack(component).Count;
				
				Console.WriteLine(size);
			}
			
			// Display the graph
			graph.Display();
			
			pairPath = "./blatt2_aufgabe2_graph.txt";

			Console.WriteLine("processing " + pairPath);
			edges = TupleReader.ReadPairs(pairPath);
			graph = new Graph(edges);
			
			// Compute Eulerian cycle
			//List<int> cycle = EulerianCycle(graph);
			
			// Compute Eulerian path
			List<int> cycle = EulerianPath(graph);

			if(cycle == null) {
				return;
			}

//			// Output tour
			System.Console.Write("Eulertour: ");
			foreach (int v in cycle)
			{
				System.Console.Write(v + ", ");
			}
			System.Console.WriteLine();
		}
		
		public static List<int> EulerianPath(Graph g, int v0 = -1) {
			// Test whether there is an Eulerian cycle
			if (!g.Connected())
			{
				System.Console.WriteLine("Graph is not connected.");
				return null;
			}

			int oddCount = 0;
			for (int v = 0; v < g.Nodes(); v++)
			{
				if (g.Degree(v) % 2 != 0)
				{
					oddCount++;
					if (oddCount > 2)
					{
						return null;
					}
				}
			}

			if (oddCount == 1)
			{
				return null;
			}
			
			// This additional block is just there to avoid a variable clash
			// with the 'v' in the for-loop before.
			{
				// Compute Eulerian cycle
				// idea:
				// cycle contains path from base to node v with no unvisited edge loops
				// nextNodes contains path from v to base
				// move vertices from nextNodes to cycle with completion of unvisited edges
				Graph copy = new Graph(g);
				Stack<int> cycle = new Stack<int>();
				Stack<int> nextNodes = new Stack<int>();
				int v;
				if(v0 < 0) {
					v = copy.Base();
				} else {
					v = v0;
				}
				nextNodes.Push(v);
				while (nextNodes.Count > 0)
				{
					v = nextNodes.Pop();
					cycle.Push(v);
					while (copy.Degree(v) > 0)
					{
						// As long as there are still edges left from this node,
						// cross them and remove them from the graph. Then continue
						// the path on the new node we have reached.
						nextNodes.Push(v);
						int w = copy.NextNode(v);
						copy.DeleteEdge(v, w);
						v = w;
					}
				}

				List<int> outList = new List<int>();
				while(cycle.Count > 0) {
					outList.Add(cycle.Pop());
				}

				return outList;
			}
		}

		public static List<int> EulerianCycle(Graph g, int v0 = -1) {
			// Test whether there is an Eulerian cycle
			if (!g.Connected())
			{
				System.Console.WriteLine("Graph is not connected.");
				return null;
			}
			for (int v = 0; v < g.Nodes(); v++)
			{
				if (g.Degree(v) % 2 != 0){
					System.Console.WriteLine("Degree of node " + v + " is odd.");
					return null;
				}
			}
			// This additional block is just there to avoid a variable clash
			// with the 'v' in the for-loop before.
			{
			// Compute Eulerian cycle
			// idea:
			// cycle contains path from base to node v with no unvisited edge loops
			// nextNodes contains path from v to base
			// move vertices from nextNodes to cycle with completion of unvisited edges
			Graph copy = new Graph(g);
			Stack<int> cycle = new Stack<int>();
			Stack<int> nextNodes = new Stack<int>();
			int v;
			if(v0 < 0) {
				v = copy.Base();
			} else {
				v = v0;
			}
			nextNodes.Push(v);
			while (nextNodes.Count > 0)
			{
				v = nextNodes.Pop();
				cycle.Push(v);
				while (copy.Degree(v) > 0)
				{
					// As long as there are still edges left from this node,
					// cross them and remove them from the graph. Then continue
					// the path on the new node we have reached.
					nextNodes.Push(v);
					int w = copy.NextNode(v);
					copy.DeleteEdge(v, w);
					v = w;
				}
			}

			List<int> outList = new List<int>();
			while(cycle.Count > 0) {
				outList.Add(cycle.Pop());
			}

			return outList;
			}
		}
	}
}
