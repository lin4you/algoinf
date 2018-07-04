using System;
using System.Collections.Generic;
using IndexedPriorityQueue;
using UndirectedWeightedGraph;
using Prim;

namespace EuclidTSP
{
	class EuclidTSP
	{
		public static void Main(string[] args)
		{
			string corrds_path = "cities_xy.txt";
			string names_path = "city_names.txt";
			string start_city = "Roswell, NM";

			// Einlesen der Städtedaten
			List<double[]> coords = TupleReader.ReadDoublesFromFile(corrds_path, 2, ";", "", "^", "$");
			List<string> names = TupleReader.ReadList(names_path, "", "^", "$");

			if (coords.Count != names.Count)
			{
				throw new InvalidOperationException("Number of city coordinates and city names does not match.");
			}

			int V = coords.Count;
			int v_0 = names.IndexOf(start_city);
			if ((v_0 < 0) || (v_0 >= V)) throw new InvalidOperationException("Did not find start city.");

			// erzeuge Distanzmatrix
			double[,] D = new double[V, V];
			for (int i = 0; i < V; ++i)
			{
				// Diagonale mit 0 initialisieren
				D[i, i] = 0;
				// Rest der symmetrischen Matrix füllen
				for (int j = i + 1; j < V; ++j)
				{
					D[i, j] = D[j, i] = EuclideanDistance(coords[i], coords[j]); // euklidischer Abstand

				}
			}

			// berechne Greedy-TSP (Aufgabe 6.1)
			List<int> greedyTour = GreedyTour(D, v_0);
			Console.Write("===\nGreedy, ");
			DisplayPath(greedyTour, names, D);

			// berechne MST-Approximation
			List<int> mstTour = MSTApproximation(D, v_0);
			Console.Write("===\nSpannbaumheuristik, ");
			DisplayPath(mstTour, names, D);
		}

		public static double EuclideanDistance(double[] cityA, double[] cityB)
		{
			return Math.Sqrt((cityA[0] - cityB[0]) * (cityA[0] - cityB[0]) + (cityA[1] - cityB[1]) * (cityA[1] - cityB[1]));
		}

		public static double GetTourLength(List<int> nodes, double[,] D)
		{
			if (nodes == null || nodes.Count == 0) return 0;

			double length = 0;
			int v = nodes[0];
			for (int i = 1; i < nodes.Count; ++i)
			{
				length += D[v, nodes[i]];
				v = nodes[i];
			}
			return length;
		}

		public static void DisplayPath(List<int> tour, List<string> names, double[,] D)
		{
			if(tour.Count == 0) throw new InvalidOperationException("Empty Tour.");

			Console.WriteLine("Tourlänge: " + GetTourLength(tour, D));
			Console.Write("(" + tour[0] + ")" + names[tour[0]]);
			for (int i = 1; i < tour.Count; ++i)
			{
				Console.Write(" -> (" + tour[i] + ")" + names[tour[i]]);
			}
			Console.WriteLine();
		}

		public static List<int> MSTApproximation(double[,] D, int v_0 = 0)
		{
			int V = D.GetLength(0);

			// erzeuge voll vernetzten Graphen aus den Distanzen
			Graph g = new Graph(V);
			for (int i = 0; i < V; ++i)
			{
				for (int j = 0; j < i; ++j)
				{
					g.AddEdge(i, j, D[i, j]);
				}
			}

			// berechne minimalen Spannbaum (minimum spanning tree) mit Prim
			List<Edge> mst = Prim.Prim.SpanningTree(g, v_0);
			// erzeuge Tour
			List<int> tour = new List<int>(V + 1);

			foreach (var edge in mst)
			{
				tour.Add(edge.Some());
			}

			// schließe Pfad zu einem Zyklus
			tour.Add(v_0);

			return tour;
		}

		public static List<int> GreedyTour(double[,] D, int v_0)
		{
			int V = D.GetLength(0);
			// erzeuge Tour
			List<int> tour = new List<int>(V + 1);
			
			var heap = new PriorityHeap(V);
			heap.Insert(v_0, 0);
			
			//start
			tour.Add(v_0);

			var lastNode = v_0;

			for (int i = 0; i < V-1; i++)
			{
				int shortestNode = -1;
				double shortestDistance = Double.PositiveInfinity;
				for (int j = 0; j < V; j++)
				{
					if (lastNode == j || heap.Contains(j))
					{
						continue;
					}
					
					if (shortestDistance > D[lastNode, j])
					{
						shortestNode = j;
						shortestDistance = D[lastNode, j];
					}
				}

				lastNode = shortestNode;
				heap.Insert(shortestNode, shortestDistance);
				tour.Add(shortestNode);
			}
			
			//end
			tour.Add(v_0);

			return tour;
		}
	}
}
