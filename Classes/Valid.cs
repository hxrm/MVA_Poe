using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MVA_poe.Classes
{
    public class Valid
    {

        // Declare a public boolean variable named 'valid' and initialize it to 'false'
        public bool valid = false;


        public bool TryReceiveID(string id, out string idErrorMessage)
        {
            idErrorMessage = string.Empty;

            // Check if ID is exactly 13 characters
            if (id.Length != 13)
            {
                idErrorMessage = "The ID number must be exactly 13 characters long.";
                return false;
            }

            // Optional: Check if the ID contains only digits
            if (!System.Text.RegularExpressions.Regex.IsMatch(id, @"^\d{13}$"))
            {
                idErrorMessage = "The ID number must contain only numeric digits.";
                return false;
            }

            return true;
        }
        // Method: TryReceiveString
        // Validates if a given input is a non-empty string
        public bool TryReceiveString(string input, out string errorMessage)
        {
            errorMessage = "";

            // Check if the input is null, empty, or contains only white spaces
            if (string.IsNullOrWhiteSpace(input))
            {
                errorMessage = "Missing Input ";
                return false;
            }

            return true;
        }

        // Method: TryReceiveNumber
        // Validates if a given input is a valid number greater than 0
        public bool TryReceiveNumber(string input, out string errorMessage)
        {
            errorMessage = "";

            try
            {
                // Try to parse the input as a double using the InvariantCulture
                if (!double.TryParse(input, NumberStyles.Number, CultureInfo.InvariantCulture, out double num))
                {
                    errorMessage = "Please enter a number";
                    return false;
                }

                // Check if the parsed number is less than or equal to 0
                if (num <= 0.0)
                {
                    errorMessage = "Please enter a number greater than 0";
                    return false;
                }

                return true;
            }
            catch (OutOfMemoryException)
            {
                errorMessage = "Please enter a valid number";
                return false;
            }
        }
        // Method: TryReceiveEmail
        // Validates format with @ and .com or co.za
        public bool TryReceiveEmail(string email, out string emailErrorMessage)
        {
            emailErrorMessage = string.Empty;

            // Check for basic email pattern with .com or .co.za domain
            string emailPattern = @"^[a-zA-Z0-9_.+-]+@[a-zA-Z0-9-]+\.(com|co\.za)$";

            if (!System.Text.RegularExpressions.Regex.IsMatch(email, emailPattern))
            {
                emailErrorMessage = "Please enter a valid email in the format 'example@example.com' or 'example@example.co.za'.";
                return false;
            }
            return true;
        }
        // Method: TryReceivePassword
        // Validates a password with at least 8 characters, one special character, one lowercase letter, one uppercase letter, and one number.
        public bool TryReceivePassword(string pWord, out string passwordErrorMessage)
        {
            passwordErrorMessage = "";
            string passwordPattern = @"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[@$!%*?&])[A-Za-z\d@$!%*?&]{8,}$";
            if (string.IsNullOrEmpty(pWord))
            {
                passwordErrorMessage = "Please enter a valid password with at least 8 characters, one special character, one lowercase letter, one uppercase letter, and one number.";
                return false;
            }
            if (!System.Text.RegularExpressions.Regex.IsMatch(pWord, passwordPattern))
            {
                passwordErrorMessage = "Please enter a valid password with at least 8 characters, one special character, one lowercase letter, one uppercase letter, and one number.";
                return false;
            }
            return true;
        }
}
}
