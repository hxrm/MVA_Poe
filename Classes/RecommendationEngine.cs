using MVA_poe.Data;
using MVA_Poe.Classes;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Globalization;
using System.Linq;

namespace MVA_poe.Classes
{
    public class RecommendationEngine
    {
        private HashSet<EventCategory> uniqueCategories; // Set for unique categories
        private HashSet<DateTime> uniqueDates; // Set for unique dates
        private Queue<Event> eventRecommendations;

      //  private PatternFrequency pattern;
        public bool recommendationEnabled { get; set; }

        List<string> dayPreference = new List<string>();
        List<EventCategory> categoryPreference = new List<EventCategory>();
        List<PatternFrequency> pattern = new List<PatternFrequency>();

        public RecommendationEngine()
        {
            uniqueCategories = new HashSet<EventCategory>();
            uniqueDates = new HashSet<DateTime>();
            eventRecommendations = new Queue<Event>();
            GetPatternData();
            GenerateDayRecommendation();
            GenerateCatRecommendation();
          //  eventRecommendations = new PriorityQueue<Event, int>();
        }

        // Method to get user Pattern data from database if exists
        private void GetPatternData()
        {
            using (var context = new AppDbContext())
            {

                pattern = context.Database.SqlQuery<PatternFrequency>("SELECT * FROM PatternFrequencies WHERE userId = " + DBHelper.userID).ToList();
                if (pattern.Any())
                {
                    recommendationEnabled = false;
                }
                else
                {
                    recommendationEnabled = false;
                }
                if (recommendationEnabled)
                { 
                    foreach (var item in pattern)
                    {
                        // Get all category frequencies in db
                        item.CategoryFrequencies = context.CategoryFrequencies.Where(cf => cf.PatternFrequencyId == item.Id).ToList();
                        // Get all date frequencies in db
                        item.DateFrequencies = context.DateFrequencies.Where(df => df.PatternFrequencyId == item.Id).ToList();

                        // Populate uniqueCategories set
                        foreach (var categoryFrequency in item.CategoryFrequencies)
                        {
                            uniqueCategories.Add(categoryFrequency.Category);
                        }

                        // Populate uniqueDates set
                        foreach (var dateFrequency in item.DateFrequencies)
                        {
                            uniqueDates.Add(dateFrequency.Date);
                        }
                    }
                }               
                else
                {
                   
                    List<SearchRecord> searchRecord = DBHelper.trackSearch;
                    if (searchRecord != null)
                    {//if the search record has data then use it to create the unique categories and dates
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
        //CAT FINDER 
        // Method to recommend events based on the preferred categories
        private void GenerateCatRecommendation()
        {
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
                categoryPreference = new List<EventCategory>(); // Empty list to indicate no data
                return;
            }

            // Determine the categories with the highest frequency
            int maxFrequency = catFrequency.Values.Max();
            categoryPreference = catFrequency
                .Where(cf => cf.Value == maxFrequency)
                .Select(cf => cf.Key)
                .ToList();
        }
        //DAY FINDER 
        // Method to generate recommendations for user days based on the unique dates,
        // will return the weekday(s) most searched by the user
        private void GenerateDayRecommendation()
        {
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
        // Method to recommend events based on the preferred weekdays
        public List<Event> RecommendEventsForDays(List<Event> allEvents)
        {
            // Filter events based on preferred days
            var recommendedEvents = allEvents.Where(e =>
                dayPreference.Contains(e.EventDate.DayOfWeek.ToString()) ||
                dayPreference.Contains("Weekend") &&
                (e.EventDate.DayOfWeek == DayOfWeek.Saturday || e.EventDate.DayOfWeek == DayOfWeek.Sunday))
                .ToList();

            return recommendedEvents;
        }
        // Method to recommend events based on the preferred cat
        public List<Event> RecommendEventsForCat(List<Event> allEvents)
        {
            // Filter events based on preferred categories
            var recommendedEvents = allEvents
                .Where(e => categoryPreference.Contains(e.EventCat))
                .ToList();

            return recommendedEvents;
        }

        // Populate the eventRecommendations queue
        public void PopulateEventRecommendations(List<Event> allEvents)
        {
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
    }
}
