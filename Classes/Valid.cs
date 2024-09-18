using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MVA_poe.Classes
{
    public class Valid // Defines a public class named Valid
    {
        // Declare a public boolean variable named 'valid' and initialize it to 'false'
        public bool valid = false;

        // Method: TryReceiveID
        // Validates if a given ID is exactly 13 characters long and contains only digits
        public bool TryReceiveID(string id, out string idErrorMessage)
        {
            // Initialize the output parameter to an empty string
            idErrorMessage = string.Empty;

            // Check if ID is exactly 13 characters
            if (id.Length != 13)
            {
                // Set the error message if the ID is not 13 characters long
                idErrorMessage = "The ID number must be exactly 13 characters long.";
                return false;
            }

            // Optional: Check if the ID contains only digits
            if (!System.Text.RegularExpressions.Regex.IsMatch(id, @"^\d{13}$"))
            {
                // Set the error message if the ID contains non-numeric characters
                idErrorMessage = "The ID number must contain only numeric digits.";
                return false;
            }

            // Return true if the ID is valid
            return true;
        }

        // Method: TryReceiveString
        // Validates if a given input is a non-empty string
        public bool TryReceiveString(string input, out string errorMessage)
        {
            // Initialize the output parameter to an empty string
            errorMessage = "";

            // Check if the input is null, empty, or contains only white spaces
            if (string.IsNullOrWhiteSpace(input))
            {
                // Set the error message if the input is invalid
                errorMessage = "Missing Input ";
                return false;
            }

            // Return true if the input is valid
            return true;
        }

        // Method: TryReceiveNumber
        // Validates if a given input is a valid number greater than 0
        public bool TryReceiveNumber(string input, out string errorMessage)
        {
            // Initialize the output parameter to an empty string
            errorMessage = "";

            try
            {
                // Try to parse the input as a double using the InvariantCulture
                if (!double.TryParse(input, NumberStyles.Number, CultureInfo.InvariantCulture, out double num))
                {
                    // Set the error message if the input is not a valid number
                    errorMessage = "Please enter a number";
                    return false;
                }

                // Check if the parsed number is less than or equal to 0
                if (num <= 0.0)
                {
                    // Set the error message if the number is not greater than 0
                    errorMessage = "Please enter a number greater than 0";
                    return false;
                }

                // Return true if the input is a valid number greater than 0
                return true;
            }
            catch (OutOfMemoryException)
            {
                // Set the error message if an exception occurs
                errorMessage = "Please enter a valid number";
                return false;
            }
        }

        // Method: TryReceiveEmail
        // Validates if a given email is in the correct format with @ and .com or co.za
        public bool TryReceiveEmail(string email, out string emailErrorMessage)
        {
            // Initialize the output parameter to an empty string
            emailErrorMessage = string.Empty;

            // Check for basic email pattern with .com or .co.za domain
            string emailPattern = @"^[a-zA-Z0-9_.+-]+@[a-zA-Z0-9-]+\.(com|co\.za)$";

            // Validate the email against the pattern
            if (!System.Text.RegularExpressions.Regex.IsMatch(email, emailPattern))
            {
                // Set the error message if the email is not in the correct format
                emailErrorMessage = "Please enter a valid email in the format 'example@example.com' or 'example@example.co.za'.";
                return false;
            }

            // Return true if the email is valid
            return true;
        }

        // Method: TryReceivePassword
        // Validates a password with at least 8 characters, one special character, one lowercase letter, one uppercase letter, and one number.
        public bool TryReceivePassword(string pWord, out string passwordErrorMessage)
        {
            // Initialize the output parameter to an empty string
            passwordErrorMessage = "";

            // Define the password pattern
            string passwordPattern = @"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[@$!%*?&])[A-Za-z\d@$!%*?&]{8,}$";

            // Check if the password is null or empty
            if (string.IsNullOrEmpty(pWord))
            {
                // Set the error message if the password is invalid
                passwordErrorMessage = "Please enter a valid password with at least 8 characters, one special character, one lowercase letter, one uppercase letter, and one number.";
                return false;
            }

            // Validate the password against the pattern
            if (!System.Text.RegularExpressions.Regex.IsMatch(pWord, passwordPattern))
            {
                // Set the error message if the password does not match the pattern
                passwordErrorMessage = "Please enter a valid password with at least 8 characters, one special character, one lowercase letter, one uppercase letter, and one number.";
                return false;
            }

            // Return true if the password is valid
            return true;
        }
    }
}

//__---____---____---____---____---____---____---__.ooo END OF FILE ooo.__---____---____---____
