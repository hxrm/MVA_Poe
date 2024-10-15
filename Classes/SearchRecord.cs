using MVA_poe.Data;
using MVA_Poe.Classes;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MVA_poe.Classes
{
    public class SearchRecord
    {
        [Key]
        public int Id { get; set; } // Primary key

        [Required]
        public int userId { get; set; } // Assuming you have a User entity

        public EventCategory? Category { get; set; } // Nullable in case no category is selected
        public DateTime SearchTimestamp { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime? EndDate { get; set; }

        // Constructor for when both category and date range are known

        public SearchRecord(int Id)
        {
            userId = Id;
           
        }
        public SearchRecord(int Id, EventCategory category, DateTime eventDate)
        {
            userId = Id;
            Category = category;
            StartDate = eventDate;
            EndDate = null;
            SearchTimestamp = DateTime.Now;
        }

        // Constructor for when only date range is known
        public SearchRecord(int Id, DateTime startDate, DateTime endDate)
        {
            userId = Id;
            StartDate = startDate;
            EndDate = endDate;
            SearchTimestamp = DateTime.Now;
            Category = null; // No category specified
        }

    }
}
