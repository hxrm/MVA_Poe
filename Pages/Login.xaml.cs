using MVA_poe.Classes;
using MVA_Poe;
using MVA_Poe.Classes;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data.SqlClient;
using System.Globalization;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
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
    /// Interaction logic for Login.xaml
    /// </summary>
    public partial class Login : Page
    {
        // Variables for password and dehelper 
        AppDbContext context;
        DBHelper dbHelper;
        string hpWord;
        private Loading load;
        private Thread loadingThread;
        public Login()
        {
            SetLanguage("en");
            InitializeComponent();
            loadingThread = new Thread(() =>
            {
               load = new Loading();
               load.ShowDialog();
            });
            loadingThread.SetApartmentState(ApartmentState.STA); 
         
        }
        private void NewThread()
        {
            loadingThread = new Thread(() =>
            {
                load = new Loading();
                load.ShowDialog();
            });
            loadingThread.SetApartmentState(ApartmentState.STA); 
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
        private void btnSubmit_Click(object sender, RoutedEventArgs e)
        {

           

            String p = txtPas.Password;


            // Checking if the email field is empty
            if (string.IsNullOrWhiteSpace(txtEmail.Text))
            {

                MessageBox.Show("Please enter your email.");
                return;

            }
            
           // Checking if the password field is empty
            if ((string.IsNullOrWhiteSpace(p)))
            {
                MessageBox.Show("Please enter your password.");
                return;
            }
            // Hash the password for comparison
            hpWord = HashPassword(ReturnBytes(SecurePasswordBox.GetPassword(txtPas)));

            // Check if the user exists in the database
            bool userExists = PullData();



            if (loadingThread.ThreadState == ThreadState.Stopped || loadingThread.ThreadState == ThreadState.Unstarted)
            {
                NewThread();
                loadingThread.Start();
            }
            else
            {
                // If the thread is already running, just start it
                loadingThread.Start();
            }
            if (userExists)
            {
              
                MainWindow mainWindow = new MainWindow(dbHelper);
                mainWindow.Show();

                Thread.Sleep(30);
                {
                    load.Dispatcher.Invoke(() => load.Close());
                    Window.GetWindow(this).Close();
                }
                
            }
            else
            {
                load.Dispatcher.Invoke(() => load.Close());                
                MessageBox.Show("Failed to login. Please check your email and password.");
            }
        }
        // method to hash password 
        public static string HashPassword(byte[] passwordArray)
        {
            string password;
            StringBuilder stringBuilder = new StringBuilder();
            foreach (byte b in passwordArray)
            {

                stringBuilder.Append(b.ToString("X2"));
            }
            password = stringBuilder.ToString();
            return password;
        }
        // Method to convert a password into a byte array and hash it
        public static byte[] ReturnBytes(String password)
        {
            // Create an instance of the SHA-256 hashing algorithm.
            using (HashAlgorithm algorithm = SHA256.Create())
            {
                // Convert the plaintext password into a byte array using UTF-8 encoding.
                byte[] passwordBytes = Encoding.UTF8.GetBytes(password);

                // Compute the hash of the password bytes.
                byte[] hashBytes = algorithm.ComputeHash(passwordBytes);

                // Return the resulting hash as a byte array.
                return hashBytes;
            }
        }
        // Method to retrieve user data from the database
        public bool PullData()
        {
            string eMail = txtEmail.Text; // Assuming txtEmail is the TextBox for email input
            string pWord = hpWord; // Hashed password
            bool loggedIn = false;

            using (var context = new AppDbContext())
            {
                var user = context.Users.SingleOrDefault(u => u.email == eMail);

                if (user != null)
                {
                    if (pWord == user.pWord) // Check if the entered password matches the stored hashed password
                    {
                        loggedIn = true;
                        dbHelper = new DBHelper(user.UserId, user.langPref, eMail); // Assuming DBHelper takes user ID as a parameter
                    }
                }
            } 

            return loggedIn;
        }
    }
}