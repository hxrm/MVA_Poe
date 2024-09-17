using MVA_poe.Controls;
using MVA_Poe.Classes;
using MVA_Poe.Controls;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace MVA_poe.Pages
{
    /// <summary>
    /// Interaction logic for ViewReport.xaml
    /// </summary>
    public partial class ViewReport : Page
    {
        public ObservableCollection<FileDetail> AttachListItems { get; set; }
        public ObservableCollection<Image> ImageListItems { get; set; }
        private List<Report> reports =  new List<Report>();
        
        public ViewReport()
        {

                     
            InitializeComponent();
            AttachListItems = new ObservableCollection<FileDetail>(); 
            ImageListItems = new ObservableCollection<Image>();
            GetData();
           
        }

        private void ReportList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            LoadReportData();
            
        }
        private void GetData()
        {
            int userId = DBHelper.userID;
            List<String> reportNames = new List<String>();
            
            using (var context = new AppDbContext())
            {
                reports = context.Reports
                    .Where(r => r.userId == userId)//
                    .ToList();
            }
            foreach (var report in reports)
            {
                reportNames.Add(report.reportName);
            }
            ReportList.ItemsSource = reportNames;
        }
        private void LoadReportData()
        {
            int userId = DBHelper.userID; 
            int reportNumber = ReportList.SelectedIndex;
            int searchID = this.reports[reportNumber].reportID;
          
            List<Report> reports = GetReportsFromDatabase(userId, searchID);
            ReportInfo reportInfo = new ReportInfo();

            // Add each report to the UploadingFilesList
            UploadingFilesList.Items.Clear();
            foreach (var report in reports)
            {
               reportInfo.PopulateReportDisplay(report);
               UploadingFilesList.Items.Add(reportInfo);
              
            }

        }       

        // Example method to retrieve reports from the database
        private List<Report> GetReportsFromDatabase(int userId, int reportNumber)
        {
            // Replace with actual database retrieval logic
            // This is just a placeholder for demonstration purposes
            List<Report> reports = new List<Report>();
            using (var context = new AppDbContext())
            {
                reports = context.Reports
                    .Where(r => r.userId == userId && r.reportID == reportNumber)
                    .Include(r => r.Attachments) // Eagerly load attachments
                    .ToList();
            }

            // Handle null Attachments
            foreach (var report in reports)
            {
                if (report.Attachments == null)
                {
                    report.Attachments = new List<Attachment>();
                }
            }

            return reports;
        }      
     
    }
}
