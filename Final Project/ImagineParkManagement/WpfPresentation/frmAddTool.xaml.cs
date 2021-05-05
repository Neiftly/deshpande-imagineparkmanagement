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
    /// Interaction logic for frmAddTool.xaml
    /// </summary>
    public partial class frmAddTool : Window
    {
        private IToolManager _toolManager = new ToolManager();
        public frmAddTool()
        {
            InitializeComponent();
        }

        private void btnOk_Click(object sender, RoutedEventArgs e)
        {
            if (txtToolName.Text == null)
            {
                MessageBox.Show("You Must Fill Out The Fields");
                txtToolName.Focus();
                return;
            }
            else
            {
                try
                {
                    _toolManager.AddNewTool(txtToolName.Text);
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

        private void txtToolName_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                btnOk_Click(sender, e);
            }
        }
    }
}
