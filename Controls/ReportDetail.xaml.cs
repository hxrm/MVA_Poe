using MVA_Poe.Classes;
using MVA_Poe.Data;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Mail;
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

namespace MVA_poe.Controls
{
    /// <summary>
    /// Interaction logic for ReportInfo.xaml
    /// </summary>
    public partial class ReportInfo : UserControl
    {
        Report report;
        public ReportInfo()
        {
            InitializeComponent();
        }

        public void PopulateReportDisplay(Report report)
        {
            this.report = report;
            LocationTextBox.Text = report.reportLoc;
            DateTextBox.Text = report.reportDate.ToString("MM/dd/yyyy");
            CategoryTextBox.Text = GetCategory();
            DescriptionTextBox.Text = report.reportDesc;
            
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

                    // Add the image to the ImageAttachmentList user control
                    var imageControl = new ImagesView();
                    imageControl.AddImage(image);

                    MediaAttachmentsListBox.Items.Add(imageControl);
                  
                }
                else
                { 
                    GetFileDisplay(attachment);
                }
            }
        }
        private void GetFileDisplay(MVA_Poe.Classes.Attachment attachment)
        {
            BitmapImage image = null;

            if (attachment.FileName.EndsWith(".docx"))
            {
                image = new BitmapImage(new Uri("/Resources/DOC.png", UriKind.Relative));
            }
            else if (attachment.FileName.EndsWith(".pdf"))
            {
                image = new BitmapImage(new Uri("/Resources/PDF.png", UriKind.Relative));
            }
            else if (attachment.FileName.EndsWith(".txt"))
            {
                image = new BitmapImage(new Uri("/Resources/TXT.png", UriKind.Relative));
            }
            else
            {
                image = new BitmapImage(new Uri("/Resources/RAW.png", UriKind.Relative));
            }

            if (image != null)
            {
                // Add the image to the ImageAttachmentList user control
                var imageControl = new ImagesView();
                imageControl.AddImage(image);

                MediaAttachmentsListBox.Items.Add(imageControl);
            }
        }

        private string GetCategory()
        {
            ReportCategory category = (ReportCategory)report.reportCat;
            string catItem = System.Text.RegularExpressions.Regex.Replace(category.ToString(), "(\\B[A-Z])", " $1");
            return catItem;
        }
    }
}
