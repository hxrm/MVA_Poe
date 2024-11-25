using MVA_poe.Classes;
using MVA_poe.Classes.SearchManagment;
using MVA_poe.Data;
using MVA_Poe.Classes;
using MVA_Poe.Migrations;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Runtime.Remoting.Contexts;

namespace MVA_poe
{
    // Define a public class named DBHelper
    public class DBHelper
    {
        // Declare a public static integer variable named 'userID'
        public static int userID = 1009;

        // Declare a public static string variable named 'lang'
        public static string lang = "af";

        // Declare a public static string variable named 'email'
        public static string email = "h@gmail.com";

        // List to store current session records
     //   public static RecordPattern trackSearch = new RecordPattern();
        public static List<SearchRecord>trackSearch = new List<SearchRecord>();

        private static AVLTree<MVA_Poe.Classes.ServiceRequest> requestTree = new AVLTree<MVA_Poe.Classes.ServiceRequest>();
        // Default constructor
        public DBHelper() { }

        // Parameterized constructor
        public DBHelper(int inputUser, string inputLang, string inputEmail)
        {
            // Assign the inputUser parameter to the static userID variable
            userID = inputUser;

            // Assign the inputLang parameter to the static lang variable
            lang = inputLang;

            // Assign the inputEmail parameter to the static email variable
            email = inputEmail;
        }

      

        // Clear the current session
        public static void ClearSession()
        {
            trackSearch = new List<SearchRecord>();
        }
        public static void NewRequest(Report report)
        {
            using (var context = new AppDbContext())
            {
                // Pull the report by its ID to ensure it is not added again
                var existingReport = context.Reports.Find(report.reportID);
                if (existingReport == null)
                {
                    throw new InvalidOperationException("Report not found in the database.");
                }

                var serviceRequest = new MVA_Poe.Classes.ServiceRequest();
                serviceRequest.SetData(existingReport);
                context.ServiceRequests.Add(serviceRequest);
                requestTree.Insert(serviceRequest, serviceRequest.requestId, serviceRequest);
                context.SaveChanges();
            }
        }

        // Finalize the session and analyze the data
        public static void FinalizeSessionAndAnalyzeData()
        {
            PatternFrequency patternFrequency = new PatternFrequency
            {
                userId = userID,
                CreatedAt = DateTime.UtcNow,
            };

            // Here, trackSearch is a list of UserSearchRecord
            patternFrequency.AggregateFromUserSearchRecords(DBHelper.trackSearch);
            SaveSearchRecord();
            SavePatternFrequency(patternFrequency);
            ClearSession();
        }
        private static void SaveSearchRecord()
        {
           using (var context = new AppDbContext())
            {
                foreach (var searchRecord in trackSearch)
                {
                    context.SearchRecords.Add(searchRecord);
                }
                context.SaveChanges();
            }
        }
        // Save the pattern frequency to the database
        private static void SavePatternFrequency(PatternFrequency patternFrequency)
        {
            using (var context = new AppDbContext())
            {
                // Add the PatternFrequency entity
                context.PatternFrequencies.Add(patternFrequency);

                // Add each CategoryFrequency entity
                foreach (var categoryFrequency in patternFrequency.CategoryFrequencies)
                {
                    context.CategoryFrequencies.Add(categoryFrequency);
                }

                // Add each DateFrequency entity
                foreach (var dateFrequency in patternFrequency.DateFrequencies)
                {
                    context.DateFrequencies.Add(dateFrequency);
                }

                // Save all changes to the database
                context.SaveChanges();
            }
        }
    }
}

