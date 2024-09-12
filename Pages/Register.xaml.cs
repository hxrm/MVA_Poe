using MVA_poe.Classes;
using MVA_Poe.Classes;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Globalization;
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
using System.Xml.Linq;

namespace MVA_poe.Pages
{
    /// <summary>
    /// Interaction logic for Register.xaml
    /// </summary>
    public partial class Register : Page
    {
        private Valid v = new Valid();
        AppDbContext context;

        // Variables for password, email validation, and threads
        string hpWord;
        bool validPWord, validEMail, validFName, validSName, validID, validLoc;
        // Error messages for various fields and Validation instance
        string fNameErrorMessage, sNameErrorMessage, emailErrorMessage, passwordErrorMessage, locErrorMessage, IDErrorMessage;

        public Register()
        {
           
            InitializeComponent(); 
            SetLanguage("en");
            context = new AppDbContext();
        }

        private void btnSubmit_Click(object sender, RoutedEventArgs e)
        {
            // Gather input data from the form fields
            string firstName = txtName.Text;
            string lastName = txtLastName.Text;
            string email = txtEmail.Text;
            string idNumber = txtID.Text;
            string location = txtLoc.Text;
            string languagePreference = SetLang();  
            string password = txtPas.Password;
            string confirmPassword = txtCPas.Password;

            bool cont = InputCheck(firstName, lastName, email, idNumber, location, password, confirmPassword);     
            if (cont == false)
            {
                MessageBox.Show("Please fill in all fields");
                return;
            }
           
            
            // Validate input data (optional)
            ProcessPassword(password, confirmPassword);
            ProcessEmail(email);


            validFName = v.TryReceiveString(txtName.Text, out fNameErrorMessage);
            fNameError.Text = fNameErrorMessage;
            fNameError.Visibility = Visibility.Visible;

            validSName = v.TryReceiveString(txtLastName.Text, out sNameErrorMessage);
            sNameError.Text = sNameErrorMessage;
            sNameError.Visibility = Visibility.Visible;

            validLoc = v.TryReceiveString(txtLastName.Text, out locErrorMessage);
            locError.Text = locErrorMessage;
            locError.Visibility = Visibility.Visible;

            validID = v.TryReceiveID(txtID.Text, out IDErrorMessage);
            idError.Text = IDErrorMessage;
            idError.Visibility = Visibility.Visible;

            if (validFName && validSName && validEMail && validPWord &&validID && validLoc)
            { 
                    hpWord = HashPassword(returnBytes(SecurePasswordBox.GetPassword(txtPas)));
                    // Create a new User object
                    User newUser = new User
                    {
                        fName = firstName,
                        lName = lastName,
                        email = email,
                        ID = idNumber,
                        address = location,
                        langPref = languagePreference,
                        pWord = hpWord // Ensure you handle password hashing in a real application
                    };

                    context.Users.Add(newUser);
                    context.SaveChanges();
                    // Optionally, show a message or clear the form
                    MessageBox.Show("User registered successfully!");
                    ClearForm();
                
            }
        }
        private bool InputCheck(string firstName, string lastName, string email, string idNumber, string location, string password, string confirmPassword)
        {
            bool valid = true;
            // Check if any input field is empty
            if (string.IsNullOrEmpty(firstName) && string.IsNullOrEmpty(lastName) && string.IsNullOrEmpty(email) &&
                string.IsNullOrEmpty(idNumber) && string.IsNullOrEmpty(location) && string.IsNullOrEmpty(password) &&
                string.IsNullOrEmpty(confirmPassword))
            {

                valid = false;
            }
            return valid;           
            
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
        public string SetLang()
        {
            string lang = "en"; 

            if (cbLang.SelectedIndex == 0)
            {
                lang = "en";
            }
            if (cbLang.SelectedIndex == 2)
            {
               // lang = "af";
            }
            if (cbLang.SelectedIndex == 3)
            {
               // lang = "isx";
            }

            return lang;
        }
     
        public void ProcessPassword(string password, string confirm)
        {
            string passwordErrorMessage = string.Empty;

            string pWord = password;
            string confP = confirm;

            // if password does not match confirmation password 
            if (pWord != confP)
            {
                conPError.Visibility = Visibility.Visible;
                conPError.Text = "Passwords do not match!";
                //   MessageBox.Show("Passwords do not match!");
                return;
            }
            else
            {
                // if match pass to validation method 
                bool validPWord = v.TryReceivePassword(pWord, out passwordErrorMessage);
                passError.Text = passwordErrorMessage;
                conPError.Visibility = Visibility.Hidden;
                passError.Visibility = Visibility.Visible;
            }
        }

        public void ProcessEmail(string email)
        {
            string emailErrorMessage = string.Empty;
            bool validEmail = v.TryReceiveEmail(email, out emailErrorMessage);
            emailError.Text = emailErrorMessage;
            emailError.Visibility = Visibility.Visible;

            //remove this line
            validEMail = true;
            // if email returns valid 
            if (validEmail)
            {
                // pass email to bool return method to ensure is a new user email
                bool newEmail = CheckData(email);
                //if email already registered 
                if (newEmail)
                {// display error 
                    emailError.Text = "Email has already been registered";
                    emailError.Visibility = Visibility.Visible;
                    return;
                }
                else
                {
                    this.validEMail = true;
                }
            }
           
        }

        public bool CheckData(string email)
        {
            bool registered = false;

            using (var context = new AppDbContext())
            {
                // Check if the email exists in the database using LINQ
                var user = context.Users.FirstOrDefault(u => u.email == email);

                if (user != null)
                {
                    registered = true;
                }
            }

            return registered;
        }

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

        // Method to convert password to bytes and hash
        public static byte[] returnBytes(string password)
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

        private void ClearForm()
        {
            txtName.Text = string.Empty;
            txtLastName.Text = string.Empty;
            txtEmail.Text = string.Empty;
            txtID.Text = string.Empty;
            txtLoc.Text = string.Empty;
            cbLang.SelectedValue = null;
            txtPas.Password = string.Empty;
            txtCPas.Password = string.Empty;
            Reset();
        }
        public void Reset()
        {
            fNameError.Visibility = Visibility.Hidden;
            sNameError.Visibility = Visibility.Hidden;
            emailError.Visibility = Visibility.Hidden;
            idError.Visibility = Visibility.Hidden;
            locError.Visibility = Visibility.Hidden;       
           
            passError.Visibility = Visibility.Hidden;
            conPError.Visibility = Visibility.Hidden;
        }
    }
}
