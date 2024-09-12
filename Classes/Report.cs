using MVA_Poe.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MVA_Poe.Classes
{
    public class Report
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int reportID { get; set; }
        public string reportName { get; set; }
        public string reportDesc { get; set; }
        public DateTime reportDate { get; set; }
        public string reportLoc { get; set; }
        public ReportCategory reportCat { get; set; }

        public ICollection<Attachment> Attachments { get; set; } = null;

        // Foreign Key for User
        public int userId { get; set; }
        public User reportedBy { get; set; }

        public Report()
        {
        }
        // public document reportDoc { get; set; }
        //public img reportImg { get; set; }
    }
}
