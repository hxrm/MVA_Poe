using MVA_Poe.Classes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Markup;
using System.Xml.Linq;

namespace MVA_poe.Classes.SearchManagment
{
    // AVLTree class represents a self-balancing binary search tree.
    //public class AVLTree<T>
    //{
    public class AVLTree<T> where T : IComparable<T>
    {
        private Node<T> root; 
        private  List <ServiceRequest> avlRequests;
        public bool status;
        public bool priority;

        // Constructor: AVLTree
        // Initializes a new instance of the AVLTree class with a specified comparison delegate
        public AVLTree()
        {
            avlRequests = new List<ServiceRequest>();
        }
        //----------------------------------------------------------------------------//

        // Method: AddServiceRequest
        // Inserts a new node value, service request into the AVL tree
        public void AddServiceRequest(T key, int serviceID, ServiceRequest request)
        {
            Insert(key, serviceID, request); // Insert into AVL tree
            avlRequests.Add(request);
        }
        //----------------------------------------------------------------------------//

        // Method: Insert
        // Inserts a new value into the AVL tree
        public void Insert(T key, int requestID, ServiceRequest service)
        {
            root = Insert(root, key, requestID, service);
        }
        //----------------------------------------------------------------------------//

        // Method: Insert (private)
        // Recursively inserts a new value into the AVL tree and balances the tree
                
       private Node<T> Insert(Node<T> node, T key, int requestID, ServiceRequest service)
        {
            if (node == null)
            {
                Console.WriteLine($"Inserting new node with key: {key}");
                return new Node<T>(key, requestID, service);
            }

            Console.WriteLine($"Inserting node with key: {key}. Current node key: {node.Key}");

            // Log the state of left and right children before insertion
            Console.WriteLine($"Left child of {node.Key}: {(node.LeftHand == null ? "null" : node.LeftHand.Key.ToString())}");
            Console.WriteLine($"Right child of {node.Key}: {(node.RightHand == null ? "null" : node.RightHand.Key.ToString())}");

            int comparison = key.CompareTo(node.Key);
           
            if (status)
            {
                comparison = service.Compare(service, node.Requests[0]);
            }
            if (priority)
            {
                comparison = service.CompareToPrior(service, node.Requests[0]);
            }

            if (comparison < 0)
            {
                node.LeftHand = Insert(node.LeftHand, key, requestID, service); // Recursively insert into left subtree

            }
            else if (comparison > 0)
            {
                node.RightHand = Insert(node.RightHand, key, requestID, service); // Recursively insert into right subtree
            }

            // After insertion, log the children of the node again
            Console.WriteLine($"After insertion, left child of {node.Key}: {(node.LeftHand == null ? "null" : node.LeftHand.Key.ToString())}");
            Console.WriteLine($"After insertion, right child of {node.Key}: {(node.RightHand == null ? "null" : node.RightHand.Key.ToString())}");

            // Balance the tree after insertion
            node = Balance(node);

            return node;
        }
        //----------------------------------------------------------------------------//

        // Method: Delete 
        // Deletes a value from the AVL tree and balances the tree
        private Node<T> Delete(Node<T> node, T data)
        {
            if (node == null)
                return node;

            int compare = data.CompareTo(node.Key);
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
                    node.Key = temp.Key;
                    node.RightHand = Delete(node.RightHand, temp.Key);
                }
            }

            if (node == null)
                return node;

            node.Height = 1 + Math.Max(GetHeight(node.LeftHand), GetHeight(node.RightHand));
            // node.Count = 1 + GetCount(node.LeftHand) + GetCount(node.RightHand); // Update Count

            return Balance(node);
        }

        //----------------------------------------------------------------------------//

        // Method: MinValueNode
        // Private helper method to get the node with the minimum value in the tree
        private Node<T> MinValueNode(Node<T> node)
        {
            Node<T> current = node;
            while (current.LeftHand != null)
                current = current.LeftHand;
            return current;
        }

        //----------------------------------------------------------------------------//

        // Method: GetHeight
        // Returns the height of a node
        private int GetHeight(Node<T> node)
        {
            return node == null ? 0 : node.Height;
        }
        
       


        //----------------------------------------------------------------------------//

        // Method: Balance
        // Balances the AVL tree at the given node
        // Private helper method to balance a node.
        private Node<T> Balance(Node<T> node)
        {
            int balanceFactor = BalanceFactor(node);
            // If the node is left-heavy.
            if (balanceFactor > 1)
            {
                // If the left child is right-heavy, perform a left rotation on the left child.
                if (BalanceFactor(node.LeftHand) < 0)
                {
                    if (node.LeftHand == null || node.LeftHand.RightHand == null)
                    {
                        throw new InvalidOperationException("Left child or its right child is null during rotation.");
                    }
                    node.LeftHand = RotateLeft(node.LeftHand);
                }
                // Perform a right rotation on the current node.
                return RotateRight(node);
            }
            // If the node is right-heavy.
            if (balanceFactor < -1)
            {
                // If the right child is left-heavy, perform a right rotation on the right child.
                if (BalanceFactor(node.RightHand) > 0)
                {
                    if (node.RightHand == null || node.RightHand.LeftHand == null)
                    {
                        throw new InvalidOperationException("Right child or its left child is null during rotation.");
                    }
                    node.RightHand = RotateLeft(node.RightHand);
                }
                // Perform a left rotation on the current node.
                return RotateLeft(node);
            }
            // Already balanced
            return node;
        }


        //----------------------------------------------------------------------------//

        // Method: BalanceFactor
        // Returns the balance factor of a node
        private int BalanceFactor(Node<T> node)
        {
            return node == null ? 0 : GetHeight(node.LeftHand) - GetHeight(node.RightHand);
        }

        //----------------------------------------------------------------------------//

        // Method: RotateRight
        // Performs a right rotation on the given node
        private Node<T> RotateRight(Node<T> y)
        {
            // New root is the left child
            Node<T> newRoot = y.LeftHand; 
            if (newRoot == null)
            {
                throw new InvalidOperationException("Left child is null during right rotation.");
            }
            // Shift subtree
            y.LeftHand = newRoot.RightHand; 
           // Update parent-child relationship
            newRoot.RightHand = y;       

            // Update heights
            y.Height = Math.Max(GetHeight(y.LeftHand), GetHeight(y.RightHand)) + 1;
            newRoot.Height = Math.Max(GetHeight(newRoot.LeftHand), GetHeight(newRoot.RightHand)) + 1;

            return newRoot; // Return new root
        }


        //----------------------------------------------------------------------------//

        // Method: RotateLeft
        // Performs a left rotation on the given node
        private Node<T> RotateLeft(Node<T> x)
        {
            // New root is the right child
            Node<T> newRoot = x.RightHand; 
            if (newRoot == null)
            {
                throw new InvalidOperationException("Right child is null during left rotation.");
            }
            // Shift subtree
            x.RightHand = newRoot.LeftHand; 
            // Update parent-child relationship
            newRoot.LeftHand = x;          

            // Update heights
            x.Height = Math.Max(GetHeight(x.LeftHand), GetHeight(x.RightHand)) + 1;
            newRoot.Height = Math.Max(GetHeight(newRoot.LeftHand), GetHeight(newRoot.RightHand)) + 1;
            // Return new root
            return newRoot;
        }

        // Method: InOrderTraversal
        // Performs an in-order traversal of the AVL tree and adds service requests to the result list
        private void InOrderTraversal(Node<T> node, List<ServiceRequest> result)
        {
            if (node == null)
                return;

            // Recursively traverse the left subtree
            InOrderTraversal(node.LeftHand, result);

            // Add all service requests from the current node to the result list
            foreach (ServiceRequest request in node.Requests)
            {
                // If the service request's report ID is 0, skip it
                if (request.reportId == 0)
                {
                    continue;
                }

                result.Add(request);
            }

            // Recursively traverse the right subtree
            InOrderTraversal(node.RightHand, result);
        }

        //----------------------------------------------------------------------------//

        // Method: GetSortedServiceRequests
        // Creates and returns a sorted list of service requests based on their IDs
        public List<ServiceRequest> GetSortedServiceRequests()
        {
            List<ServiceRequest> sortedRequests = new List<ServiceRequest>();
            // Perform in-order traversal to populate the sortedRequests list
            InOrderTraversal(root, sortedRequests);

            return sortedRequests;
        }

    }

    //----------------------------------------------------------------------------//

}
//__---____---____---____---____---____---____---__.ooo END OF FILE ooo.__---____---____---____---____---____---____---__\\