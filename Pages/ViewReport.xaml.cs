﻿using MVA_poe.Controls;
using MVA_Poe.Classes;
using MVA_Poe.Controls;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.Entity;
using System.Globalization;
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
    {  // ObservableCollection for file attachments
        public ObservableCollection<FileDetail> AttachListItems { get; set; }

        // ObservableCollection for images
        public ObservableCollection<Image> ImageListItems { get; set; }

        // List to store reports
        private List<Report> reports = new List<Report>();

        // Constructor for the ViewReport class
        public ViewReport()
        {
            // Initialize the component (generated by XAML)
            InitializeComponent();

            // Initialize the ObservableCollection for file attachments
            AttachListItems = new ObservableCollection<FileDetail>();

            // Initialize the ObservableCollection for images
            ImageListItems = new ObservableCollection<Image>();

            // Retrieve data
            GetData();

            // Set the language based on the DBHelper.lang value
            SetLanguage(DBHelper.lang);
        }

        //----------------------------------------------------------------------------//

        // Method: SetLanguage
        // Sets the language for the application based on the culture code
        private void SetLanguage(string cultureCode)
        {
            // Set the current UI culture to the specified culture code
            CultureInfo.CurrentUICulture = new CultureInfo(cultureCode);

            // Create a new ResourceDictionary instance
            ResourceDictionary dict = new ResourceDictionary();

            // Switch based on the culture code
            switch (cultureCode)
            {
                case "af":
                    // Set the source to the Afrikaans resource file
                    dict.Source = new Uri("Resources/Strings.af.xaml", UriKind.Relative);
                    break;
                case "isx":
                    // Set the source to the Icelandic resource file
                    dict.Source = new Uri("Resources/Strings.isx.xaml", UriKind.Relative);
                    break;
                default:
                    // Set the source to the English resource file
                    dict.Source = new Uri("Resources/Strings.en.xaml", UriKind.Relative);
                    break;
            }

            // Add the resource dictionary to the merged dictionaries
            this.Resources.MergedDictionaries.Add(dict);
        }

        //----------------------------------------------------------------------------//

        // Event handler for the ReportList selection changed event
        private void ReportList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            // If a report is selected
            if (ReportList.SelectedItem != null)
            {
                // Hide the default message
                DefaultMessage.Visibility = Visibility.Collapsed;

                // Show the uploading files list
                UploadingFilesList.Visibility = Visibility.Visible;

                // Load the selected report data
                LoadReportData();
            }
            else
            {
                // Show the default message
                DefaultMessage.Visibility = Visibility.Visible;

                // Hide the uploading files list
                UploadingFilesList.Visibility = Visibility.Collapsed;
            }
        }

        //----------------------------------------------------------------------------//

        // Method: GetData
        // Retrieves data from the database
        private void GetData()
        {
            // Get the user ID from DBHelper
            int userId = DBHelper.userID;

            // List to store report names
            List<String> reportNames = new List<String>();

            // Create a new instance of AppDbContext
            using (var context = new AppDbContext())
            {
                // Retrieve the reports from the database based on the user ID
                reports = context.Reports
                    .Where(r => r.userId == userId)
                    .ToList();
            }

            // Add each report name to the reportNames list
            foreach (var report in reports)
            {
                reportNames.Add(report.reportName);
            }

            // Set the report names as the item source for the ReportList
            ReportList.ItemsSource = reportNames;
        }

        //----------------------------------------------------------------------------//

        // Method: LoadReportData
        // Loads the selected report data
        private void LoadReportData()
        {
            // Get the user ID from DBHelper
            int userId = DBHelper.userID;

            // Get the selected report index
            int reportNumber = ReportList.SelectedIndex;

            // Get the report ID of the selected report
            int searchID = this.reports[reportNumber].reportID;

            // Retrieve the reports from the database
            List<Report> reports = GetReportsFromDatabase(userId, searchID);

            // Create a new instance of ReportInfo
            ReportInfo reportInfo = new ReportInfo();

            // Clear the UploadingFilesList
            UploadingFilesList.Items.Clear();

            // Add each report to the UploadingFilesList
            foreach (var report in reports)
            {
                // Populate the report display
                reportInfo.PopulateReportDisplay(report);

                // Add the reportInfo to the UploadingFilesList
                UploadingFilesList.Items.Add(reportInfo);
            }
        }

        //----------------------------------------------------------------------------//

        // Method: GetReportsFromDatabase
        // Retrieves reports from the database
        private List<Report> GetReportsFromDatabase(int userId, int reportNumber)
        {
            // List to store reports
            List<Report> reports = new List<Report>();

            // Create a new instance of AppDbContext
            using (var context = new AppDbContext())
            {
                // Retrieve the reports from the database based on the user ID and report number
                reports = context.Reports
                    .Where(r => r.userId == userId && r.reportID == reportNumber)
                    .Include(r => r.Attachments) // Eagerly load attachments
                    .ToList();
            }

            // Handle null Attachments
            foreach (var report in reports)
            {
                // If the report has no attachments, initialize an empty list
                if (report.Attachments == null)
                {
                    report.Attachments = new List<Attachment>();
                }
            }

            // Return the list of reports
            return reports;
        }
    }
}

//__---____---____---____---____---____---____---__.ooo END OF FILE ooo.__---____---____---____---____---____---____---__\\