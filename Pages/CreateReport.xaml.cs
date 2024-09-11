using Microsoft.Win32;
using MVA_poe;
using MVA_Poe.Classes;
using MVA_Poe.Data;
using System;
using System.Collections.Generic;
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
using WPFModernVerticalMenu.Controls;

namespace WPFModernVerticalMenu.Pages
{
    /// <summary>
    /// Interaction logic for CreateReport.xaml
    /// </summary>
    public partial class CreateReport : Page
    {
        AppDbContext context;// = new AppDbContext();
        bool validReport, validAttachment;
        List<Attachment> attachments = new List<Attachment>();
        Report report ;

        public CreateReport()
        {
            InitializeComponent();

            SetLanguage("en"); // Set default language


            BitmapImage image = new BitmapImage();
            image.BeginInit();
            image.UriSource = new Uri("C:\\Users\\User\\Downloads\\Android Icons  (8).png");
            image.EndInit();
            //ImagePrevire.Source = image;
            context = new AppDbContext();
        }
        private void btnSubmit_Click(object sender, RoutedEventArgs e)
        {
            string title;
            ReportCategory category;
            string description;
            string location;


            title = txtTitle.Text;
            category = ReportCategory.StreetLight;
            description = txtDescrip.Text;
            location = txtLocation.Text;
           
            //create report object 
            report = AddReport(title, category, description, location);

            SaveToDB();
          
          
        }
        public void SaveToDB()
        {
           
            if (validReport && validAttachment)
            {
                // Save the report to the database
                context.Reports.Add(report);
                context.SaveChanges();

                // Save the attachments to the database
                foreach (var attachment in attachments)
                {
                    attachment.Id = attachment.Id;
                    context.Attachments.Add(attachment);
                }
                context.SaveChanges();

                MessageBox.Show("Report submitted successfully!");
            }
            else
            {
                MessageBox.Show("Please fill in all the required fields.");
            }
        }
        private void SetLanguage(string cultureCode)
        {
            CultureInfo.CurrentUICulture = new CultureInfo(cultureCode);
            ResourceDictionary dict = new ResourceDictionary();
            switch (cultureCode)
            {
                case "af":
                    dict.Source = new Uri("Resources/Strings.af.xaml", UriKind.Relative);
                    break;
                default:
                    dict.Source = new Uri("Resources/Strings.en.xaml", UriKind.Relative);
                    break;
            }
            this.Resources.MergedDictionaries.Add(dict);
        }


        private Report AddReport(string t, ReportCategory c, string d, string l)
        {
            var newReport = new Report
            {
                userId = DBHelper.userID,
                reportName = t,
                reportCat = c,
                reportDesc = d,
                reportDate = DateTime.Now,
                reportLoc = l
            };

            validReport = true;
           //context.Reports.Add(newReport);
           // context.SaveChanges();
           return newReport;
        }

        private void LoadReports()
        {
            using (var context = new AppDbContext())
            {
                //var reports = context.Reports.ToList();
                // Bind reports to a UI element, e.g., a DataGrid
                // ReportsDataGrid.ItemsSource = reports;
            }
        }

        private void btnLoad_Click(object sender, RoutedEventArgs e)
        {
            LoadReports();
            LoadAndDisplayFiles();

        }

        private void btnUpload_Click(object sender, RoutedEventArgs e)
        {

            OpenFileDialog openFile = new OpenFileDialog
            {
                Multiselect = true,// Allow multiple file selection
                Filter = "Image and Document files (*.png;*.jpeg;*.jpg;*.pdf;*.docx;*.txt)|*.png;*.jpeg;*.jpg;*.pdf;*.docx;*.txt|All files (*.*)|*.*",

            };
            //will return nullable boolean 
            //if it open the dialog for the user 
            bool? reposnse = openFile.ShowDialog();
            //flie fliter : https://www.youtube.com/watch?v=Ks9bzPSx7Vs
            if (reposnse == true)
            {
                //string file = openFile.FileName;
                //  MessageBox.Show(file);
                string[] files = openFile.FileNames;

                for (int i = 0; i < files.Length; i++)
                {
                    MessageBox.Show(files[i]);
                    string filename = System.IO.Path.GetFileName(files[i]);
                    FileInfo fileInfo = new FileInfo(files[i]);
                    byte[] fileContent = File.ReadAllBytes(files[i]);

                    //Upload to list and make file object 
                    UploadingFilesList.Items.Add(new FileDetail()
                    {
                        FileName = filename,
                        //Convert bytes to Mb => bytes / 1.049e+6
                        FileSize = string.Format("{0} {1}", (fileInfo.Length / 1.049e+6).ToString("0.0"), "Mb"),
                        UploadProgress = 100

                    });
                    // Display image if it's an image file
                    if (filename.EndsWith(".png") || filename.EndsWith(".jpeg") || filename.EndsWith(".jpg"))
                    {
                        BitmapImage image = new BitmapImage(new Uri(files[i]));
                        //  ImagePrevire.Source = image;
                    }
                    //Save to File in app 

                    string destination = @"C:\Users\User\source\repos\ReportPOE\ReportPOE\SaveFile\" + System.IO.Path.GetFileName(files[i]);

                    //fileInfo.CopyTo(destination);
                    //Upload to database and make file object
                    var fileDetail = new Attachment
                    {
                        FileName = filename,
                        FileSize = fileInfo.Length / 1.049e+6, // Convert bytes to MB
                        FileContent = fileContent
                    };
                    attachments.Add(fileDetail);

                    validAttachment = true;
                    MessageBox.Show("Files uploaded successfully DEAD ASS!");

                }

                //try
                //{

                //    BitmapImage image = new BitmapImage(new Uri(openFile.FileName));
                //    ImagePrevire.Source = image;
                //    byte[] imageData = File.ReadAllBytes(openFile.FileName);

                //}
                //catch (Exception ex)
                //{

                //    MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);

                //}





            }
        }
        private void LoadAndDisplayFiles()
        {
            var uploadedFiles = context.Attachments.ToList();

            foreach (var file in uploadedFiles)
            {
                UploadingFilesList.Items.Add(new FileDetail()
                {
                    FileName = file.FileName,
                    FileSize = string.Format("{0} {1}", (file.FileSize).ToString("0.0"), "Mb"),
                    UploadProgress = 100
                });

                // Display image if it's an image file
                if (file.FileName.EndsWith(".png") || file.FileName.EndsWith(".jpeg") || file.FileName.EndsWith(".jpg"))
                {
                    BitmapImage image = new BitmapImage();
                    using (MemoryStream ms = new MemoryStream(file.FileContent))
                    {
                        image.BeginInit();
                        image.StreamSource = ms;
                        image.CacheOption = BitmapCacheOption.OnLoad;
                        image.EndInit();
                    }
                    //ImagePrevire.Source = image;
                }
                // Display text if it's a TXT file
                else if (file.FileName.EndsWith(".txt"))
                {
                    string textContent = System.Text.Encoding.UTF8.GetString(file.FileContent);
                    // TextBoxPreview.Text = textContent;
                }
                // Placeholder for PDF preview
                else if (file.FileName.EndsWith(".pdf"))
                {
                    using (MemoryStream ms = new MemoryStream(file.FileContent))
                    {
                       /* var pdfDocument = PdfiumViewer.PdfDocument.Load(ms);
                        var pdfViewer = new PdfiumViewer.PdfViewer();
                        pdfViewer.Document = pdfDocument;
                        //PdfPreviewHost.Child = pdfViewer;*/
                    }
                }
                // Placeholder for DOCX preview
                /*  else if (file.FileName.EndsWith(".docx"))
                  {
                      using (MemoryStream ms = new MemoryStream(file.FileContent))
                      {
                          try
                          {
                              using (WordprocessingDocument wordDoc = WordprocessingDocument.Open(ms, false))
                              {
                                  var body = wordDoc.MainDocumentPart.Document.Body;
                                  RichTextBoxPreview.Document.Blocks.Clear();
                                  RichTextBoxPreview.Document.Blocks.Add(new Paragraph(new Run(body.InnerText)));
                              }
                          }
                          catch (System.IO.FileFormatException ex)
                          {
                              // Log the exception details
                              Console.WriteLine($"Error opening .docx file: {ex.Message}");
                              // Optionally, display an error message to the user
                              MessageBox.Show("The document is corrupted and cannot be opened.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                          }
                      }
                  }*/
            }
        }
    }
}
