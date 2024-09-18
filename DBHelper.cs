using MVA_Poe.Classes;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace MVA_poe
{
    // Define a public class named DBHelper
    public class DBHelper
    {
       // Declare a public static integer variable named 'userID'
        public static int userID;

        // Declare a public static string variable named 'lang'
        public static string lang;

        // Declare a public static string variable named 'email'
        public static string email;

        // Default constructor
        public DBHelper() { }

        //----------------------------------------------------------------------------//

        // Parameterized constructor
        public DBHelper(int inputUser, string inputLang, string inputEmail)
        {
            // Assign the inputUser parameter to the static userID variable
            userID = inputUser;

            // Assign the inputLang parameter to the static lang variable
            lang = inputLang;

            // Assign the inputEmail parameter to the static email variable
            email = inputEmail;
        }
    }
}

//__---____---____---____---____---____---____---__.ooo END OF FILE ooo.__---____---____---____---____---____---____---__\\