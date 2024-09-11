using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MVA_Poe.Data
{
    public enum ReportCategory
    {

        Pothole,                // Common road maintenance issue
        StreetLight,            // Faulty or broken streetlights
        WaterLeak,              // Burst pipes or water wastage
        GarbageCollection,      // Missed or irregular waste collection
        IllegalDumping,         // Illegal waste disposal sites
        SewageLeak,             // Overflowing or leaking sewage
        PowerOutage,            // Report power cuts/load shedding issues
        Vandalism,              // Damage to public property
        RoadSignDamage,         // Broken or missing road signs
        TrafficLightFault,      // Faulty or non-functioning traffic lights
        NoiseComplaint,         // Excessive noise in residential areas
        AnimalControl,          // Stray animals or animal-related incidents
        IllegalConstruction,    // Unapproved building or construction activity
        Other
    }
}
