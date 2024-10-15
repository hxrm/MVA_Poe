// Import necessary namespaces
using MVA_poe.Data;
using MVA_Poe.Classes;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MVA_poe.Classes
{
    // Define the SearchRecord class
    public class SearchRecord
    {
        // Primary key for the SearchRecord entity
        [Key]
        public int Id { get; set; }

        // Foreign key to the user
        [Required]
        public int userId { get; set; } // Assuming you have a User entity

        // Nullable category in case no category is selected
        public EventCategory? Category { get; set; }

        // Timestamp for when the search was made
        public DateTime SearchTimestamp { get; set; }

        // Start date of the search range
        public DateTime StartDate { get; set; }

        // End date of the search range, nullable in case only a start date is provided
        public DateTime? EndDate { get; set; }

        //----------------------------------------------------------------------------

        // Constructor for when only the user ID is known
        public SearchRecord(int Id)
        {
            // Set the userId to the provided Id
            userId = Id;
        }

        //----------------------------------------------------------------------------

        // Constructor for when both category and date range are known
        public SearchRecord(int Id, EventCategory category, DateTime eventDate)
        {
            // Set the userId to the provided Id
            userId = Id;

            // Set the category to the provided category
            Category = category;

            // Set the start date to the provided event date
            StartDate = eventDate;

            // Set the end date to null as it's not provided
            EndDate = null;

            // Set the search timestamp to the current date and time
            SearchTimestamp = DateTime.Now;
        }

        //----------------------------------------------------------------------------

        // Constructor for when only date range is known
        public SearchRecord(int Id, DateTime startDate, DateTime endDate)
        {
            // Set the userId to the provided Id
            userId = Id;

            // Set the start date to the provided start date
            StartDate = startDate;

            // Set the end date to the provided end date
            EndDate = endDate;

            // Set the search timestamp to the current date and time
            SearchTimestamp = DateTime.Now;

            // Set the category to null as it's not provided
            Category = null; // No category specified
        }
    }
}
//__---____---____---____---____---____---____---__.ooo END OF FILE ooo.__---____---____---____---____---____---____---__\\