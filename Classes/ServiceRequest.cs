using MVA_poe.Classes.SearchManagment;
using MVA_poe.Data;
using MVA_Poe.Migrations;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Remoting.Contexts;

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

        // referencing relationship for dependencies
        public virtual ICollection<ServiceRequest> Dependencies { get; set; }
         
        public virtual ICollection<ServiceRequest> DependentOn { get; set; }


        // store the weight of the request.
        // This will give us the number of days the request has been unresolved.
        public double RequestAgeWeight => (DateTime.Now - requestDate).TotalDays; 

        // Constructor for initializing properties
        public ServiceRequest()
        {
         
        }
        // Constructor for initializing properties
        public void SetData(Report report)
        {
            AssignPriority();
            var random = new Random();
            // Assign a random employee ID (1, 2, or 3)
            int employeeId = random.Next(1, 4);
            reportId = report.reportID;
            this.report = report;
            this.employeeId = employeeId;
            this.requestDate = report.reportDate;
            this.requestUpdate = DateTime.Now;

        }

        ///AVL 
        // Implement the CompareTo method
        public int Compare(ServiceRequest x, ServiceRequest y)
        {
            if (x == null && y == null)
                return 0;
            if (x == null)
                return -1;
            if (y == null)
                return 1;

            // Compare based on status first (convert to int for numeric comparison)
            int statusComparison = ((int)x.requestStat).CompareTo((int)y.requestStat);
            if (statusComparison != 0)
                return statusComparison;

            // If statuses are equal, compare by RequestId
            return x.requestId.CompareTo(y.reportId);
        }
        public int CompareTo(ServiceRequest other)
        {
            if (other == null) return 1;

            // Compare based on requestId or any other property you prefer
            return this.requestId.CompareTo(other.requestId);
        }
        public int CompareToStat(ServiceRequest other)
        {
            int pass; 
            if (other == null)
                pass = 1;
             pass = string.Compare(this.requestStat.GetString(), other.requestStat.GetString(), StringComparison.Ordinal);
            return pass;
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
}//__---____---____---____---____---____---____---__.ooo END OF FILE ooo.__---____---____---____---____---____---____---__\\