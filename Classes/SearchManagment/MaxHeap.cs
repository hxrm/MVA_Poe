using MVA_Poe.Classes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MVA_poe.Classes.SearchManagment
{
        public class MaxHeap
        {
            private ServiceRequest[] heap;
            private int size;
            private int capacity;

            public MaxHeap(int capacity = 10)
            {
                this.capacity = capacity;
                heap = new ServiceRequest[capacity];
                size = 0;
            }

            public void Insert(ServiceRequest request)
            {
                if (size == capacity)
                {
                    Resize();
                }

                heap[size] = request;
                HeapifyUp(size);
                size++;
            }

            public ServiceRequest ExtractMax()
            {
                if (size == 0) throw new InvalidOperationException("Heap is empty");

                var max = heap[0];
                heap[0] = heap[size - 1];
                size--;
                HeapifyDown(0);

                return max;
            }

            public ServiceRequest[] ToArray()
            {
                ServiceRequest[] result = new ServiceRequest[size];
                Array.Copy(heap, result, size);
                Array.Sort(result, (x, y) => y.requestPri.CompareTo(x.requestPri));
                return result;
            }

            private void HeapifyUp(int index)
            {
                while (index > 0)
                {
                    int parentIndex = (index - 1) / 2;
                    if (heap[index].requestPri <= heap[parentIndex].requestPri) break;

                    Swap(index, parentIndex);
                    index = parentIndex;
                }
            }

            private void HeapifyDown(int index)
            {
                while (index < size)
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

            private void Swap(int index1, int index2)
            {
                var temp = heap[index1];
                heap[index1] = heap[index2];
                heap[index2] = temp;
            }

            private void Resize()
            {
                capacity *= 2;
                Array.Resize(ref heap, capacity);
            }
        }
    }


