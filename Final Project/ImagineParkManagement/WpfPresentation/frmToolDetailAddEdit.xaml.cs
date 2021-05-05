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
    /// Interaction logic for frmToolAddEdit.xaml
    /// </summary>
    public partial class frmToolAddEdit : Window
    {
        private List<ProjectViewModel> projects = new List<ProjectViewModel>();
        private IToolManager _toolManager = new ToolManager();
        private IProjectManager _projectManager = new ProjectManager();
        private int _projectID;
        private Tool _tool;
        public frmToolAddEdit(Tool tool)
        {
            _tool = tool;
            projects = _projectManager.RetrieveAllProjects();
            _projectID = _toolManager.RetrieveProjectByToolID(_tool.ToolID);
            InitializeComponent();
        }

        private void btnEditSave_Click(object sender, RoutedEventArgs e)
        {
            var selectedItem = Convert.ToInt32(cmbProjectID.SelectedItem);
            try
            {
                if (_projectID == 0)
                {
                    _toolManager.AddToolToProject(selectedItem, _tool.ToolID);
                }
                else
                {
                    _toolManager.RemoveToolFromProject(selectedItem, _tool.ToolID);
                }
                
                this.DialogResult = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message + "\n\n" + ex.InnerException.Message);
                
            }
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            txtToolID.Text = _tool.ToolID.ToString();
            foreach (var project in projects)
            {
                cmbProjectID.Items.Add(project.ProjectID.ToString());
                
            }
            cmbProjectID.Text = _projectID.ToString();
            cmbProjectID.SelectedItem = _projectID.ToString();

        }
    }
}
