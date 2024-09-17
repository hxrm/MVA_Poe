using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
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

        static string dbFilePath = @"C:\Users\User\source\DB\MVA_poe\bin\Debug\MVC_Database.mdf";

        // Define the connection string using the database file path
        static string theDB = $"Data Source=(LocalDB)\\MSSQLLocalDB;AttachDbFilename={dbFilePath};Integrated Security=True;Connect Timeout=30";

        /// </summary>

        // Constructor for the AppDbContext class. It calls the base class constructor with a connection string.
        public AppDbContext() : base(theDB)
        {


        }     

       
        
    }
}
