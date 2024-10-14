using MVA_poe.Data;
using MVA_Poe.Classes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

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
