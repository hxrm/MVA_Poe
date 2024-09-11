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
        public DateTime validDate;

        // Method: TryDate
        // Validates if a given input is a valid date
        public bool TryDate(string input, out string errorMessage, out DateTime date)
        {
            errorMessage = "";

            // Check if the input is null or empty
            if (string.IsNullOrWhiteSpace(input))
            {
                errorMessage = "Missing Input";
                date = DateTime.MinValue; // Set date to a default value
                return false;
            }

            // Try to parse the input as a DateTime
            if (!DateTime.TryParse(input, out date))
            {
                errorMessage = "Input is not a valid date";
                return false;
            }
            this.validDate = date;
            return true;
        }

        // Method: TryReceiveModuleCode
        // Validates if a given input is a valid module code in the format "CLDV6233"        
        public bool TryReceiveModuleCode(string input, out string errorMessage)
        {
            string pattern = @"^[A-Za-z]{4}\d{4}$";
            errorMessage = "";

            // Check if the input is null or empty
            if (string.IsNullOrWhiteSpace(input))
            {
                errorMessage = "Missing Input ";
                return false;
            }

            // Use a regular expression to validate the input against the specified pattern
            if (!System.Text.RegularExpressions.Regex.IsMatch(input, pattern))
            {
                errorMessage = " Code format follows CLDV6233 ";
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
