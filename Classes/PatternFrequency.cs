using MVA_poe.Classes;
using MVA_poe.Data;
using MVA_Poe.Classes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

public class PatternFrequency
{
    [Key]
    public int Id { get; set; } // Primary Key

    // Foreign key to the user
    [Required]
    public int userId { get; set; } // Assuming you have a User entity

    // Navigation property for User
    [ForeignKey("userId")]
    public virtual User User { get; set; }

    public DateTime CreatedAt { get; set; } // Timestamp for the entry

    // Dictionary to hold category frequencies
    // This property will not be mapped to the database directly
    public virtual ICollection<CategoryFrequency> CategoryFrequencies { get; set; }

    // Dictionary to hold date frequencies
    // This property will not be mapped to the database directly
    public virtual ICollection<DateFrequency> DateFrequencies { get; set; }

    public PatternFrequency()
    {
        CategoryFrequencies = new List<CategoryFrequency>();
        DateFrequencies = new List<DateFrequency>();
        CreatedAt = DateTime.UtcNow; // Initialize to the current time
    }
    // Aggregates from a list of user search records
    public void AggregateFromUserSearchRecords(IEnumerable<SearchRecord> records)
    {
        var uniqueCategories = new HashSet<EventCategory>();
        var uniqueDates = new HashSet<DateTime>();

        foreach (var record in records)
        {
            if (record.Category.HasValue)
            {
                uniqueCategories.Add(record.Category.Value);
            }
            uniqueDates.Add(record.StartDate);
        }

        // Frequency aggregation logic
        foreach (var category in uniqueCategories)
        {
            var existingCategory = CategoryFrequencies.FirstOrDefault(cf => cf.Category == category);
            if (existingCategory != null)
            {
                existingCategory.Frequency++;
            }
            else
            {
                CategoryFrequencies.Add(new CategoryFrequency
                {
                    Category = category,
                    Frequency = 1
                });
            }
        }

        foreach (var date in uniqueDates)
        {
            var existingDate = DateFrequencies.FirstOrDefault(df => df.Date == date);
            if (existingDate != null)
            {
                existingDate.Frequency++;
            }
            else
            {
                DateFrequencies.Add(new DateFrequency
                {
                    Date = date,
                    Frequency = 1
                });
            }
        }
    }
}

    public class CategoryFrequency
{
    [Key]
    public int Id { get; set; } // Primary Key
    public EventCategory Category { get; set; } // The category type
    public int Frequency { get; set; } // Frequency count
    public int PatternFrequencyId { get; set; } // Foreign key for the PatternFrequency entity
    public virtual PatternFrequency PatternFrequency { get; set; }
}

public class DateFrequency
{
    [Key]
    public int Id { get; set; } // Primary Key
    public DateTime Date { get; set; } // The date being tracked
    public int Frequency { get; set; } // Frequency count
    public int PatternFrequencyId { get; set; } // Foreign key for the PatternFrequency entity
    public virtual PatternFrequency PatternFrequency { get; set; }
}
