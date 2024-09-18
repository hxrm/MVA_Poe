using MVA_Poe.Classes;
using MVA_poe;
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
using System.Globalization;

namespace MVA_Poe.Pages
{
    /// <summary>
    /// Lógica de interacción para Dashboard.xaml
    /// </summary>
    public partial class Dashboard : Page
    {
        private List<Report> reports = new List<Report>();
        public Dashboard()
        {
            InitializeComponent();
            PopulateRecent();
            SetLanguage(DBHelper.lang);
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
        private void PopulateRecent()
        {
            int userId = DBHelper.userID;

            using (var context = new AppDbContext())
            {
                var recentReport = context.Reports
                    .Where(r => r.userId == userId)
                    .OrderByDescending(r => r.reportDate) // Assuming DateLogged is a DateTime field in your Report model
                    .FirstOrDefault();

                if (recentReport != null)
                {
                   txtReportTitle.Text = $"Report Name: {recentReport.reportName}";
                    txtRportDate.Text = $"Date Logged: {recentReport.reportDate.ToString("yyyy-MM-dd")}";
                    ProgressBar.Value = 10;// Assuming Progress is a property in your Report model
                }
                else
                {
                    txtReportTitle.Text = "No recent reports found.";
                    txtRportDate.Text = string.Empty;
                    ProgressBar.Value = 0;
                }
            }
        }
        private void Border_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            
            this.NavigationService.Navigate(new Uri("Pages/CreateReport.xaml", UriKind.Relative));
            
        }
        private void Border1_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
           
            this.NavigationService.Navigate(new Uri("Pages/Events.xaml", UriKind.Relative));

        }

        private void Border2_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            
            this.NavigationService.Navigate(new Uri("Pages/Service.xaml", UriKind.Relative));

        }
    }
}
