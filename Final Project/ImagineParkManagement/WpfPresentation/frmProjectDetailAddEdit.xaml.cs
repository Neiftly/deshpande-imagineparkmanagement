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
    /// Interaction logic for frmProjectDetailAddEdit.xaml
    /// </summary>
    public partial class frmProjectDetailAddEdit : Window
    {
        private ProjectViewModel _project;
        private IProjectManager _projectManager = new ProjectManager();
        private IWorkerManager _workerManager = new WorkerManager();
        private IToolManager _toolManager = new ToolManager();
        private bool _addProject = false;
        private bool _admin = false;
        private List<int> _workerIDs = new List<int>();
        private List<string> _projectNames = new List<string>();
        List<Tool> _tools = new List<Tool>();
        List<string> _projectTools = new List<string>();

        public frmProjectDetailAddEdit()
        {
            _project = new ProjectViewModel();
            _addProject = true;
            _admin = true;
            foreach (var worker in _workerManager.RetrieveWorkersByActive())
            {
                _workerIDs.Add(worker.WorkerID);
            }
            _projectNames = _projectManager.RetrieveAllProjectNames();
            InitializeComponent();
        }

        public frmProjectDetailAddEdit(ProjectViewModel project, bool admin)
        {
            _project = project;
            _admin = admin;
            _tools = _projectManager.RetrieveToolsByProjectID(_project.ProjectID);
            foreach (var tool in _tools)
            {
                _projectTools.Add(tool.ToolDescription);
            }
            foreach (var worker in _workerManager.RetrieveWorkersByActive())
            {
                _workerIDs.Add(worker.WorkerID);
            }
            _projectNames = _projectManager.RetrieveAllProjectNames();

            InitializeComponent();
        }


        private void btnEditSave_Click(object sender, RoutedEventArgs e)
        {
            if((string)(btnEditSave.Content) == "Start")
            {
                if (cmbWorkerID.SelectedItem == null)
                {
                    MessageBox.Show("Must Select A Worker!");
                    cmbWorkerID.Focus();
                    return;
                }
                if (cmbProjectName.SelectedItem == null)
                {
                    MessageBox.Show("Must Select A Project Type!");
                    cmbProjectName.Focus();
                    return;
                }
                var _newProject = new ProjectViewModel()
                {
                    WorkerID = Convert.ToInt32(cmbWorkerID.SelectedItem),
                    Paid = (bool)chkPaid.IsChecked,
                    StartDate = (DateTime)dpStartDate.SelectedDate,
                    ProjectName = cmbProjectName.SelectedItem.ToString()
                };
                try
                {
                    _projectManager.AddNewProject(_newProject);
                    this.DialogResult = true;
                }
                catch (Exception ex)
                {

                    MessageBox.Show(ex.Message);
                }
            }
            else if ((string)(btnEditSave.Content) == "Edit")
            {
                setupEdit();
            }
            else
            {
                if (_admin)
                {
                    if (cmbWorkerID.SelectedItem == null)
                    {
                        MessageBox.Show("Must Select A Worker!");
                        cmbWorkerID.Focus();
                        return;
                    }
                    if (cmbProjectName.SelectedItem == null)
                    {
                        MessageBox.Show("Must Select A Project Type!");
                        cmbProjectName.Focus();
                        return;
                    }
                    var _newProject = new ProjectViewModel()
                    {
                        WorkerID = Convert.ToInt32(cmbWorkerID.SelectedItem),
                        Paid = (bool)chkPaid.IsChecked,
                        StartDate = (DateTime)dpStartDate.SelectedDate,
                        ProjectName = cmbProjectName.SelectedItem.ToString()
                    };
                    try
                    {
                        _projectManager.EditProjectDetail(_project, _newProject);
                        this.DialogResult = true;
                    }
                    catch (Exception ex)
                    {

                        MessageBox.Show(ex.Message);
                    }
                }
                else
                {
                    this.DialogResult = true;
                }
            }
        }

        private void setupEdit()
        {
            cmbWorkerID.IsEnabled = true;
            chkPaid.IsEnabled = true;
            cmbProjectName.IsEnabled = true;

            dpStartDate.IsEnabled = true;

            btnEditSave.Content = "OK";
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            if (_admin)
            {
                if (_addProject)
                {
                    tbkTitle.Text = "Start New Project";
                    txtProjectID.Text = "Assigned Automatically";
                    txtProjectID.IsEnabled = false;
                    cmbWorkerID.ItemsSource = _workerIDs;
                    cmbWorkerID.IsEnabled = true;
                    chkPaid.IsChecked = true;
                    chkPaid.IsEnabled = true;
                    dpStartDate.IsEnabled = true;
                    foreach (var name in _projectNames)
                    {
                        cmbProjectName.Items.Add(name);
                    }
                    cmbProjectName.IsEnabled = true;

                    btnEditSave.Content = "Start";
                }
                else
                {
                    tbkTitle.Text = "Edit Project";
                    txtProjectID.Text = _project.ProjectID.ToString();
                    txtProjectID.IsEnabled = false;
                    cmbWorkerID.ItemsSource = _workerIDs;
                    cmbWorkerID.Text = _project.WorkerID.ToString();
                    cmbWorkerID.SelectedItem = _project.WorkerID.ToString();
                    cmbWorkerID.IsEnabled = false;
                    chkPaid.IsChecked = _project.Paid;
                    chkPaid.IsEnabled = false;
                    dpStartDate.SelectedDate = _project.StartDate;
                    dpStartDate.DisplayDate = _project.StartDate;
                    foreach (var name in _projectNames)
                    {
                        cmbProjectName.Items.Add(name);
                    }
                    cmbProjectName.SelectedItem = _project.ProjectName;
                    cmbProjectName.Text = _project.ProjectName;
                    cmbProjectName.IsEnabled = false;

                    btnEditSave.Content = "Edit";
                }
            }
            else
            {
                tbkTitle.Text = "Project Details";
                txtProjectID.Text = _project.ProjectID.ToString();
                txtProjectID.IsEnabled = false;
                cmbWorkerID.ItemsSource = _workerIDs;
                cmbWorkerID.SelectedItem = _project.WorkerID.ToString();
                cmbWorkerID.IsEnabled = false;
                chkPaid.IsChecked = _project.Paid;
                chkPaid.IsEnabled = false;
                dpStartDate.SelectedDate = _project.StartDate;
                foreach (var name in _projectNames)
                {
                    cmbProjectName.Items.Add(name);
                }
                cmbProjectName.SelectedItem = _project.ProjectName;
                cmbProjectName.IsEnabled = false;
                lblTools.Visibility = Visibility.Visible;
                lstTools.Visibility = Visibility.Visible;
                lstTools.ItemsSource = _projectTools;

                btnEditSave.Content = "OK";
            }
        }
    }
}
