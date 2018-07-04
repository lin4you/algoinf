//**************************************
// Flow networks 
// implementation via adjacency lists
//**************************************

using System;
using System.Collections.Generic;
using DirectedWeightedGraph;

namespace Flow
{

	// Residual flow network with edges i->j and j->i
	// where weight(i->j)+weight(j->i)=Capacity(i->j)
	// Flows are implicit as Residual, i.e. weight(j->i)
	// Zero weight corresponds to edges not being available for an augmenting path
	// Extends the basic class Graph

	public class FlowGraph : Graph
	{
		readonly private int source;
		public int Source { get { return source; } }
		readonly private int target;
		public int Target { get { return target; } }

		// creates the Residual Graph according to a Graph G with zero flow,
		// extends the basic constructor
		public FlowGraph(Graph G, int s, int t) : base(G.Nodes())
		{
			this.source = s;
			this.target = t;
			for (int v = 0; v < G.Nodes(); v++)
			{
				foreach (Edge edge in G.Edges(v))
				{
					// Add the forward edge corresponding to the capacity
					AddEdge(edge.From(), edge.To(), edge.Weight());
					// Add the inverse edge corresponding to the current flow
					// through this connection, which is currently zero.
					AddEdge(edge.To(), edge.From(), 0);
				}
			}
		}

		// increase weight by value
		private void increase(int from, int to, double value)
		{
			Edge edge = Edge(from, to);
			if (edge != null)
			{
				edge.SetWeight(edge.Weight() + value);
			}
			else throw new ArgumentException("No edge exists between " + from + " and " + to);
		}
		// decrease weight by value
		private void decrease(int from, int to, double value)
		{
			Edge edge = Edge(from, to);
			if (edge != null)
			{
				edge.SetWeight(edge.Weight() - value);
			}
			else throw new ArgumentException("No edge exists between " + from + " and " + to);
		}

		// compute Flow of the network, is the sum of incoming weights to source of residual network
		public double ComputeFlow()
		{
			double flow = 0.0;
			for (int v = 0; v < V; v++)
			{
				foreach (Edge edge in Edges(v))
				{
					if (edge.To() == source) flow += edge.Weight();
				}
			}
			return flow;
		}

		// Computes the capacity along the given path.
		// The path is given as a list of node indices.
		// The path capacity is defined as the minimum weight
		// along the path.
		public double PathCapacity(List<int> path) {
			double min = System.Double.PositiveInfinity;
			if(path == null) return min;
			int v = -1;
			foreach (int w in path)
			{
				if (v > -1)
				{
					double weight = Weight(v, w);
					if (weight < min) min = weight;
				}
				v = w;
			}
			return min;
		}

		// Augments according to given Path
		public void Augment(List<int> path)
		{
			if (path == null) return;
			// compute the path capacity
			double cap = PathCapacity(path);
			// adjust values along vertices of the path
			int v = -1;
			foreach (int w in path)
			{
				if (v > -1)
				{
					this.decrease(v, w, cap);
					this.increase(w, v, cap);
				}
				v = w;
			}
		}
	}
}
