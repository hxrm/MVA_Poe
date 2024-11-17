using System.ComponentModel;

namespace MVA_Poe.Data
{
    // Define an enumeration named ReportCategory
    public enum ReportCategory
    {
        [Description("Pothole")]
        Pothole,

        [Description("Street Light")]
        StreetLight,

        [Description("Water Leak")]
        WaterLeak,

        [Description("Garbage Collection")]
        GarbageCollection,

        [Description("Illegal Dumping")]
        IllegalDumping,

        [Description("Sewage Leak")]
        SewageLeak,

        [Description("Power Outage")]
        PowerOutage,

        [Description("Vandalism")]
        Vandalism,

        [Description("Road Sign Damage")]
        RoadSignDamage,

        [Description("Traffic Light Fault")]
        TrafficLightFault,

        [Description("Noise Complaint")]
        NoiseComplaint,

        [Description("Animal Control")]
        AnimalControl,

        [Description("Illegal Construction")]
        IllegalConstruction,
        [Description("All")]
        All,
        [Description("Other")]
        Other
    }
}

//__---____---____---____---____---____---____---__.ooo END OF FILE ooo.__---____---____---____---____---____---____---__\\
