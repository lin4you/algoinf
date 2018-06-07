using System;

namespace IndexedPriorityQueue {

	public class PriorityArray : IndexedPriorityQueue {
		readonly private double[] priorities;

		public PriorityArray(int N)
		{
			priorities = new double[N];
			for(int i = 0; i < priorities.Length; i++)
			{
				priorities[i] = Double.PositiveInfinity;
			}
		}

		//test if array is empty
		public bool Empty()
		{
			for (int i = 0; i < priorities.Length; i++)
			{
				if(Contains(i)) return false;
			}
			
			return true;
		}
		//test if index is contained
		public bool Contains(int index)
		{
			return priorities[index] < Double.PositiveInfinity;
		}
		
		//delete minimum key and return the index of that key
		public int DeleteMin()
		{
			double min = Double.PositiveInfinity;
			int index = -1;
			for (int i = 0; i < priorities.Length; i++)
			{
				if (priorities[i] < min)
				{
					min = priorities[i];
					index = i;
				}
			}
			
			priorities[index] = Double.PositiveInfinity;

			return index;
		}
		//insert new key
		public void Insert(int index, double key)
		{
			priorities[index] = key;
		}
		//change value of key
		public void Change(int index, double key)
		{
			priorities[index] = key;
		}

		public void Output()
		{
			for (int i = 0; i < priorities.Length; i++)
				System.Console.WriteLine(i + " " + priorities[i]);
		}
	}
}
