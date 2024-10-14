using MVA_poe.Classes;
using MVA_Poe.Classes;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MVA_poe
{
    // Define a public class named DBHelper
    public class DBHelper
    {
        // Declare a public static integer variable named 'userID'
        public static int userID;

        // Declare a public static string variable named 'lang'
        public static string lang;

        // Declare a public static string variable named 'email'
        public static string email;

        // List to store current session records
     //   public static RecordPattern trackSearch = new RecordPattern();
        public static List<RecordPattern>trackSearch = new List<RecordPattern>();

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
            trackSearch = new List<RecordPattern>();
        }
        
        //SHOULD I BE ADDING A LIST OF RECORD PATTERNS TO THE PATTERN FREQUENCY CLASS? AS I THINK IT ALWASY RETURNS 1 FOR FREQUENCY ???
        // THEN SET UP TIMER TRIGGER TO SAVE THE RECORDS TO THE DATABASE
        // Finalize the session and analyze the data
        public static void FinalizeSessionAndAnalyzeData()  
        {
            PatternFrequency patternFrequency = new PatternFrequency
            {
                userId = userID,
                CreatedAt = DateTime.UtcNow,
                CategoryFrequencies = new List<CategoryFrequency>(),
                DateFrequencies = new List<DateFrequency>()
            };

            foreach(var record in trackSearch)
            {               
                patternFrequency.AggregateFromRecordPattern(record);
            }
           // patternFrequency.AggregateFromRecordPattern(trackSearch);
            SavePatternFrequency(patternFrequency);
            ClearSession();
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
