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
using MVA_Poe.Classes;

namespace MVA_poe.Pages
{
    public partial class Profile : Page
    {
        private  User _user = new User();
        int userId;
        private string lang;
        private bool edits;
        
        public Profile()
        {
            InitializeComponent();
            userId = DBHelper.userID;
            GetData();

        }
        private void GetData()
        {
                     

            using (var context = new AppDbContext())
            {
                _user = context.Users
                    .Where(u => u.UserId == userId)//
                    .FirstOrDefault();
                
            }

            if (_user != null)
            {
                txtFName.Text = _user.fName;
                txtLName.Text = _user.lName;
                txtEmail.Text = _user.email;
                txtID.Text = _user.ID;
                txtUsername.Text = _user.uName;
                txtAddress.Text = _user.address;
                lang = _user.langPref;
                GetCheckData();
            }

        }
        private void GetCheckData()
        {
            if (lang == "en")
            {
                chkEn.IsChecked = true;
            }
            else if (lang == "af")
            {
                chkAf.IsChecked = true;
            }
            else if (lang == "isx")
            {
                chkIsx.IsChecked = true;
            }
        }
        private void LangPref_Checked(object sender, RoutedEventArgs e)
        {
            if (sender == chkEn)
            {
                chkAf.IsChecked = false;
                chkIsx.IsChecked = false;
            }
            else if (sender == chkAf)
            {
                chkEn.IsChecked = false;
                chkIsx.IsChecked = false;
            }
            else if (sender == chkIsx)
            {
                chkEn.IsChecked = false;
                chkAf.IsChecked = false;
            }
        }
        private void EditButton_Click(object sender, RoutedEventArgs e)
        {
            txtAddress.IsEnabled = true;
            txtEmail.IsEnabled = true;
            txtFName.IsEnabled = true;
            txtLName.IsEnabled = true;
            txtUsername.IsEnabled = true;
            txtID.IsEnabled = true;
            chkAf.IsEnabled = true;
            chkEn.IsEnabled = true;
            chkIsx.IsEnabled = true;
            SaveButton.IsEnabled = true;
            edits = true;
        }
        private void Disabled()
        {
            txtAddress.IsEnabled = false;
            txtEmail.IsEnabled = false;
            txtFName.IsEnabled = false;
            txtLName.IsEnabled = false;
            txtUsername.IsEnabled = false;
            txtID.IsEnabled = false;
            chkAf.IsEnabled = false;
            chkEn.IsEnabled = false;
            chkIsx.IsEnabled = false;
            SaveButton.IsEnabled = false;
        }

        private void SaveButton_Click_1(object sender, RoutedEventArgs e)
        {
            if (edits)
            {
                 
                if (chkAf.IsChecked == true)
                {
                    _user.langPref = "af";
                }
                else if (chkEn.IsChecked == true)
                {
                    _user.langPref = "en";
                }
                else if (chkIsx.IsChecked == true)
                {
                    _user.langPref = "isx";
                }
                using (var context = new AppDbContext())
                {
                    _user = context.Users
                        .Where(u => u.UserId == userId)//
                        .FirstOrDefault();

                    //_user.fName = txtFName.Text;
                    //_user.lName = txtLName.Text;
                    //_user.email = txtEmail.Text;
                    //_user.ID = txtID.Text;
                    //_user.uName = txtUsername.Text;
                    //_user.address = txtAddress.Text;
                    _user.langPref = _user.langPref;
                    context.SaveChanges();
                }

            }
            MessageBox.Show("Profile Updated");
            Disabled();           
            
        }
    }
}
