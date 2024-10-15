// Import necessary namespaces
using MVA_poe.Classes;
using MVA_poe.Data;
using MVA_Poe.Classes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

// Define the PatternFrequency class
public class PatternFrequency
{
    // Primary Key
    [Key]
    public int Id { get; set; }

    // Foreign key to the user
    [Required]
    public int userId { get; set; } // Assuming you have a User entity

    // Navigation property for User
    [ForeignKey("userId")]
    public virtual User User { get; set; }

    // Timestamp for the entry
    public DateTime CreatedAt { get; set; }

    // Collection to hold category frequencies
    // This property will not be mapped to the database directly
    public virtual ICollection<CategoryFrequency> CategoryFrequencies { get; set; }

    // Collection to hold date frequencies
    // This property will not be mapped to the database directly
    public virtual ICollection<DateFrequency> DateFrequencies { get; set; }

    // Constructor to initialize collections and set CreatedAt to the current time
    public PatternFrequency()
    {
        CategoryFrequencies = new List<CategoryFrequency>();
        DateFrequencies = new List<DateFrequency>();
        CreatedAt = DateTime.UtcNow; // Initialize to the current time
    }

    //----------------------------------------------------------------------------

    // Method to aggregate data from a list of user search records
    public void AggregateFromUserSearchRecords(IEnumerable<SearchRecord> records)
    {
        // Create sets to hold unique categories and dates
        var uniqueCategories = new HashSet<EventCategory>();
        var uniqueDates = new HashSet<DateTime>();

        // Iterate through each search record
        foreach (var record in records)
        {
            // Add category to the set if it has a value
            if (record.Category.HasValue)
            {
                uniqueCategories.Add(record.Category.Value);
            }
            // Add start date to the set
            uniqueDates.Add(record.StartDate);
        }

        // Frequency aggregation logic for categories
        foreach (var category in uniqueCategories)
        {
            // Check if the category already exists in the collection
            var existingCategory = CategoryFrequencies.FirstOrDefault(cf => cf.Category == category);
            if (existingCategory != null)
            {
                // Increment frequency if it exists
                existingCategory.Frequency++;
            }
            else
            {
                // Add new category frequency if it doesn't exist
                CategoryFrequencies.Add(new CategoryFrequency
                {
                    Category = category,
                    Frequency = 1
                });
            }
        }

        // Frequency aggregation logic for dates
        foreach (var date in uniqueDates)
        {
            // Check if the date already exists in the collection
            var existingDate = DateFrequencies.FirstOrDefault(df => df.Date == date);
            if (existingDate != null)
            {
                // Increment frequency if it exists
                existingDate.Frequency++;
            }
            else
            {
                // Add new date frequency if it doesn't exist
                DateFrequencies.Add(new DateFrequency
                {
                    Date = date,
                    Frequency = 1
                });
            }
        }
    }
}
//__---____---____---____---____---____---____---__.ooo END OF FILE ooo.__---____---____---____---____---____---____---__\\
//----------------------------------------------------------------------------

// Define the CategoryFrequency class
public class CategoryFrequency
{
    // Primary Key
    [Key]
    public int Id { get; set; }

    // The category type
    public EventCategory Category { get; set; }

    // Frequency count
    public int Frequency { get; set; }

    // Foreign key for the PatternFrequency entity
    public int PatternFrequencyId { get; set; }

    // Navigation property for PatternFrequency
    public virtual PatternFrequency PatternFrequency { get; set; }
}

//----------------------------------------------------------------------------

// Define the DateFrequency class
public class DateFrequency
{
    // Primary Key
    [Key]
    public int Id { get; set; }

    // The date being tracked
    public DateTime Date { get; set; }

    // Frequency count
    public int Frequency { get; set; }

    // Foreign key for the PatternFrequency entity
    public int PatternFrequencyId { get; set; }

    // Navigation property for PatternFrequency
    public virtual PatternFrequency PatternFrequency { get; set; }
}
