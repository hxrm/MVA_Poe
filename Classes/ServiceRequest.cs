using MVA_poe.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MVA_Poe.Classes
{
    public class ServiceRequest : IComparable<ServiceRequest>
    {
        // Primary Key for ServiceRequest
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int requestId { get; set; }

        // Foreign Key to the Report
        [ForeignKey("report")]
        public int reportId { get; set; }

        // Reference to the Report object
        public Report report { get; set; }

        // Service request status (using the ServiceRequestStatus enum)
        public Status requestStat { get; set; }

        // Priority of the service request (using the ServiceRequestPriority enum)
        public Priority requestPri { get; set; }

        // ID of the employee assigned to the service request
        public int employeeId { get; set; }

        // Last updated date of the service request
        public DateTime requestUpdate { get; set; }
        // Last updated date of the service request
        public DateTime requestDate { get; set; }

        // Self-referencing relationship for dependencies
        
        //this request must be completed before can start
        public virtual ICollection<ServiceRequest> Dependencies { get; set; }
        // this request can start after   
        public virtual ICollection<ServiceRequest> DependentOn { get; set; }

        // 
        // Add this property to the ServiceRequest class to store the weight of the request.
        public double RequestAgeWeight => (DateTime.Now - requestDate).TotalDays; // This will give us the number of days the request has been unresolved.

        // Constructor for initializing properties
        public ServiceRequest()
        {
            // Set the last updated time to the current date and time
            requestUpdate = DateTime.Now;
            requestDate = report.reportDate;
        }


        ///AVL 
       // Implement the CompareTo method
        public int CompareTo(ServiceRequest other)
        {
            if (other == null) return 1;

            // Compare based on requestId or any other property you prefer
            return this.requestId.CompareTo(other.requestId);
        }

        // Assign priority based on status and age of request
        public void AssignPriority()
        {
            var age = DateTime.Now - requestDate;

            // Calculate the weight based on the age
            double weight = age.TotalDays;

            // Prioritize based on status and weight
            if (requestStat == Status.Pending && weight > 7)
            {
                // If unresolved for more than a week, assign top priority and high weight
                requestPri = Priority.High;
            }
            else if (requestStat == Status.Completed)
            {
                // If completed, assign no priority
                requestPri = Priority.Low;
            }
            else
            {
                // Default priority for unresolved tasks less than a week old
                requestPri = Priority.Medium;
            }

            weight = this.RequestAgeWeight;
        }
        //MTS
        // Method to calculate edge weight between two requests.
        public double CalculateDependencyWeight(ServiceRequest other)
        {
            // Calculate weight based on dependency (e.g., time or other factors)
            var weight = Math.Abs((this.requestDate - other.requestDate).TotalDays);
            return weight;  
        }
        // Implement the CompareTo method for comparison based on priority
        public int CompareToMTS(ServiceRequest other)
        {
            if (other == null) return 1;
            return this.requestPri.CompareTo(other.requestPri);
        }
    }
}
