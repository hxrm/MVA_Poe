using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MVA_poe.Data
{
    public enum Status
    {
        [Description("Request has been submitted but not yet started")]
        Pending,    // Request has been submitted but not yet started

        [Description("The service request is being worked on")]
        Active, // The service request is being worked on

        [Description("The service request has been completed")]
        Completed,  // The service request has been completed

        [Description("The service request has been closed")]
        Closed      // The service request has been closed
    }
}
