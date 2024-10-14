using MVA_poe.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MVA_poe.Classes
{
    public class RecordPattern
    {
        private HashSet<EventCategory> uniqueCategories;
        private HashSet<DateTime> uniqueDates;

        public RecordPattern()
        {
            uniqueCategories = new HashSet<EventCategory>();
            uniqueDates = new HashSet<DateTime>();
        }

        // Record category search
        public void RecordSearchCatergory(EventCategory searchCat)
        {
            uniqueCategories.Add(searchCat);

        }
        // Record date search
        public void RecordSearchDateRange(DateTime searchDateRange)
        {
            uniqueDates.Add(searchDateRange);
        }
        // Retrieve unique dates
        public HashSet<DateTime> GetSearchDateHistory()
        {
            return uniqueDates;
        }
        // Retrieve unique categories
        public HashSet<EventCategory> GetSearchCatHistory()
        {
            return uniqueCategories;
        }

       
    }

   
}