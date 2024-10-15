using MVA_poe.Classes;
using MVA_poe.Data;
using MVA_Poe.Classes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace MVA_poe.Pages
{
    /// <summary>
    /// Interaction logic for Rec.xaml
    /// </summary>
    public partial class Rec : Page
    {
        private RecommendationEngine recommendationEngine;
        // List to store Events
        private List<Event> events = new List<Event>();

        public Rec()
        {
            InitializeComponent();
            LoadRecommendedEvents();
            GetData();
        }

        private void LoadRecommendedEvents()
        {
            recommendationEngine = new RecommendationEngine();
            GetData();

            recommendationEngine.PopulateEventRecommendations(events);

            var recommendedEvents = recommendationEngine.RecommendEventsForDays(events)
                .Union(recommendationEngine.RecommendEventsForCat(events))
                .ToList();

            RecommendedEventsListView.ItemsSource = recommendedEvents;

            Console.WriteLine($"Total recommended events displayed: {recommendedEvents.Count}");
        }


        private void GetData()
        {
            // populate the dictionay with the events in database 
            using (var db = new AppDbContext())
            {
                events = db.Events.ToList();
               
            }
           
        }
    }
}
