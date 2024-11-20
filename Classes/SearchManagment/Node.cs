using MVA_Poe.Classes;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MVA_poe.Classes.SearchManagment
{
    public class Node<T>
    {
        // The Data property holds the value stored in the node.
        public T Key { get; set; }

        // The Left property points to the left child node.
        public int serviceID { get; set; }
        public Node<T> LeftHand { get; set; }

        // The Right property points to the right child node.
        public Node<T> RightHand { get; set; }

        // The Height property stores the height of the node in the tree.
        // This is used to maintain the balance of the AVL tree.
        public int Height { get; set; }

        public List<ServiceRequest> Requests = new List<ServiceRequest>();

        // Constructor to initialize the node with a given data value.
        // The height is initialized to 1 because a new node is initially a leaf node.
        public Node()
        {
           
            Requests = new List<ServiceRequest>(); // Initialize the list
        }
        public Node(T data, int serviceID, ServiceRequest service)
        {
            Key = data;
            //leaf node
            Height = 1;
            this.serviceID = serviceID;
            Requests.Add(service); // Initialize the list
        }
    }

}
//__---____---____---____---____---____---____---__.ooo END OF FILE ooo.__---____---____---____---____---____---____---__\\