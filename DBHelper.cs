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
        private static List<RecordPattern> _currentSessionRecords = new List<RecordPattern>();

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

        // Add a record pattern to the current session
        public static void AddRecordPatternToSession(RecordPattern record)
        {
            _currentSessionRecords.Add(record);
        }

        // Get the current session records
        public static List<RecordPattern> GetCurrentSessionRecords()
        {
            return new List<RecordPattern>(_currentSessionRecords);
        }

        // Clear the current session
        public static void ClearSession()
        {
            _currentSessionRecords.Clear();
        }

        // Finalize the session and analyze the data
        public static void FinalizeSessionAndAnalyzeData()
        {
            var sessionData = GetCurrentSessionRecords();
            var patternFrequency = AnalyzeData(sessionData);
            SavePatternFrequency(patternFrequency);
            ClearSession();
        }

        // Analyze the session data
        private static PatternFrequency AnalyzeData(List<RecordPattern> sessionData)
        {
            var patternFrequency = new PatternFrequency
            {
                userId = userID,
                CreatedAt = DateTime.UtcNow,
                CategoryFrequencies = new List<CategoryFrequency>(),
                DateFrequencies = new List<DateFrequency>()
            };

            foreach (var record in sessionData)
            {
                foreach (var category in record.GetSearchCatHistory())
                {
                    var categoryFrequency = patternFrequency.CategoryFrequencies
                        .FirstOrDefault(cf => cf.Category == category);
                    if (categoryFrequency == null)
                    {
                        categoryFrequency = new CategoryFrequency
                        {
                            Category = category,
                            Frequency = 0
                        };
                        patternFrequency.CategoryFrequencies.Add(categoryFrequency);
                    }
                    categoryFrequency.Frequency++;
                }

                foreach (var date in record.GetSearchDateHistory())
                {
                    var dateFrequency = patternFrequency.DateFrequencies
                        .FirstOrDefault(df => df.Date.Date == date.Date);
                    if (dateFrequency == null)
                    {
                        dateFrequency = new DateFrequency
                        {
                            Date = date.Date,
                            Frequency = 0
                        };
                        patternFrequency.DateFrequencies.Add(dateFrequency);
                    }
                    dateFrequency.Frequency++;
                }
            }

            return patternFrequency;
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
