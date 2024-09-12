using System;
using System.Collections.Generic;
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

namespace MVA_Poe.Controls
{
    /// <summary>
    /// Interaction logic for FileDetail.xaml
    /// </summary>
    public partial class FileDetail : UserControl
    {

        public FileDetail()
        {
            InitializeComponent();
        }

        public string FileName
        {
            get { return (string)GetValue(FileNameProperty); }
            set { SetValue(FileNameProperty, value); }

        }

        // Using a DependencyProperty as the backing store for FileName. This enables animation, styling, binding, etc ...
        public static readonly DependencyProperty FileNameProperty =
        DependencyProperty.Register("FileName", typeof(string), typeof(FileDetail));


        public string FileSize
        {
            get { return (string)GetValue(FileSizeProperty); }
            set { SetValue(FileSizeProperty, value); }

        }

        // Using a DependencyProperty as the backing store for FileSize. This enables animation, styling, binding, etc ...
        public static readonly DependencyProperty FileSizeProperty =
          DependencyProperty.Register("FileSize", typeof(string), typeof(FileDetail));


        public int UploadProgress
        {
            get { return (int)GetValue(UploadProgressProperty); }
            set { SetValue(UploadProgressProperty, value); }
        }

        public static readonly DependencyProperty UploadProgressProperty =
            DependencyProperty.Register("UploadProgress", typeof(int), typeof(FileDetail));


        public int UploadSpeed
        {
            get { return (int)GetValue(UploadSpeedProperty); }
            set { SetValue(UploadSpeedProperty, value); }
        }

        public static readonly DependencyProperty UploadSpeedProperty =
            DependencyProperty.Register("UploadSpeed", typeof(int), typeof(FileDetail));
    }
}