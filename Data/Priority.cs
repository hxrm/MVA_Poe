using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MVA_poe.Data
{
    public enum ServiceRequestPriority
    {
        [Description("Low priority")]
        Low = 1,    // Low priority

        [Description("Medium priority")]
        Medium = 2, // Medium priority

        [Description("High priority")]
        High = 3    // High priority
    }
}
