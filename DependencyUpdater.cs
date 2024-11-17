using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.Entity;

namespace MVA_Poe.Classes
{
    public class DependencyUpdater
    {
        private readonly AppDbContext _context;

        public DependencyUpdater(AppDbContext context)
        {
            _context = context;
        }

        public void AddDependenciesToExistingData()
        {
            // Fetch all ServiceRequests and include their related Report
            var serviceRequests = _context.ServiceRequests.Include(sr => sr.report).ToList();

            // Group ServiceRequests by ReportCategory
            var groupedByCategory = serviceRequests.GroupBy(sr => sr.report.reportCat);

            foreach (var group in groupedByCategory)
            {
                // Sort each group by ServiceRequestPriority
                var sortedRequests = group.OrderBy(sr => sr.requestPri).ToList();

                // Add dependencies based on priority within each category
                for (int i = 0; i < sortedRequests.Count - 1; i++)
                {
                    var currentRequest = sortedRequests[i];
                    var nextRequest = sortedRequests[i + 1];

                    currentRequest.Dependencies.Add(nextRequest);
                    nextRequest.DependentOn.Add(currentRequest);
                }
            }

            // Save changes to the database
            _context.SaveChanges();
        }
    }
}
