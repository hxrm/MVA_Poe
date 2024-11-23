// Import necessary namespaces
using MVA_poe.Data;
using MVA_poe.Pages;
using MVA_Poe.Classes;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Globalization;
using System.Linq;

namespace MVA_poe.Classes
{
    // Class to generate event recommendations based on user patterns
    public class RecommendationEngine
    {
        // Set for unique categories
        private HashSet<EventCategory> uniqueCategories;

        // Set for unique dates
        private HashSet<DateTime> uniqueDates;

        // Queue to store event recommendations
        private Queue<Event> eventRecommendations;
        // Queue to store event recommendations
        private Queue<Event> eventRelated;

        // Static property to enable or disable recommendations
        public static bool recommendationEnabled { get; set; }

        // List to store preferred days
        public List<string> dayPreference = new List<string>();

        // List to store preferred categories
        public List<EventCategory> categoryPreference = new List<EventCategory>();

        // List to store pattern frequencies
        private List<PatternFrequency> pattern = new List<PatternFrequency>();

        // Constructor to initialize the recommendation engine
        public RecommendationEngine()
        {
            // Initialize unique categories set
            uniqueCategories = new HashSet<EventCategory>();

            // Initialize unique dates set
            uniqueDates = new HashSet<DateTime>();

            // Initialize event recommendations queue
            eventRecommendations = new Queue<Event>();
            // Initialize event related queue
            eventRelated = new Queue<Event>();

            // Get user pattern data
            GetPatternData();

            // Generate day recommendations
            GenerateDayRecommendation();

            // Generate category recommendations
            GenerateCatRecommendation();

            // Populate event recommendations
            PopulateEventRecommendations();
            // Populate event related
            PopulateEventRelated();
        }

        //----------------------------------------------------------------------------

        // Method to get user pattern data from the database if it exists
        private void GetPatternData()
        {
            // Use a database context to access the database
            using (var context = new AppDbContext())
            {
                // Query the database for pattern frequencies for the current user
                pattern = context.Database.SqlQuery<PatternFrequency>("SELECT * FROM PatternFrequencies WHERE userId = " + DBHelper.userID).ToList();

                // Check if any pattern data exists
                if (pattern.Any())
                {
                    // Enable recommendations if pattern data exists
                    recommendationEnabled = true;

                }
                else
                {
                    // Disable recommendations if no pattern data exists
                    recommendationEnabled = false;
                }

                // If recommendations are enabled, process the pattern data
                if (recommendationEnabled)
                {
                    foreach (var item in pattern)
                    {
                        // Get all category frequencies from the database
                        item.CategoryFrequencies = context.CategoryFrequencies.Where(cf => cf.PatternFrequencyId == item.Id).ToList();

                        // Get all date frequencies from the database
                        item.DateFrequencies = context.DateFrequencies.Where(df => df.PatternFrequencyId == item.Id).ToList();

                        // Populate unique categories set
                        foreach (var categoryFrequency in item.CategoryFrequencies)
                        {
                            uniqueCategories.Add(categoryFrequency.Category);
                        }

                        // Populate unique dates set
                        foreach (var dateFrequency in item.DateFrequencies)
                        {
                            uniqueDates.Add(dateFrequency.Date);
                        }
                    }
                }
                else
                {
                    // If no pattern data exists, use search records to create unique categories and dates
                    List<SearchRecord> searchRecord = DBHelper.trackSearch;
                    if (searchRecord != null)
                    {
                        foreach (var record in searchRecord)
                        {
                            if (record.Category.HasValue)
                            {
                                uniqueCategories.Add(record.Category.Value);
                                uniqueDates.Add(record.StartDate);
                            }

                            if (record.EndDate != null)
                            {
                                uniqueDates.Add(record.EndDate.Value);
                                uniqueDates.Add(record.StartDate);
                            }
                        }
                    }
                    else
                    {
                        return;
                    }
                }
            }
        }

        //----------------------------------------------------------------------------

        // Method to recommend events based on the preferred categories
        private void GenerateCatRecommendation()
        {
            // Dictionary to store category frequencies
            Dictionary<EventCategory, int> catFrequency = new Dictionary<EventCategory, int>();

            // Count the frequency of each category based on the user's search patterns
            foreach (var category in uniqueCategories)
            {
                // Sum the frequencies of the current category from the pattern data
                int frequency = pattern
                    .SelectMany(p => p.CategoryFrequencies)
                    .Where(cf => cf.Category == category)
                    .Sum(cf => cf.Frequency);

                catFrequency[category] = frequency;
            }

            // Check if the catFrequency dictionary is empty
            if (catFrequency.Count == 0)
            {
                // Empty list to indicate no data
                categoryPreference = new List<EventCategory>();
                return;
            }

            // Determine the categories with the highest frequency
            int maxFrequency = catFrequency.Values.Max();
            categoryPreference = catFrequency
                .Where(cf => cf.Value == maxFrequency)
                .Select(cf => cf.Key)
                .ToList();
        }

        //----------------------------------------------------------------------------

        // Method to generate recommendations for user days based on the unique dates,
        // will return the weekday(s) most searched by the user
        private void GenerateDayRecommendation()
        {
            // Dictionary to store day frequencies
            Dictionary<DayOfWeek, int> dayFrequency = new Dictionary<DayOfWeek, int>();

            // Count the frequency of each day of the week based on unique dates
            foreach (var date in uniqueDates)
            {
                var dayOfWeek = date.DayOfWeek;

                if (dayFrequency.ContainsKey(dayOfWeek))
                {
                    dayFrequency[dayOfWeek]++;
                }
                else
                {
                    dayFrequency[dayOfWeek] = 1;
                }
            }

            // Check if the dayFrequency dictionary is empty
            if (dayFrequency.Count == 0)
            {
                dayPreference = new List<string> { "No data available" };
                return;
            }

            // Determine the days with the highest frequency
            int maxFrequency = dayFrequency.Values.Max();
            dayPreference = dayFrequency
                .Where(df => df.Value == maxFrequency)
                .Select(df => df.Key.ToString())
                .ToList();

            // Check if weekends are part of the most searched days
            if (dayPreference.Contains(DayOfWeek.Saturday.ToString()) ||
                dayPreference.Contains(DayOfWeek.Sunday.ToString()))
            {
                dayPreference.Add("Weekend");
            }
        }

        //----------------------------------------------------------------------------

        // Method to recommend events based on the preferred weekdays
        private List<Event> RecommendEventsForDays(List<Event> allEvents)
        {
            // Filter events based on preferred days
            var recommendedEvents = allEvents.Where(e =>
                dayPreference.Contains(e.EventDate.DayOfWeek.ToString()) ||
                dayPreference.Contains("Weekend") &&
                (e.EventDate.DayOfWeek == DayOfWeek.Saturday || e.EventDate.DayOfWeek == DayOfWeek.Sunday))
                .ToList();

            return recommendedEvents;
        }

        //----------------------------------------------------------------------------

        // Method to recommend events based on the preferred categories
        private List<Event> RecommendEventsForCat(List<Event> allEvents)
        {
            // Filter events based on preferred categories
            var recommendedEvents = allEvents
                .Where(e => categoryPreference.Contains(e.EventCat))
                .ToList();

            return recommendedEvents;
        }

        //----------------------------------------------------------------------------

        // Method to populate the eventRecommendations queue
        private void PopulateEventRecommendations()
        {
            // List to store all events
            List<Event> allEvents;

            // Use a database context to access the database
            using (var db = new AppDbContext())
            {
                // Get all events from the database
                allEvents = db.Events.ToList();
            }

            // Get recommended events for days
            var recommendedByDay = RecommendEventsForDays(allEvents);

            // Get recommended events for categories
            var recommendedByCat = RecommendEventsForCat(allEvents);

            // Combine both lists and remove duplicates
            var combinedRecommendations = recommendedByDay
                .Union(recommendedByCat) // Union removes duplicates
                .ToList();

            // Add the combined recommended events to the queue
            foreach (var evt in combinedRecommendations)
            {
                eventRecommendations.Enqueue(evt);
            }
        }

        //----------------------------------------------------------------------------

        // Method: PopulateEventRelated
        // Private method to curate related events according to the user's favored event categories
        // and populate the related events list.
        private void PopulateEventRelated()
        {
            // List to store all events
            List<Event> allEvents;

            // Use a database context to access the database
            using (var db = new AppDbContext())
            {
                // Get all events from the database
                allEvents = db.Events.ToList();
            }

            // List to store related events based on category preference
            List<Event> relatedEvents = new List<Event>();

            // Iterate through each preferred category
            foreach (EventCategory cat in categoryPreference)
            {
                switch (cat)
                {
                    case EventCategory.CulturalExhibitions:
                        // Combine related events for Cultural Exhibitions
                        relatedEvents = relatedEvents
                            .Union(allEvents.Where(e => e.EventCat == EventCategory.FoodCraftMarkets))
                            .ToList();
                        break;

                    case EventCategory.MusicFestivals:
                        // Combine related events for Music Festivals
                        relatedEvents = relatedEvents
                            .Union(allEvents.Where(e => e.EventCat == EventCategory.FoodCraftMarkets))
                            .ToList();
                        break;

                    case EventCategory.FoodCraftMarkets:
                        // Combine related events for Food Craft Markets
                        relatedEvents = relatedEvents
                            .Union(allEvents.Where(e => e.EventCat == EventCategory.MusicFestivals))
                            .ToList();
                        break;

                    case EventCategory.SportsEvents:
                        // Combine related events for Sports Events
                        relatedEvents = relatedEvents
                            .Union(allEvents.Where(e => e.EventCat == EventCategory.HealthWellnessWorkshops))
                            .ToList();
                        break;

                    case EventCategory.CharityEvents:
                        // Combine related events for Charity Events
                        relatedEvents = relatedEvents
                            .Union(allEvents.Where(e => e.EventCat == EventCategory.EducationalSeminars))
                            .ToList();
                        break;

                    case EventCategory.HealthWellnessWorkshops:
                        // Combine related events for Health & Wellness Workshops
                        relatedEvents = relatedEvents
                            .Union(allEvents.Where(e => e.EventCat == EventCategory.SportsEvents))
                            .ToList();
                        break;

                    case EventCategory.EducationalSeminars:
                        // Combine related events for Educational Seminars
                        relatedEvents = relatedEvents
                            .Union(allEvents.Where(e => e.EventCat == EventCategory.CharityEvents))
                            .ToList();
                        break;

                    case EventCategory.LocalGovernmentAnnouncements:
                        // Combine related events for Local Government Announcements
                        relatedEvents = relatedEvents
                            .Union(allEvents.Where(e => e.EventCat == EventCategory.CommunityMeetings))
                            .ToList();
                        break;

                    case EventCategory.CommunityMeetings:
                        // Combine related events for Community Meetings
                        relatedEvents = relatedEvents
                            .Union(allEvents.Where(e => e.EventCat == EventCategory.LocalGovernmentAnnouncements))
                            .ToList();
                        break;
                }
            }

            // Remove duplicates by calling Distinct on the combined relatedEvents list
            relatedEvents = relatedEvents.Distinct().ToList();

            // Add the combined recommended events to the queue
            foreach (var evt in relatedEvents)
            {
                eventRelated.Enqueue(evt);
            }
        }


        //----------------------------------------------------------------------------

        // Method to return the event recommendations queue
        public Queue<Event> ReturnRecommendations()
        {
            return eventRecommendations;
        }

        //----------------------------------------------------------------------------
        // Method to return the related even queue
        public Queue<Event> ReturnRelatedEvents()
        {
            return eventRelated;
        }
    }
}
//__---____---____---____---____---____---____---__.ooo END OF FILE ooo.__---____---____---____---____---____---____---__\\