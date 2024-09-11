using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Runtime.Remoting.Contexts;
using System.Text;
using System.Threading.Tasks;

namespace MVA_Poe.Classes
{
    // Define a class named AppDbContext that inherits from DbContext, which is part of Entity Framework.
    public class AppDbContext : DbContext
    {
        // Define a DbSet property named Reports, which represents a collection of Report entities in the database.

        // Define a DbSet property named Reports, which represents a collection of Report entities in the database.

        public DbSet<User> Users { get; set; }
        public DbSet<Report> Reports { get; set; }
        public DbSet<Attachment> Attachments { get; set; }

        // Commented out code that shows alternative ways to define the database path and connection string.     
        //Actual path to the database file
        static string relativePath = "|DataDirectory|\\MVA_Database.mdf";
        // static string dbFilePath = @"C:\Users\User\source\DB\muniDB.mdf";

        // Define the connection string using the database file path.
        static string theDB = $"Data Source=(LocalDB)\\MSSQLLocalDB;AttachDbFilename={relativePath};Integrated Security=True;Connect Timeout=30";
       //static string theDB = $"Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=MVC_Database;Integrated Security=True;Connect Timeout=30;Encrypt=False;";

        // Constructor for the AppDbContext class. It calls the base class constructor with a connection string.
        public AppDbContext() : base(theDB)
        {


        }
    }
}
