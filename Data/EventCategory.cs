// Import necessary namespaces
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MVA_poe.Data
{
    // Define the EventCategory enumeration
    public enum EventCategory
    {
        // Music Festivals category with description attribute
        [Description("Music Festivals")]
        MusicFestivals,

        // Community Meetings category with description attribute
        [Description("Community Meetings")]
        CommunityMeetings,

        // Sports Events category with description attribute
        [Description("Sports Events")]
        SportsEvents,

        // Cultural Exhibitions category with description attribute
        [Description("Cultural Exhibitions")]
        CulturalExhibitions,

        // Health & Wellness Workshops category with description attribute
        [Description("Health & Wellness Workshops")]
        HealthWellnessWorkshops,

        // Charity Events category with description attribute
        [Description("Charity Events")]
        CharityEvents,

        // Educational Seminars category with description attribute
        [Description("Educational Seminars")]
        EducationalSeminars,

        // Food & Craft Markets category with description attribute
        [Description("Food & Craft Markets")]
        FoodCraftMarkets,

        // Local Government Announcements category with description attribute
        [Description("Local Government Announcements")]
        LocalGovernmentAnnouncements,

        // Others category with description attribute
        [Description("Others")]
        Others
    }
}
//__---____---____---____---____---____---____---__.ooo END OF FILE ooo.__---____---____---____---____---____---____---__\\
