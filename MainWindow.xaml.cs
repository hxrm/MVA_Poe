using MVA_poe;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Interop;

namespace MVA_Poe
{
    public partial class MainWindow : Window
    {
        DBHelper dBHelper;

        public MainWindow()     
        {

            InitializeComponent();
            DBHelper.userID = 1;
        }
        public MainWindow(DBHelper db)
        {
            InitializeComponent();
            SetLanguage(DBHelper.lang);            
            this.dBHelper = db;
           
            if (fContainer.CurrentSource == null)
            {
                fContainer.Navigate(new System.Uri("Pages/Home.xaml", UriKind.Relative));
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
                case "isx":
                    dict.Source = new Uri("Resources/Strings.isx.xaml", UriKind.Relative);
                    break;
                default:
                    dict.Source = new Uri("Resources/Strings.en.xaml", UriKind.Relative);
                    break;
            }
            this.Resources.MergedDictionaries.Add(dict);
        }

        private void BG_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            Tg_Btn.IsChecked = false;
        }

        // Start: MenuLeft PopupButton //
        private void btnCreateReport_MouseEnter(object sender, MouseEventArgs e)
        {
            if (Tg_Btn.IsChecked == false)
            {
                Popup.PlacementTarget = btnCreateReport;
                Popup.Placement = PlacementMode.Right;
                Popup.IsOpen = true;
                Header.PopupText.Text = "Create Report";
            }
           
        }

        private void btnCreateReport_MouseLeave(object sender, MouseEventArgs e)
        {
            Popup.Visibility = Visibility.Collapsed;
            Popup.IsOpen = false;
        }

        private void btnViewReport_MouseEnter(object sender, MouseEventArgs e)
        {
            if (Tg_Btn.IsChecked == false)
            {
                Popup.PlacementTarget = btnViewReport;
                Popup.Placement = PlacementMode.Right;
                Popup.IsOpen = true;
                Header.PopupText.Text = "View Reports";
            }
        }

        private void btnViewReport_MouseLeave(object sender, MouseEventArgs e)
        {
            Popup.Visibility = Visibility.Collapsed;
            Popup.IsOpen = false;
        }

        private void btnLocal_MouseEnter(object sender, MouseEventArgs e)
        {
            if (Tg_Btn.IsChecked == false)
            {
                Popup.PlacementTarget = btnLocal;
                Popup.Placement = PlacementMode.Right;
                Popup.IsOpen = true;
                Header.PopupText.Text = "Local Events";
            }
        }

        private void btnServiceRequest_MouseLeave(object sender, MouseEventArgs e)
        {
            Popup.Visibility = Visibility.Collapsed;
            Popup.IsOpen = false;
        }

        private void btnServiceRequest_MouseEnter(object sender, MouseEventArgs e)
        {
            if (Tg_Btn.IsChecked == false)
            {
                Popup.PlacementTarget = btnServiceRequest;
                Popup.Placement = PlacementMode.Right;
                Popup.IsOpen = true;
                Header.PopupText.Text = "Service";
            }
        }

        private void btnLocal_MouseLeave(object sender, MouseEventArgs e)
        {
            Popup.Visibility = Visibility.Collapsed;
            Popup.IsOpen = false;
        }

        

        private void btnPointOfSale_MouseLeave(object sender, MouseEventArgs e)
        {
            Popup.Visibility = Visibility.Collapsed;
            Popup.IsOpen = false;
        }

        private void btnLogout_MouseEnter(object sender, MouseEventArgs e)
        {
            if (Tg_Btn.IsChecked == false)
            {
                Popup.PlacementTarget = btnLogout;
                Popup.Placement = PlacementMode.Right;
                Popup.IsOpen = true;
                Header.PopupText.Text = "Logout";
            }
        }

        private void btnLogout_MouseLeave(object sender, MouseEventArgs e)
        {
            Popup.Visibility = Visibility.Collapsed;
            Popup.IsOpen = false;
        }
        private void btnSetting_MouseEnter(object sender, MouseEventArgs e)
        {
            if (Tg_Btn.IsChecked == false)
            {
                Popup.PlacementTarget = btnSetting;
                Popup.Placement = PlacementMode.Right;
                Popup.IsOpen = true;
                Header.PopupText.Text = "Setting";
            }
        }

        private void btnSetting_MouseLeave(object sender, MouseEventArgs e)
        {
            Popup.Visibility = Visibility.Collapsed;
            Popup.IsOpen = false;
        }
        // End: MenuLeft PopupButton //

        // Start: Button Close | Restore | Minimize 
        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void btnRestore_Click(object sender, RoutedEventArgs e)
        {
            if (WindowState == WindowState.Normal)
                WindowState = WindowState.Maximized;
            else
                WindowState = WindowState.Normal;
        }

        private void btnMinimize_Click(object sender, RoutedEventArgs e)
        {
            WindowState = WindowState.Minimized;
        }
        // End: Button Close | Restore | Minimize

        private void btnReport_Click(object sender, RoutedEventArgs e)
        {
            // Check if the current page is not the CreateReport page
            if (fContainer.CurrentSource == null || !fContainer.CurrentSource.OriginalString.EndsWith("CreateReport.xaml"))
            {
                // Navigate to the CreateReport page
                fContainer.Navigate(new System.Uri("Pages/CreateReport.xaml", UriKind.Relative));
            }
            else if (fContainer.CurrentSource.OriginalString.EndsWith("CreateReport.xaml"))
            {
                fContainer.Navigate(new System.Uri("Pages/Home.xaml", UriKind.Relative));
            }
        }

        private void btnViewReport_Click(object sender, RoutedEventArgs e)
        {
            // Check if the current page is not the CreateReport page
            if (fContainer.CurrentSource == null || !fContainer.CurrentSource.OriginalString.EndsWith("ViewReport.xaml"))
            {
                // Navigate to the CreateReport page
                fContainer.Navigate(new System.Uri("Pages/ViewReport.xaml", UriKind.Relative));
            }
            else if (fContainer.CurrentSource.OriginalString.EndsWith("ViewReport.xaml"))
            {
                fContainer.Navigate(new System.Uri("Pages/Home.xaml", UriKind.Relative));
            }

        }

        private void btnSetting_Click(object sender, RoutedEventArgs e)
        {
            // Check if the current page is not the CreateReport page
            if (fContainer.CurrentSource == null || !fContainer.CurrentSource.OriginalString.EndsWith("Profile.xaml"))
            {
                // Navigate to the CreateReport page
                fContainer.Navigate(new System.Uri("Pages/Profile.xaml", UriKind.Relative));
            }
            else if (fContainer.CurrentSource.OriginalString.EndsWith("Profile.xaml"))
            {
                fContainer.Navigate(new System.Uri("Pages/Home.xaml", UriKind.Relative));
            }
        }

        private void btnLogout_Click(object sender, RoutedEventArgs e)
        {
            DBHelper.userID = 0;
            Auth auth = new Auth();
            auth.Show();
            Window.GetWindow(this).Close(); // Close the current window
        }


        [DllImport("user32.DLL", EntryPoint = "ReleaseCapture")]
        private extern static void ReleaseCapture();

        [DllImport("user32.DLL", EntryPoint = "SendMessage")]
        private extern static void SendMessage(System.IntPtr hWnd, int wMsg, int wParam, int lParam);


        private void Border_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
            {
                ReleaseCapture();
                IntPtr windowHandle = new WindowInteropHelper(this).Handle;
                SendMessage(windowHandle, 0x112, 0xf012, 0);
            }
        }
    }
}
