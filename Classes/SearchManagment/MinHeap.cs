using MVA_Poe.Classes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MVA_poe.Classes.SearchManagment
{

    public class MinHeap
    {
        private ServiceRequest[] heap;
        private int size;
        private const int InitialCapacity = 10;

        public MinHeap()
        {
            heap = new ServiceRequest[InitialCapacity];
            size = 0;
        }

        public void Enqueue(ServiceRequest request)
        {
            if (size == heap.Length)
            {
                Resize();
            }

            heap[size] = request;
            HeapifyUp(size);
            size++;
        }

        public ServiceRequest Dequeue()
        {
            if (size == 0)
                throw new InvalidOperationException("Heap is empty.");

            var root = heap[0];
            heap[0] = heap[size - 1];
            size--;
            HeapifyDown(0);

            return root;
        }


        public bool IsEmpty()
        {
            return size == 0;
        }

        private void HeapifyUp(int index)
        {
            while (index > 0)
            {
                int parentIndex = (index - 1) / 2;
                if (heap[index].requestPri >= heap[parentIndex].requestPri) break;

                Swap(index, parentIndex);
                index = parentIndex;
            }
        }

        private void HeapifyDown(int index)
        {
            int lastIndex = size - 1;
            while (index < lastIndex)
            {
                int leftChildIndex = 2 * index + 1;
                int rightChildIndex = 2 * index + 2;
                int smallestIndex = index;

                if (leftChildIndex <= lastIndex && heap[leftChildIndex].requestPri < heap[smallestIndex].requestPri)
                {
                    smallestIndex = leftChildIndex;
                }

                if (rightChildIndex <= lastIndex && heap[rightChildIndex].requestPri < heap[smallestIndex].requestPri)
                {
                    smallestIndex = rightChildIndex;
                }

                if (smallestIndex == index) break;

                Swap(index, smallestIndex);
                index = smallestIndex;
            }
        }

        private void Swap(int index1, int index2)
        {
            var temp = heap[index1];
            heap[index1] = heap[index2];
            heap[index2] = temp;
        }

        private void Resize()
        {
            var newHeap = new ServiceRequest[heap.Length * 2];
            Array.Copy(heap, newHeap, heap.Length);
            heap = newHeap;
        }
    }
}