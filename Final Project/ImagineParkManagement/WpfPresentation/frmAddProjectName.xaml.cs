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
using System.Windows.Shapes;

namespace WpfPresentation
{
    /// <summary>
    /// Interaction logic for frmAddProjectName.xaml
    /// </summary>
    public partial class frmAddProjectName : Window
    {
        ProjectManager _projectManager = new ProjectManager();
        public frmAddProjectName()
        {
            InitializeComponent();
        }

        private void btnOk_Click(object sender, RoutedEventArgs e)
        {
            if (txtProjectName.Text == null || txtProjectDescription.Text == null)
            {
                MessageBox.Show("You Must Fill Out The Fields");
                txtProjectName.Focus();
                return;
            }
            else
            {
                try
                {
                    _projectManager.AddNewProjectName(txtProjectName.Text, txtProjectDescription.Text);
                    this.DialogResult = true;
                }
                catch (Exception ex)
                {

                    MessageBox.Show(ex.Message + "\n\n" + ex.InnerException.Message);
                }
            }
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
        }

        private void txtProjectDescription_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                btnOk_Click(sender, e);
            }
        }
    }
}
