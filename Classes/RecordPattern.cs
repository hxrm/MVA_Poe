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
        private List<EventCategory> searchCategories;
        private List<DateTime> searchDates;

        public RecordPattern()
        {
            searchCategories = new List<EventCategory>();
            searchDates = new List<DateTime>();
        }

        public void RecordSearchCategory(EventCategory searchCat)
        {
            searchCategories.Add(searchCat);
        }

        public void RecordSearchDateRange(DateTime searchDateRange)
        {
            searchDates.Add(searchDateRange);
        }

        public List<DateTime> GetSearchDateHistory()
        {
            return searchDates;
        }

        public List<EventCategory> GetSearchCatHistory()
        {
            return searchCategories;
        }
    }
}