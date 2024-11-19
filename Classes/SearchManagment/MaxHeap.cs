using MVA_Poe.Classes;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MVA_poe.Classes.SearchManagment
{
    public class MaxHeap
    {
        private Dictionary<int, ServiceRequest> heap;  // Changed to Dictionary
        private int size;

        // Constructor to initialize heap (no need for capacity)
        public MaxHeap()
        {
            heap = new Dictionary<int, ServiceRequest>();
            size = 0;
        }

        // Method to insert a ServiceRequest into the heap
        public void Insert(ServiceRequest request)
        {
            // Ensure priority is assigned before insertion
            request.AssignPriority();

            heap[request.requestId] = request; // Use requestId as the key
            size++;
            HeapifyUp(request.requestId);  // Heapify using requestId
        }

        // Method to extract the maximum priority service request
        public ServiceRequest GetTopPriority()
        {
            if (size == 0) throw new InvalidOperationException("Heap is empty");

            // Find the service request with the maximum priority
            var maxRequest = heap.Values.OrderByDescending(r => r.requestPri).FirstOrDefault();

            heap.Remove(maxRequest.requestId);  // Remove from dictionary
            size--;
            return maxRequest;
        }

       // Method to convert the heap into an array, sorted by priority
        public ServiceRequest[] ToArray()
        {
            return heap.Values.OrderByDescending(r => r.requestPri).ToArray();  // Sort by priority
        }


        public Dictionary<int, ServiceRequest> ToDictionary()
        {
            // Sort the heap by priority in descending order (highest priority first)
            var sortedHeap = heap.Values
                                  .OrderByDescending(r => r.requestPri)
                                  .ToDictionary(r => r.requestId);

            return sortedHeap;
        }


        // Method to maintain the heap property while moving elements up
        private void HeapifyUp(int requestId)
        {
            // Find the request to be heapified
            var request = heap[requestId];
            var parentId = GetParentId(requestId);

            while (parentId != -1 && heap[parentId].requestPri < request.requestPri)
            {
                // Swap with parent if priority is higher
                Swap(requestId, parentId);
                requestId = parentId;
                parentId = GetParentId(requestId);
            }
        }

        // Method to maintain the heap property while moving elements down
        private void HeapifyDown(int requestId)
        {
            int leftChildId = GetLeftChildId(requestId);
            int rightChildId = GetRightChildId(requestId);

            // Find the highest priority among the current node, left and right children
            int highestPriorityChildId = requestId;

            if (leftChildId != -1 && heap[leftChildId].requestPri > heap[highestPriorityChildId].requestPri)
                highestPriorityChildId = leftChildId;

            if (rightChildId != -1 && heap[rightChildId].requestPri > heap[highestPriorityChildId].requestPri)
                highestPriorityChildId = rightChildId;

            if (highestPriorityChildId != requestId)
            {
                // Swap with the child if its priority is higher
                Swap(requestId, highestPriorityChildId);
                HeapifyDown(highestPriorityChildId);  // Recursively heapify down
            }
        }

        // Method to get the parent ID of the current request
        private int GetParentId(int requestId)
        {
            int parentIndex = (requestId - 1) / 2;
            return heap.ContainsKey(parentIndex) ? parentIndex : -1;
        }

        // Method to get the left child ID of the current request
        private int GetLeftChildId(int requestId)
        {
            int leftChildIndex = 2 * requestId + 1;
            return heap.ContainsKey(leftChildIndex) ? leftChildIndex : -1;
        }

        // Method to get the right child ID of the current request
        private int GetRightChildId(int requestId)
        {
            int rightChildIndex = 2 * requestId + 2;
            return heap.ContainsKey(rightChildIndex) ? rightChildIndex : -1;
        }

        // Swap two elements in the dictionary (based on requestId)
        private void Swap(int requestId1, int requestId2)
        {
            var temp = heap[requestId1];
            heap[requestId1] = heap[requestId2];
            heap[requestId2] = temp;
        }
    }
}
