using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MVA_poe.Data
{
    public enum EventCategory
    {
        [Description("Music Festivals")]
        MusicFestivals,

        [Description("Community Meetings")]
        CommunityMeetings,

        [Description("Sports Events")]
        SportsEvents,

        [Description("Cultural Exhibitions")]
        CulturalExhibitions,

        [Description("Health & Wellness Workshops")]
        HealthWellnessWorkshops,

        [Description("Charity Events")]
        CharityEvents,

        [Description("Educational Seminars")]
        EducationalSeminars,

        [Description("Food & Craft Markets")]
        FoodCraftMarkets,

        [Description("Local Government Announcements")]
        LocalGovernmentAnnouncements,

        [Description("Others")]
        Others


    }

}
