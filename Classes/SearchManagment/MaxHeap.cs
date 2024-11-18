using MVA_Poe.Classes;
using System;

namespace MVA_poe.Classes.SearchManagment
{
    public class MaxHeap
    {
        private ServiceRequest[] heap;
        private int size;
        private int capacity;


        // Constructor to initialize heap with a default or specified capacity
        public MaxHeap(int capacity = 10)
        {
            this.capacity = capacity;
            heap = new ServiceRequest[capacity];
            size = 0;
        }
        // Method to insert a ServiceRequest into the heap
        public void Insert(ServiceRequest request)
        {
            // Ensure priority is assigned before insertion
            request.AssignPriority();

            if (size == capacity)
            {
                Resize();
            }

            heap[size] = request;
            HeapifyUp(size);
            size++;
        }
        // Method to extract the maximum priority service request
        public ServiceRequest GetTopPriority()
        {
            if (size == 0) throw new InvalidOperationException("Heap is empty");

            var max = heap[0];
            heap[0] = heap[size - 1];
            size--;
            HeapifyDown(0);

            return max;
        }
        // Method to convert the heap into an array, sorted by priority
        public ServiceRequest[] ToArray()
        {
            ServiceRequest[] result = new ServiceRequest[size];
            Array.Copy(heap, result, size);
            Array.Sort(result, (x, y) => y.requestPri.CompareTo(x.requestPri)); // Sorting for non-heap purposes
            return result;
        }

        // Method to maintain the heap property while moving elements up
        private void HeapifyUp(int index)
        {
            while (index > 0)
            {
                int parentIndex = (index - 1) / 2;  // Calculate parent index
                if (heap[index].requestPri <= heap[parentIndex].requestPri) break;  // No need to swap if heap property is satisfied

                Swap(index, parentIndex);  // Swap with parent if priority is higher
                index = parentIndex;  // Move index to parent
            }
        }
        // Method to maintain the heap property while moving elements down
        private void HeapifyDown(int index)
        {
            while (index < size / 2)  // Only check nodes with children
            {
                int leftChildIndex = 2 * index + 1;
                int rightChildIndex = 2 * index + 2;
                int largestIndex = index;

                if (leftChildIndex < size && heap[leftChildIndex].requestPri > heap[largestIndex].requestPri)
                {
                    largestIndex = leftChildIndex;
                }

                if (rightChildIndex < size && heap[rightChildIndex].requestPri > heap[largestIndex].requestPri)
                {
                    largestIndex = rightChildIndex;
                }

                if (largestIndex == index) break;

                Swap(index, largestIndex);
                index = largestIndex;
            }
        }
        // Method to maintain the heap property while moving elements down
        private void Swap(int index1, int index2)
        {
            var temp = heap[index1];
            heap[index1] = heap[index2];
            heap[index2] = temp;
        }
        // Resize the heap when the capacity is exceeded
        private void Resize()
        {
            capacity *= 2;
            Array.Resize(ref heap, capacity);
        }
    }
}