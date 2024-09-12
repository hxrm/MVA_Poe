using MVA_Poe;
using System;
using System.Collections.Generic;
using System.ComponentModel;
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
using System.Windows.Shapes;

namespace MVA_poe
{
    /// <summary>
    /// Interaction logic for Loading.xaml
    /// </summary>
    public partial class Loading : Window
    {
       
        public static int progress = 0;
        public Loading()
        {
            InitializeComponent();
            UpdateProgress(progress);
         
        }        

        private void Window_ContentRendered(object sender, EventArgs e)
        {

            BackgroundWorker worker = new BackgroundWorker();
            worker.WorkerReportsProgress = true;
            worker.DoWork += worker_DoWork;
            worker.ProgressChanged += worker_ProgressChanged;
            worker.RunWorkerAsync();
        }

        void worker_DoWork(object sender, DoWorkEventArgs e)
        {
            for (int i = 0; i < 100; i++)
            {
                (sender as BackgroundWorker).ReportProgress(i);
                System.Threading.Thread.Sleep(80);
            }
        }
       public void worker_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {

            progressBar.Value = e.ProgressPercentage;
           

        }
        public void UpdateProgress(int value)
        {
            progress = value;
            progressBar.Value = progress;
           
        }
    }
}
