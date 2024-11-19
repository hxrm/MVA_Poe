using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MVA_poe.Classes.SearchManagment
{
    // AVLTree class represents a self-balancing binary search tree.
    // The generic type T must implement the IComparable<T> interface.
    public class AVLTree<T> where T : IComparable<T>
    {
        // The root node of the AVL tree.
        private Node<T> root;

        // Public method to insert a value into the AVL tree.
        public void Insert(T value)
        {
            root = Insert(root, value);
        }

        // Private helper method to insert a value into the AVL tree.
        private Node<T> Insert(Node<T> node, T data)
        {
            // If the node is null, create a new node with the given data.
            if (node == null)
                return new Node<T>(data);

            // Compare the data with the current node's data.
            int compare = data.CompareTo(node.Data);
            if (compare < 0)
                // If data is less than the current node's data, insert it into the left subtree.
                node.LeftHand = Insert(node.LeftHand, data);
            else if (compare > 0)
                // If data is greater than the current node's data, insert it into the right subtree.
                node.RightHand = Insert(node.RightHand, data);
            else
                // If data is equal to the current node's data, return the current node (no duplicates).
                return node;

            // Update the height of the current node.
            node.Height = 1 + Math.Max(Height(node.LeftHand), Height(node.RightHand));
            node.Count = 1 + GetCount(node.LeftHand) + GetCount(node.RightHand); 
            // Balance the node and return the balanced node.
            return Balance(node);
        }

        // Private helper method to delete a value into the AVL tree.
        public void Delete(T value)
        {
            root = Delete(root, value);
        }

        private Node<T> Delete(Node<T> node, T data)
        {
            if (node == null)
                return node;

            int compare = data.CompareTo(node.Data);
            if (compare < 0)
            {
                node.LeftHand = Delete(node.LeftHand, data);
            }
            else if (compare > 0)
            {
                node.RightHand = Delete(node.RightHand, data);
            }
            else
            {
                if (node.LeftHand == null || node.RightHand == null)
                {
                    Node<T> temp = node.LeftHand ?? node.RightHand;
                    if (temp == null)
                    {
                        temp = node;
                        node = null;
                    }
                    else
                    {
                        node = temp;
                    }
                }
                else
                {
                    Node<T> temp = MinValueNode(node.RightHand);
                    node.Data = temp.Data;
                    node.RightHand = Delete(node.RightHand, temp.Data);
                }
            }

            if (node == null)
                return node;

            node.Height = 1 + Math.Max(Height(node.LeftHand), Height(node.RightHand));
            node.Count = 1 + GetCount(node.LeftHand) + GetCount(node.RightHand); // Update Count

            return Balance(node);
        }
        // Private helper method to get the height of a node.
        private int Height(Node<T> node)
        {
            return node?.Height ?? 0;
        }
        // Private helper method to get the count of a node.
        private int GetCount(Node<T> node)
        {
            return node?.Count ?? 0;
        }
        // Private helper method to get the min value of a node.
        private Node<T> MinValueNode(Node<T> node)
        {
            Node<T> current = node;
            while (current.LeftHand != null)
                current = current.LeftHand;
            return current;
        }

        // Private helper method to calculate the balance factor of a node.
        private int BalanceFactor(Node<T> node)
        {
            return Height(node.LeftHand) - Height(node.RightHand);
        }

        // Private helper method to balance a node.
        private Node<T> Balance(Node<T> node)
        {
            // If the node is left-heavy.
            if (BalanceFactor(node) > 1)
            {
                // If the left child is right-heavy, perform a left rotation on the left child.
                if (BalanceFactor(node.LeftHand) < 0)
                    node.LeftHand = RotateLeft(node.LeftHand);
                // Perform a right rotation on the current node.
                return RotateRight(node);
            }
            // If the node is right-heavy.
            if (BalanceFactor(node) < -1)
            {
                // If the right child is left-heavy, perform a right rotation on the right child.
                if (BalanceFactor(node.RightHand) > 0)
                    node.RightHand = RotateLeft(node.RightHand);
                // Perform a left rotation on the current node.
                return RotateLeft(node);
            }
            // Return the balanced node.
            return node;
        }

        // Private helper method to perform a right rotation.
        private Node<T> RotateRight(Node<T> y)
        {
            Node<T> x = y.LeftHand;
            Node<T> T2 = x.RightHand;

            // Perform rotation.
            x.RightHand = y;
            y.LeftHand = T2;

            // Update heights.
            y.Height = Math.Max(Height(y.LeftHand), Height(y.RightHand)) + 1;
            x.Height = Math.Max(Height(x.LeftHand), Height(x.RightHand)) + 1;

            //
            y.Count = 1 + GetCount(y.LeftHand) + GetCount(y.RightHand); 
            x.Count = 1 + GetCount(x.LeftHand) + GetCount(x.RightHand); 


            // Return the new root.
            return x;
        }

        // Private helper method to perform a left rotation.
        private Node<T> RotateLeft(Node<T> x)
        {
            Node<T> y = x.RightHand;
            Node<T> T2 = y.LeftHand;

            // Perform rotation.
            y.LeftHand = x;
            x.RightHand = T2;

            // Update heights.
            x.Height = Math.Max(Height(x.LeftHand), Height(x.RightHand)) + 1;
            y.Height = Math.Max(Height(y.LeftHand), Height(y.RightHand)) + 1;

            //
            x.Count = 1 + GetCount(x.LeftHand) + GetCount(x.RightHand); 
            y.Count = 1 + GetCount(y.LeftHand) + GetCount(y.RightHand); 


            // Return the new root.
            return y;
        }

        // Public method to perform an in-order traversal of the AVL tree.
        public IEnumerable<T> InOrderTraversal()
        {
            var result = new List<T>();
            InOrderTraversal(root, result);
            return result;
        }

        // Private helper method to perform an in-order traversal of the AVL tree.
        private void InOrderTraversal(Node<T> node, List<T> result)
        {
            if (node != null)
            {
                // Traverse the left subtree.
                InOrderTraversal(node.LeftHand, result);
                // Visit the current node.
                result.Add(node.Data);
                // Traverse the right subtree.
                InOrderTraversal(node.RightHand, result);
            }
        }

    }

}
