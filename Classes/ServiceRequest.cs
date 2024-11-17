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

        // Self-referencing relationship for dependencies
        public virtual ICollection<ServiceRequest> Dependencies { get; set; }
        public virtual ICollection<ServiceRequest> DependentOn { get; set; }


        // Constructor for initializing properties
        public ServiceRequest()
        {
            // Set the last updated time to the current date and time
            requestUpdate = DateTime.Now;
        }


        ///AVL 
       // Implement the CompareTo method
        public int CompareTo(ServiceRequest other)
        {
            if (other == null) return 1;

            // Compare based on requestId or any other property you prefer
            return this.requestId.CompareTo(other.requestId);
        }
    }
}
