using MVA_poe.Classes;
using MVA_Poe;
using MVA_Poe.Classes;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Security.Cryptography;
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
    /// Interaction logic for Login.xaml
    /// </summary>
    public partial class Login : Page
    {
        // Variables for password and dehelper 
        AppDbContext context;
        DBHelper dbHelper;
        string hpWord;
        public Login()
        {
            InitializeComponent();
        }

        private void btnSubmit_Click(object sender, RoutedEventArgs e)
        {
            // Checking if the email field is empty
            if (string.IsNullOrWhiteSpace(txtEmail.Text))
            {
                MessageBox.Show("Please enter your email.");
                return;
            }
            String p = txtPas.Password;          
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
            if (userExists)
            {
                // If the user exists, display the MainWindow
                MainWindow mainWindow = new MainWindow(dbHelper);
                mainWindow.Show();
                Window.GetWindow(this).Close(); // Close the current window
            }
            else
            {
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
                        dbHelper = new DBHelper(user.UserId); // Assuming DBHelper takes user ID as a parameter
                    }
                }
            } 

            return loggedIn;
        }
    }
}