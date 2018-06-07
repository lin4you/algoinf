//**************************************
// This datastructure associates indices
// (integers) with real-valued keys (doubles)
// and stores them in a sorted fashion.
//
// implements indexed binary heap
// with known number N of maximum entries
// indices running from 0 .. N-1
// sort keys of type double
// via arrays
//**************************************

using System;
using System.Collections.Generic;

namespace IndexedPriorityQueue
{
	public class PriorityHeap : IndexedPriorityQueue
	{
		readonly private int N;			// maximum number of entries
		readonly private double[] keys;	// key of heap vertex
		readonly private int[] index;	// index of heap vertex
		readonly private int[] pos;		// position of index i in heap
		private int last;				// first empty position

//		public static void Main()
//		{
//			PriorityHeap heap = new PriorityHeap (10);
//			heap.Insert (0, 1);
//			//heap.Output ();
//			heap.Insert (1, 3);
//			//heap.Output ();
//			heap.Insert (2, 2);
//			//heap.Output ();
//			heap.Insert (3, 5);
//			heap.Output ();
////			heap.Exchange (1, 2);
////			heap.Output ();
//			int min = heap.DeleteMin ();
//			System.Console.WriteLine(min);
//
//			heap.Output ();
//
//		}

		//create empty heap
		public PriorityHeap(int N)
		{
			this.N = N;
			keys = new double[N];
			index = new int[N];
			pos = new int[N];
			last = 0;
			for (int i = 0; i < N; i++) pos[i] = -1;
		}
		//test if heap is empty
		public bool Empty()
		{
			return last == 0;
		}
		//test if index is contained
		public bool Contains(int index)
		{
			if (pos[index] != -1) return true;
			return false;
		}
		//delete root as minimum key
		public int DeleteMin()
		{
			if (last == 0) throw new ApplicationException("Empty Heap");
			int min = index[0];
			pos[min] = -1;
			keys[0] = 0;
			index[0] = 0;
			last--;
			if (last > 0)
			{
				keys[0] = keys[last]; keys[last] = 0;
				index[0] = index[last]; index[last] = 0;
				pos[index[0]] = 0;
				Sink(0);
			}
			return min;
		}
		// get key for index
		public double GetKey(int index)
		{
			return keys[pos[index]];
		}
		//insert new key
		public void Insert(int index, double key)
		{
			if (!TestRange(index)) return;
			if (Contains (index)) return;
			pos[index] = last;
			keys[last] = key;
			this.index[last] = index;
			last++;
			Swim(last - 1);
		}
		//change value of key
		public void Change(int index, double key)
		{
			if (!TestRange(index)) return;
			if (!Contains(index)) return;
			double oldKey = keys[pos[index]];
			keys[pos[index]] = key;
			if (oldKey > key) Swim(pos[index]);
			if (oldKey < key) Sink (pos[index]);
		}

		public void Output()
		{
			System.Console.WriteLine("N " + N + " last " + last);
			for (int i = 0; i < N; i++)
				System.Console.WriteLine(keys[i] + " " + index[i] + " " + pos[i]);
		}
		
		private bool TestRange(int i)
		{
			if (i < 0) return false;
			if (i >= N) return false;
			return true;
		}
		private void Exchange(int i, int j)
		{
			pos[index[i]] = j;
			pos[index[j]] = i;
			double tmpKey = keys[i]; keys[i] = keys[j]; keys[j] = tmpKey;
			int tmpIndex = index[i]; index[i] = index[j]; index[j] = tmpIndex;
		}
		private void Sink(int v)  // 2*v+1 and 2*v+2 are v's children (if existing)
		{
			//if (2*v >= last) return;
			if (2*v+1 >= last) return;
			//int min = 2*v;
			int min = 2*v+1;
			//if (((2*v + 1) < last) && (keys[2*v] > keys[2*v+1])) min = 2*v + 1;
			if (((2*v+2) < last) && (keys[2*v+1] > keys[2*v+2])) min = 2*v+2;
			if (keys[v] > keys[min])
			{
				Exchange(v, min);
				Sink(min);
			}
		}
		private void Swim(int v) // calc parent (v-1)/2 (1 ist parent of 3 and 4)
		{
			if (v == 0) return;
			int parent = (v-1)/2;
			//if (keys[v] < keys[v/2])
			if (keys[v] < keys[parent])
			{
				//Exchange(v, v/2);
				//Swim(v/2);
				Exchange(v, parent);
				Swim(parent);
			}
		}
	}
}
