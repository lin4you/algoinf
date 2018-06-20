namespace IndexedPriorityQueue {

	public interface IndexedPriorityQueue {

		//test if Queue is empty
		bool Empty();

		//test if index is contained
		bool Contains(int index);

		//delete minimum key and return the index
		int DeleteMin();

		//insert new key
		void Insert(int index, double key);

		//change value of key
		void Change(int index, double key);

		//print the Queue to the console.
		void Output();
	}
}
