using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MVA_poe.Classes.SearchManagment
{
    public class Node<T> where T : IComparable<T>
    {
        // The Data property holds the value stored in the node.
        public T Data { get; set; }

        // The Left property points to the left child node.
        public Node<T> LeftHand { get; set; }

        // The Right property points to the right child node.
        public Node<T> RightHand { get; set; }

        // The Height property stores the height of the node in the tree.
        // This is used to maintain the balance of the AVL tree.
        public int Height { get; set; }

        public int Count { get; set; }

        // Constructor to initialize the node with a given data value.
        // The height is initialized to 1 because a new node is initially a leaf node.
        public Node(T data)
        {
            Data = data;
            Height = 1;
            Count = 1;
        }
    }

}
