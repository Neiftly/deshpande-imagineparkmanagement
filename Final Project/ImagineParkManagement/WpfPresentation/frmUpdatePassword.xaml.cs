using DataObjects;
using LogicLayer;
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

namespace WpfPresentation
{
    /// <summary>
    /// Interaction logic for frmUpdatePassword.xaml
    /// </summary>
    public partial class frmUpdatePassword : Window
    {
        private User _user;
        private IUserManager _userManager;
        private bool _isNewUser;
        public frmUpdatePassword(User user, IUserManager userManager, bool isNewUser = false)
        {
            _user = user;
            _userManager = userManager;
            _isNewUser = isNewUser;
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            if (_isNewUser)
            {
                tbkMessage.Text = "On First Log In " + tbkMessage.Text;
                txtEmail.Text = _user.Email;
                txtEmail.IsEnabled = false;
                pwdOldPassword.Password = "newuser";
                pwdOldPassword.IsEnabled = false;
                pwdNewPassword.Focus();
            }
        }

        private void btnSubmit_Click(object sender, RoutedEventArgs e)
        {
            if (!txtEmail.Text.IsValidEmail() || txtEmail.Text != _user.Email)
            {
                MessageBox.Show("Invalid Email Address.");
                txtEmail.Clear();
                txtEmail.Focus();
                return;
            }
            if (!pwdOldPassword.Password.IsValidPassword())
            {
                MessageBox.Show("Invalid Password.");
                pwdOldPassword.Clear();
                pwdOldPassword.Focus();
                return;
            }
            if (!pwdNewPassword.Password.IsValidPassword() || pwdNewPassword.Password.Equals("newuser"))
            {
                MessageBox.Show("Invalid Password.");
                pwdNewPassword.Clear();
                pwdNewPassword.Focus();
                return;
            }
            if (!pwdNewPassword.Password.Equals(pwdRetypePassword.Password))
            {
                MessageBox.Show("Passwords Must Match.");
                pwdRetypePassword.Clear();
                pwdRetypePassword.Focus();
                return;
            }
            try
            {
                if(_userManager.UpdatePassword(_user, pwdOldPassword.Password, pwdNewPassword.Password))
                {
                    MessageBox.Show("Password Change.", "Profile Update", MessageBoxButton.OK, MessageBoxImage.Information);
                    this.DialogResult = true;
                }
                else
                {
                    throw new ApplicationException("Password Change Failed, Password Not Changed.");
                }
            }
            catch (Exception ex)
            {

                pwdNewPassword.Clear();
                pwdRetypePassword.Clear();
                MessageBox.Show(ex.Message + "\n\n" + ex.InnerException.Message);
                pwdNewPassword.Focus();
            }
        }

        private void pwdRetypePassword_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                btnSubmit_Click(sender, e);
            }
        }
    }
}
