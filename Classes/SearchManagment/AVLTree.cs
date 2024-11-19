using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MVA_poe.Classes.SearchManagment
{
    // AVLTree class represents a self-balancing binary search tree.
    public class AVLTree<T>
    {
        private Node<T> root; // Root node of the AVL tree
        private readonly Comparison<T> comparison; // Comparison delegate to compare node values

        // Constructor: AVLTree
        // Initializes a new instance of the AVLTree class with a specified comparison delegate
        public AVLTree(Comparison<T> comparison)
        {
            this.comparison = comparison;
        }

        //----------------------------------------------------------------------------//

        // Method: Insert
        // Inserts a new value into the AVL tree
        public void Insert(T value)
        {
            root = Insert(root, value);
        }

        //----------------------------------------------------------------------------//

        // Method: Insert (private)
        // Recursively inserts a new value into the AVL tree and balances the tree
        private Node<T> Insert(Node<T> node, T data)
        {
            if (node == null)
                return new Node<T>(data); // Create a new node if the current node is null

            int compare = comparison(data, node.Data);
            if (compare < 0)
                node.LeftHand = Insert(node.LeftHand, data); // Insert into the left subtree
            else if (compare > 0)
                node.RightHand = Insert(node.RightHand, data); // Insert into the right subtree
            else
                return node; // Duplicate values are not allowed

            // Update the height and count of the current node
            node.Height = 1 + Math.Max(Height(node.LeftHand), Height(node.RightHand));
            node.Count = 1 + GetCount(node.LeftHand) + GetCount(node.RightHand);

            // Print the balance factor of the current node
            Console.WriteLine($"Node with data {node.Data}: Balance Factor = {BalanceFactor(node)}");

            return Balance(node); // Balance the tree
        }

        //----------------------------------------------------------------------------//

        // Method: Height
        // Returns the height of a node
        private int Height(Node<T> node)
        {
            return node?.Height ?? 0;
        }

        //----------------------------------------------------------------------------//

        // Method: GetCount
        // Returns the count of nodes in the subtree rooted at the given node
        private int GetCount(Node<T> node)
        {
            return node?.Count ?? 0;
        }

        //----------------------------------------------------------------------------//

        // Method: Balance
        // Balances the AVL tree at the given node
        private Node<T> Balance(Node<T> node)
        {
            if (BalanceFactor(node) > 1)
            {
                if (BalanceFactor(node.LeftHand) < 0)
                {
                    Console.WriteLine($"Left-Right rotation needed at node with data {node.Data}");
                    node.LeftHand = RotateLeft(node.LeftHand); // Perform left rotation on left child
                }
                Console.WriteLine($"Right rotation needed at node with data {node.Data}");
                return RotateRight(node); // Perform right rotation
            }
            if (BalanceFactor(node) < -1)
            {
                if (BalanceFactor(node.RightHand) > 0)
                {
                    Console.WriteLine($"Right-Left rotation needed at node with data {node.Data}");
                    node.RightHand = RotateLeft(node.RightHand); // Perform left rotation on right child
                }
                Console.WriteLine($"Left rotation needed at node with data {node.Data}");
                return RotateLeft(node); // Perform left rotation
            }
            return node; // Return the balanced node
        }

        //----------------------------------------------------------------------------//

        // Method: BalanceFactor
        // Returns the balance factor of a node
        private int BalanceFactor(Node<T> node)
        {
            return Height(node.LeftHand) - Height(node.RightHand);
        }

        //----------------------------------------------------------------------------//

        // Method: RotateRight
        // Performs a right rotation on the given node
        private Node<T> RotateRight(Node<T> y)
        {
            Node<T> x = y.LeftHand;
            Node<T> T2 = x.RightHand;

            // Perform rotation
            x.RightHand = y;
            y.LeftHand = T2;

            // Update heights
            y.Height = Math.Max(Height(y.LeftHand), Height(y.RightHand)) + 1;
            x.Height = Math.Max(Height(x.LeftHand), Height(x.RightHand)) + 1;

            // Update counts
            y.Count = 1 + GetCount(y.LeftHand) + GetCount(y.RightHand);
            x.Count = 1 + GetCount(x.LeftHand) + GetCount(x.RightHand);

            return x; // Return the new root
        }

        //----------------------------------------------------------------------------//

        // Method: RotateLeft
        // Performs a left rotation on the given node
        private Node<T> RotateLeft(Node<T> x)
        {
            Node<T> y = x.RightHand;
            Node<T> T2 = y.LeftHand;

            // Perform rotation
            y.LeftHand = x;
            x.RightHand = T2;

            // Update heights
            x.Height = Math.Max(Height(x.LeftHand), Height(x.RightHand)) + 1;
            y.Height = Math.Max(Height(y.LeftHand), Height(y.RightHand)) + 1;

            // Update counts
            x.Count = 1 + GetCount(x.LeftHand) + GetCount(x.RightHand);
            y.Count = 1 + GetCount(y.LeftHand) + GetCount(y.RightHand);

            return y; // Return the new root
        }

        //----------------------------------------------------------------------------//

        // Method: InOrderTraversal
        // Performs an in-order traversal of the AVL tree and returns the elements
        public IEnumerable<T> InOrderTraversal()
        {
            var result = new List<T>();
            InOrderTraversal(root, result);
            return result;
        }

        //----------------------------------------------------------------------------//

        // Method: InOrderTraversal (private)
        // Recursively performs an in-order traversal of the AVL tree
        private void InOrderTraversal(Node<T> node, List<T> result)
        {
            if (node != null)
            {
                InOrderTraversal(node.LeftHand, result); // Traverse the left subtree
                result.Add(node.Data); // Visit the node
                InOrderTraversal(node.RightHand, result); // Traverse the right subtree
            }
        }
    }

    //----------------------------------------------------------------------------//

}
//__---____---____---____---____---____---____---__.ooo END OF FILE ooo.__---____---____---____---____---____---____---__\\