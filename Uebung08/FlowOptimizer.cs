using System;
using System.Collections.Generic;
using Flow;

namespace Flow
{
	public abstract class FlowOptimizer
	{

		public void OptimumFlow(FlowGraph network)
		{

			// As long as there is a path from source to target with non-zero
			// Weight we augment the residual network.
			List<int> residualPath = ComputeResidualPath(network);
			while (residualPath != null)
			{
				network.Augment(residualPath);
				residualPath = ComputeResidualPath(network);
			}
		}

		public void InteractiveOptimumFlow(FlowGraph network)
		{

			// As long as there is a path from source to target with non-zero
			// Weight we augment the residual network.
			List<int> residualPath = ComputeResidualPath(network);
			int augmentCounter = 0;
			while (residualPath != null)
			{
				++augmentCounter;
				System.Console.Write("Path (" + augmentCounter + "): " + network.Source);
				foreach (int v in residualPath.GetRange(1, residualPath.Count - 1))
				{
					System.Console.Write(" -> " + v);
				}
				System.Console.WriteLine(" with capacity: " + network.PathCapacity(residualPath));
				network.Augment(residualPath);
				Console.ReadKey();
				residualPath = ComputeResidualPath(network);
			}
		}


		// compute residual path from source to sink using depth first search
		public virtual List<int> ComputeResidualPath(FlowGraph network)
		{
			// Subclasses need to override this to find a residual path.
			return null;
		}

	}
}

