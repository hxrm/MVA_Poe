using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows;

namespace MVA_poe.Classes
{
    public class SecurePasswordBox : DependencyObject
    {
        // Defines the Password attached property for the PasswordBox
        public static readonly DependencyProperty PasswordProperty = DependencyProperty.RegisterAttached(
            "Password", // Property name
            typeof(string), // Type of the property
            typeof(SecurePasswordBox), // Owner type of the property
            new FrameworkPropertyMetadata(string.Empty, OnPasswordPropertyChanged)); // Default value and callback method

        // Getter for the Password property
        public static string GetPassword(DependencyObject d)
        {
            return (string)d.GetValue(PasswordProperty);
        }

        // Setter for the Password property
        public static void SetPassword(DependencyObject d, string value)
        {
            d.SetValue(PasswordProperty, value);
        }

        // Method that is called when the Password property changes
        private static void OnPasswordPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            // Checks if the DependencyObject is a PasswordBox
            if (d is PasswordBox passwordBox)
            {
                // Validate the new password value before setting it
                if (!IsPasswordValid((string)e.NewValue))
                {
                    // Display a message box indicating the password requirements
                    MessageBox.Show("Password must contain at least 8 characters, including upper and lower case letters, a number, and a special character.");

                    // Revert to the previous value (OldValue)
                    SetPassword(passwordBox, (string)e.OldValue);

                    // Exit the method
                    return;
                }

                // Listen for password changes in the PasswordBox
                passwordBox.PasswordChanged -= PasswordChanged;
                passwordBox.PasswordChanged += PasswordChanged;
            }
        }

        // Method called when the PasswordBox's PasswordChanged event is triggered
        private static void PasswordChanged(object sender, RoutedEventArgs e)
        {
            // Checks if the sender is a PasswordBox
            if (sender is PasswordBox passwordBox)
            {
                // Set the Password property with the updated password text
                SetPassword(passwordBox, passwordBox.Password);
            }
        }

        // Method for password validation
        private static bool IsPasswordValid(string password)
        {
            // Define the password criteria using a regular expression
            // Example: At least 8 characters, one uppercase, one lowercase, one number, and one special character
            string passwordPattern = @"^(?=.*[A-Za-z])(?=.*\d)(?=.*[!@#$%^&*])[A-Za-z\d!@#$%^&*]{8,}$";

            // Check if the password matches the defined criteria
            return System.Text.RegularExpressions.Regex.IsMatch(password, passwordPattern);
        }
    }// end class 
}
