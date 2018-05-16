using System;
using System.Collections.Generic;

namespace Graphsuche
{
    class Program
    {
        static void Main(string[] args)
        {
            var queue = new Queue<int>();
            
            queue.Enqueue(1);
            queue.Enqueue(2);
            queue.Enqueue(3);
            queue.Enqueue(4);
            
            Console.WriteLine("Hello World!");
        }
    }
}