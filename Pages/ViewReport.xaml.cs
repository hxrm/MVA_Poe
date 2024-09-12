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
        public ViewReport()
        {
            InitializeComponent();
            AttachListItems = new ObservableCollection<FileDetail>(); 
            ImageListItems = new ObservableCollection<Image>();
            LoadReport();
            pList.ItemsSource = AttachListItems;
            ImageList.ItemsSource = ImageListItems;
        }

        public void LoadReport()
        {
            // Example user ID and report number
            int userId = 1; // Replace with actual user ID
            int reportNumber = 1002; // Replace with actual report number

            // Retrieve report data from the database
            List<Report> reports = GetReportsFromDatabase(userId, reportNumber);

            // Add each report to the UploadingFilesList
            foreach (var report in reports)
            {
                UploadingFilesList.Items.Add(report);

              
                 foreach (var attachment in report.Attachments)
                 {
                    if (attachment.FileName.EndsWith(".png") || attachment.FileName.EndsWith(".jpeg") || attachment.FileName.EndsWith(".jpg"))
                    {
                        // Convert byte array to BitmapImage
                        BitmapImage image = new BitmapImage();
                        using (MemoryStream ms = new MemoryStream(attachment.FileContent))
                        {
                            image.BeginInit();
                            image.StreamSource = ms;
                            image.CacheOption = BitmapCacheOption.OnLoad;
                            image.EndInit();
                        }
                        // Create Image control and set its source
                        Image imagePreview = new Image();
                        imagePreview.Source = image;
                        // Add the Image control to the collection
                        ImageListItems.Add(imagePreview);

                    }
                    else 
                    {
                        AttachListItems.Add(new FileDetail()
                        {
                            FileName = attachment.FileName,
                            FileSize = string.Format("{0} {1}", (attachment.FileSize).ToString("0.0"), "Mb"),
                            UploadProgress = 100
                        });
                        
                    }
                   //Save to File in app
                }

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
