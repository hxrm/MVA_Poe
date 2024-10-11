using MVA_poe.Classes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Runtime.Remoting.Contexts;
using System.Text;
using System.Threading.Tasks;

namespace MVA_Poe.Classes
{
    // Define a class named AppDbContext that inherits from DbContext, which is part of Entity Framework.
    public class AppDbContext : DbContext
    {
        // Define a DbSet property named Users, which represents a collection of User entities in the database.
        public DbSet<User> Users { get; set; }

        // Define a DbSet property named Reports, which represents a collection of Report entities in the database.
        public DbSet<Report> Reports { get; set; }
        // Define a DbSet property named Events, which represents a collection of Report entities in the database.
        public DbSet<Event> Events { get; set; }

        // Define a DbSet property named Attachments, which represents a collection of Attachment entities in the database.
        public DbSet<Attachment> Attachments { get; set; }

        // Static field to store the database file path
        static string dbFilePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"MVC_Database.mdf");

        // Define the connection string using the database file path
        static string theDB = $"Data Source=(LocalDB)\\MSSQLLocalDB;AttachDbFilename={dbFilePath};Integrated Security=True;Connect Timeout=30";

        // Constructor for the AppDbContext class. It calls the base class constructor with a connection string.
        public AppDbContext() : base(theDB)
        {
        }
    }
}

//__---____---____---____---____---____---____---__.ooo END OF FILE ooo.__---____---____---____-
