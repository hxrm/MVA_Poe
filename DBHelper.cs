using MVA_Poe.Classes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MVA_poe
{
    public class DBHelper
    {
        // VARIABLES
        public static int userID;
        // Default constructor
        public DBHelper() { }

        public DBHelper(int inputUser)
        {
            userID = inputUser;
        //    FindModulesByUserID();
          //  FindSemesterByUserID();
        }

    }
}
