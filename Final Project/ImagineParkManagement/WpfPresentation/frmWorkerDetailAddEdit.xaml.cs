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
    /// Interaction logic for frmWorkerDetailAddEdit.xaml
    /// </summary>
    public partial class frmWorkerDetailAddEdit : Window
    {
        private WorkerViewModel _worker;
        private List<string> _originalUnassignedRoles = new List<string>();
        private bool _addUser = false;
        private bool _admin = false;
        private IWorkerManager _workerManager = new WorkerManager();
        private List<string> _assignedRoles;
        private List<string> _unassignedRoles;
        public frmWorkerDetailAddEdit()
        {
            _worker = new WorkerViewModel();
            _addUser = true;
            _admin = true;

            InitializeComponent();
        }

        public frmWorkerDetailAddEdit(WorkerViewModel worker)
        {
            _worker = worker;
            _admin = true;
            InitializeComponent();
        }
        public frmWorkerDetailAddEdit(int workerID)
        {
            _worker = _workerManager.RetreiveWorkerByID(workerID);
            _admin = false;
            InitializeComponent();
        }
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            if (_admin)
            {
                if (_addUser)
                {
                    txtWorkerID.Text = "Assigned Automatically";
                    txtWorkerID.IsEnabled = false;
                    txtFirstName.Text = "";
                    txtLastName.Text = "";
                    txtEmail.Text = "";
                    txtPhone.Text = "";
                    txtStreetAddress.Text = "";
                    txtZIPCode.Text = "";
                    chkActive.IsChecked = true;
                    dpStartDate.SelectedDate = DateTime.Now;
                    dpStartDate.DisplayDate = DateTime.Now;
                    dpEndDate.SelectedDate = DateTime.MinValue;
                    dpEndDate.Visibility = Visibility.Collapsed;
                    txtEndDate.Visibility = Visibility.Visible;
                    txtEndDate.Text = "N/A";

                    try
                    {
                        _assignedRoles = new List<string>();

                        _unassignedRoles = _workerManager.RetrieveAllRoles();

                        lstAssignedRoles.ItemsSource = _assignedRoles;
                        lstUnassignedRoles.ItemsSource = _unassignedRoles;
                    }
                    catch (Exception ex)
                    {

                        MessageBox.Show(ex.Message + "\n\n" + ex.InnerException.Message);
                    }
                    setupEdit();
                    chkActive.IsEnabled = false;
                }
                else
                {
                    txtWorkerID.Text = _worker.WorkerID.ToString();
                    txtFirstName.Text = _worker.FirstName.ToString();
                    txtLastName.Text = _worker.LastName.ToString();
                    txtEmail.Text = _worker.Email.ToString();
                    txtPhone.Text = _worker.Phone.ToString();
                    txtStreetAddress.Text = _worker.StreetAddress.ToString();
                    txtZIPCode.Text = _worker.ZIPCode.ToString();
                    if (_worker.Active)
                    {
                        chkActive.IsChecked = true;
                        dpEndDate.SelectedDate = DateTime.MinValue;
                        dpEndDate.Visibility = Visibility.Collapsed;
                        txtEndDate.Visibility = Visibility.Visible;
                        txtEndDate.Text = "N/A";
                    }
                    else
                    {
                        chkActive.IsChecked = false;
                        dpEndDate.DisplayDate = _worker.EndDate;
                        dpEndDate.SelectedDate = _worker.EndDate;
                    }
                    dpStartDate.DisplayDate = Convert.ToDateTime(_worker.StartDate);
                    dpStartDate.SelectedDate = Convert.ToDateTime(_worker.StartDate);


                    resetRoles();
                }
            }
            else
            {
                txtWorkerID.Text = _worker.WorkerID.ToString();
                txtFirstName.Text = _worker.FirstName.ToString();
                txtLastName.Text = _worker.LastName.ToString();
                txtEmail.Text = _worker.Email.ToString();
                txtPhone.Text = _worker.Phone.ToString();
                txtStreetAddress.Text = _worker.StreetAddress.ToString();
                txtZIPCode.Text = _worker.ZIPCode.ToString();
                chkActive.IsChecked = true;
                dpStartDate.DisplayDate = Convert.ToDateTime(_worker.StartDate);
                dpStartDate.SelectedDate = Convert.ToDateTime(_worker.StartDate);
                dpEndDate.SelectedDate = DateTime.MinValue;
                dpEndDate.Visibility = Visibility.Collapsed;
                txtEndDate.Visibility = Visibility.Visible;
                txtEndDate.Text = "N/A";

                resetRoles();
            }
            
        }

        private void resetRoles()
        {
            try
            {
                _assignedRoles = _workerManager.RetrieveRolesByWorkerID(_worker.WorkerID);
                _worker.Roles = new List<string>();

                foreach (var r in _assignedRoles)
                {
                    _worker.Roles.Add(r);
                }
                _unassignedRoles = _workerManager.RetrieveAllRoles();

                foreach (var r in _assignedRoles)
                {
                    _unassignedRoles.Remove(r);
                }

                foreach (var r in _unassignedRoles)
                {
                    _originalUnassignedRoles.Add(r);
                }

                lstAssignedRoles.ItemsSource = _assignedRoles;
                lstUnassignedRoles.ItemsSource = _unassignedRoles;
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);
            }
        }
        private void lstAssignedRoles_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            var selectedRole = lstAssignedRoles.SelectedItem;
            _assignedRoles.Remove((string)selectedRole);
            _unassignedRoles.Add((string)selectedRole);

            lstAssignedRoles.ItemsSource = null;
            lstUnassignedRoles.ItemsSource = null;
            lstAssignedRoles.ItemsSource = _assignedRoles;
            lstUnassignedRoles.ItemsSource = _unassignedRoles;
        }

        private void lstUnassignedRoles_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            var selectedRole = lstUnassignedRoles.SelectedItem;
            _assignedRoles.Add((string)selectedRole);
            _unassignedRoles.Remove((string)selectedRole);

            lstAssignedRoles.ItemsSource = null;
            lstUnassignedRoles.ItemsSource = null;
            lstAssignedRoles.ItemsSource = _assignedRoles;
            lstUnassignedRoles.ItemsSource = _unassignedRoles;
        }

        private void btnEditSave_Click(object sender, RoutedEventArgs e)
        {
            if ((string)(btnEditSave.Content) == "Edit")
            {
                setupEdit();
            }
            else
            {
                if(_addUser == false)
                {
                    if (!txtFirstName.Text.IsValidFirstName())
                    {
                        MessageBox.Show("Invaild First Name");
                        txtFirstName.Focus();
                        txtFirstName.SelectAll();
                        return;
                    }
                    if (!txtLastName.Text.IsValidLastName())
                    {
                        MessageBox.Show("Invaild Last Name");
                        txtLastName.Focus();
                        txtLastName.SelectAll();
                        return;
                    }
                    if (!txtEmail.Text.IsValidEmail())
                    {
                        MessageBox.Show("Invaild Email Address");
                        txtEmail.Focus();
                        txtEmail.SelectAll();
                        return;
                    }
                    if (!txtPhone.Text.IsValidPhoneNumber())
                    {
                        MessageBox.Show("Invaild Phone Number");
                        txtPhone.Focus();
                        txtPhone.SelectAll();
                        return;
                    }
                    var newWorker = new WorkerViewModel()
                    {
                        Email = txtEmail.Text,
                        FirstName = txtFirstName.Text,
                        LastName = txtLastName.Text,
                        Phone = txtPhone.Text,
                        StreetAddress = txtStreetAddress.Text,
                        ZIPCode = txtZIPCode.Text,
                        Active = (bool)chkActive.IsChecked,
                        StartDate = (DateTime)dpStartDate.SelectedDate,
                        EndDate = (DateTime)dpEndDate.SelectedDate
                    };
                    List<string> roles = new List<string>();
                    foreach (var role in lstAssignedRoles.Items)
                    {
                        roles.Add((string)role);
                    }
                    newWorker.Roles = roles;
                    try
                    {
                        _workerManager.EditWorkerProfile(_worker, newWorker, _originalUnassignedRoles, _unassignedRoles);
                        this.DialogResult = true;
                    }
                    catch (Exception ex)
                    {
                        resetRoles();
                        if (ex.InnerException.Message.Contains("Worker Could Not Be Deactivated."))
                        {
                            chkActive.IsChecked = true;
                        }
                        MessageBox.Show(ex.Message + "\n\n" + ex.InnerException.Message);
                    }
                }
                else
                {
                    if (!txtFirstName.Text.IsValidFirstName())
                    {
                        MessageBox.Show("Invaild First Name");
                        txtFirstName.Focus();
                        txtFirstName.SelectAll();
                        return;
                    }
                    if (!txtLastName.Text.IsValidLastName())
                    {
                        MessageBox.Show("Invaild Last Name");
                        txtLastName.Focus();
                        txtLastName.SelectAll();
                        return;
                    }
                    if (!txtEmail.Text.IsValidEmail())
                    {
                        MessageBox.Show("Invaild Email Address");
                        txtEmail.Focus();
                        txtEmail.SelectAll();
                        return;
                    }
                    if (!txtPhone.Text.IsValidPhoneNumber())
                    {
                        MessageBox.Show("Invaild Phone Number");
                        txtPhone.Focus();
                        txtPhone.SelectAll();
                        return;
                    }
                    var newWorker = new WorkerViewModel()
                    {
                        Email = txtEmail.Text,
                        FirstName = txtFirstName.Text,
                        LastName = txtLastName.Text,
                        Phone = txtPhone.Text,
                        StreetAddress = txtStreetAddress.Text,
                        ZIPCode = txtZIPCode.Text,
                        Active = (bool)chkActive.IsChecked,
                        StartDate = DateTime.Now,
                        EndDate = DateTime.MinValue
                    };
                    List<string> roles = new List<string>();
                    foreach (var role in lstAssignedRoles.Items)
                    {
                        roles.Add((string)role);
                    }
                    newWorker.Roles = roles;
                    try
                    {
                        _workerManager.AddNewWorker(newWorker);
                        MessageBox.Show("Don't Forget To Update Worker Availability");
                        this.DialogResult = true;
                    }
                    catch (Exception ex)
                    {

                        MessageBox.Show(ex.Message + "\n\n" + ex.InnerException.Message);

                    }
                }
            }
        }

        private void setupEdit()
        {
            btnEditSave.Content = "Save";
            txtFirstName.IsReadOnly = false;
            txtLastName.IsReadOnly = false;
            txtEmail.IsReadOnly = false;
            txtPhone.IsReadOnly = false;
            txtStreetAddress.IsReadOnly = false;
            txtZIPCode.IsReadOnly = false;
            chkActive.IsEnabled = true;
            dpStartDate.IsEnabled = true;
            dpEndDate.IsEnabled = true;
            lstAssignedRoles.IsEnabled = true;
            lstUnassignedRoles.IsEnabled = true;
            txtFirstName.BorderBrush = Brushes.Black;
            txtLastName.BorderBrush = Brushes.Black;
            txtEmail.BorderBrush = Brushes.Black;
            txtPhone.BorderBrush = Brushes.Black;
            txtStreetAddress.BorderBrush = Brushes.Black;
            txtZIPCode.BorderBrush = Brushes.Black;
            chkActive.BorderBrush = Brushes.Black;
            dpStartDate.BorderBrush = Brushes.Black;
            dpEndDate.BorderBrush = Brushes.Black;
            lstAssignedRoles.BorderBrush = Brushes.Black;
            lstUnassignedRoles.BorderBrush = Brushes.Black;
            if (!_admin)
            {
                dpStartDate.IsEnabled = false;
                chkActive.IsEnabled = false;
                dpEndDate.IsEnabled = false;
                txtEndDate.IsEnabled = false;
                lstAssignedRoles.IsEnabled = false;
                lstUnassignedRoles.IsEnabled = false;
            }
            txtFirstName.Focus();

        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
        }
    }
}
