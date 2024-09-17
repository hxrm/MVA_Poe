using MVA_Poe.Classes;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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

namespace MVA_poe.Controls
{
    /// <summary>
    /// Interaction logic for ImagesView.xaml
    /// </summary>
    public partial class ImagesView : UserControl
    {
        public ObservableCollection<BitmapImage> Images { get; set; }
        public ObservableCollection<Attachment> Attachments { get; set; }

        public ImagesView()
        {
            InitializeComponent();
            Images = new ObservableCollection<BitmapImage>();
            ImageList.ItemsSource = Images;           
            
        }

        public void AddImage(BitmapImage image)
        {
            Images.Add(image);
          //  txtSize.Text = FileSize;
        }
        public void AddAttachment(Attachment attachment)
        {
            Attachments.Add(attachment);
        }
    }
}
