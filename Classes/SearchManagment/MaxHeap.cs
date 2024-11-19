using MVA_Poe.Classes;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MVA_poe.Classes.SearchManagment
{
    public class MaxHeap
    {
        // Dictionary to store the heap elements with requestId as the key
        private Dictionary<int, ServiceRequest> heap;

        //----------------------------------------------------------------------------//
        // Method: MaxHeap
        // Constructor to initialize heap (no need for capacity)
        public MaxHeap()
        {
            // Initialize the heap as a dictionary
            heap = new Dictionary<int, ServiceRequest>();
        }

        //----------------------------------------------------------------------------//
        // Method: Insert
        // Method to insert a ServiceRequest into the heap
        public void Insert(ServiceRequest request)
        {
            // Ensure priority is assigned before insertion
            request.AssignPriority();

            // Use requestId as the key and add the request to the heap
            heap[request.requestId] = request;
            // Heapify using requestId to maintain heap property
            HeapifyUp(request.requestId);
        }

        //----------------------------------------------------------------------------//
        // Method: GetTopPriority
        // Method to extract the maximum priority service request
        public ServiceRequest GetTopPriority()
        {
            // Throw an exception if the heap is empty
            if (heap.Count == 0) throw new InvalidOperationException("Heap is empty");

            // Find the service request with the maximum priority
            var maxRequest = heap.Values.OrderByDescending(r => r.requestPri).FirstOrDefault();

            // Remove the maximum priority request from the dictionary
            heap.Remove(maxRequest.requestId);

            // Return the maximum priority request
            return maxRequest;
        }

        //----------------------------------------------------------------------------//
        // Method: ToArray
        // Method to convert the heap into an array, sorted by priority without using the dictionary to sort it
        public ServiceRequest[] ToArray()
        {
            // Create a temporary list to store the sorted elements
            List<ServiceRequest> sortedList = new List<ServiceRequest>();

            // Create a copy of the heap to avoid modifying the original heap
            var tempHeap = new Dictionary<int, ServiceRequest>(heap);

            // Extract the maximum element from the heap until it is empty
            while (heap.Count > 0)
            {
                // Get the top priority service request
                var topPriorityRequest = GetTopPriority();
                // Add the top priority service request to the sorted list
                sortedList.Add(topPriorityRequest);
            }

            // Restore the original heap
            heap = tempHeap;

            // Convert the sorted list to an array and return it
            return sortedList.ToArray();
        }

        //----------------------------------------------------------------------------//
        // Method: ToStatusArray
        // Method to convert the heap into an array, sorted by status
        public ServiceRequest[] ToStatusArrayUsingAVL()
        {
            // Create an AVL Tree to store the service requests, using CompareToStat for comparisons
            AVLTree<ServiceRequest> avlTree = new AVLTree<ServiceRequest>((x, y) => x.CompareToStat(y));

            // Insert all service requests from the heap into the AVL Tree
            foreach (var request in heap.Values)
            {
                avlTree.Insert(request);
                Console.WriteLine($"Inserted request with ID: {request.requestId} and Status: {request.requestStat}");
            }

            // Perform an in-order traversal of the AVL Tree to get the sorted list
            var sortedList = avlTree.InOrderTraversal().ToList();

            // Debugging: Print the sorted list
            foreach (var request in sortedList)
            {
                Console.WriteLine($"Sorted request with ID: {request.requestId} and Status: {request.requestStat}");
            }

            // Convert the sorted list to an array and return it
            return sortedList.ToArray();
        }


        //----------------------------------------------------------------------------//
        // Method: ToDictionary
        // Method to convert the heap into a dictionary, sorted by priority
        public Dictionary<int, ServiceRequest> ToDictionary()
        {
            // Sort the heap by priority in descending order and convert to dictionary
            var sortedHeap = heap.Values
                                  .OrderByDescending(r => r.requestPri)
                                  .ToDictionary(r => r.requestId);

            // Return the sorted dictionary
            return sortedHeap;
        }

        //----------------------------------------------------------------------------//
        // Method: HeapifyUp
        // Method to maintain the heap property while moving elements up
        private void HeapifyUp(int requestId)
        {
            // Find the request to be heapified
            var request = heap[requestId];
            // Get the parent ID of the current request
            var parentId = GetParentId(requestId);

            // Continue heapifying up until the heap property is restored
            while (parentId != -1 && heap[parentId].requestPri < request.requestPri)
            {
                // Swap with parent if priority is higher
                Swap(requestId, parentId);
                // Update the requestId to the parentId
                requestId = parentId;
                // Get the new parent ID
                parentId = GetParentId(requestId);
            }
        }

        //----------------------------------------------------------------------------//
        // Method: HeapifyDown
        // Method to maintain the heap property while moving elements down
        private void HeapifyDown(int requestId)
        {
            // Get the left child ID of the current request
            int leftChildId = GetLeftChildId(requestId);
            // Get the right child ID of the current request
            int rightChildId = GetRightChildId(requestId);

            // Find the highest priority among the current node, left and right children
            int highestPriorityChildId = requestId;

            // Update the highest priority child ID if the left child has higher priority
            if (leftChildId != -1 && heap[leftChildId].requestPri > heap[highestPriorityChildId].requestPri)
                highestPriorityChildId = leftChildId;

            // Update the highest priority child ID if the right child has higher priority
            if (rightChildId != -1 && heap[rightChildId].requestPri > heap[highestPriorityChildId].requestPri)
                highestPriorityChildId = rightChildId;

            // Swap with the child if its priority is higher and recursively heapify down
            if (highestPriorityChildId != requestId)
            {
                Swap(requestId, highestPriorityChildId);
                HeapifyDown(highestPriorityChildId);
            }
        }

        //----------------------------------------------------------------------------//
        // Method: GetParentId
        // Method to get the parent ID of the current request
        private int GetParentId(int requestId)
        {
            // Calculate the parent index
            int parentIndex = (requestId - 1) / 2;
            // Return the parent index if it exists in the heap, otherwise return -1
            return heap.ContainsKey(parentIndex) ? parentIndex : -1;
        }

        //----------------------------------------------------------------------------//
        // Method: GetLeftChildId
        // Method to get the left child ID of the current request
        private int GetLeftChildId(int requestId)
        {
            // Calculate the left child index
            int leftChildIndex = 2 * requestId + 1;
            // Return the left child index if it exists in the heap, otherwise return -1
            return heap.ContainsKey(leftChildIndex) ? leftChildIndex : -1;
        }

        //----------------------------------------------------------------------------//
        // Method: GetRightChildId
        // Method to get the right child ID of the current request
        private int GetRightChildId(int requestId)
        {
            // Calculate the right child index
            int rightChildIndex = 2 * requestId + 2;
            // Return the right child index if it exists in the heap, otherwise return -1
            return heap.ContainsKey(rightChildIndex) ? rightChildIndex : -1;
        }

        //----------------------------------------------------------------------------//
        // Method: Swap
        // Swap two elements in the dictionary (based on requestId)
        private void Swap(int requestId1, int requestId2)
        {
            // Temporarily store the first request
            var temp = heap[requestId1];
            // Swap the first request with the second request
            heap[requestId1] = heap[requestId2];
            // Assign the temporarily stored request to the second request's position
            heap[requestId2] = temp;
        }
    }
}
