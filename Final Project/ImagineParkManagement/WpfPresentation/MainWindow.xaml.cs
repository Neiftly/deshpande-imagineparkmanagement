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

/*
 * Image for Landing screen from Lukasz Szmigiel on Unsplash
 * Some Code based on camp_management in class example. Modified and adjust
 * for the purposes of this assignment. There are things i would change and fix if
 * i had a better understanding and more time.
 */

namespace WpfPresentation
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private IUserManager _userManager = new UserManager();
        private IProjectManager _projectManager = new ProjectManager();
        private IToolManager _toolManager = new ToolManager();
        private User _user = null;
        public MainWindow()
        {
            InitializeComponent();
        }

        private void MainWin_Loaded(object sender, RoutedEventArgs e)
        {
            resetWindow();
        }

        private void btnLogIn_Click(object sender, RoutedEventArgs e)
        {
            if((string)(btnLogIn.Content) == "Log In")
            {
                try
                {
                    _user = _userManager.AuthenticateUser(txtUsername.Text, pwdPassword.Password);

                    btnLogIn.Content = "Log Out";
                    txtUsername.Text = "";

                    if(pwdPassword.Password == "newuser")
                    {
                        var updatePassword = new frmUpdatePassword(_user, _userManager, true);

                        if (!updatePassword.ShowDialog() == true)
                        {
                            resetWindow();
                            _user = null;
                            return;
                        }
                    }

                    pwdPassword.Password = "";
                    txtUsername.Visibility = Visibility.Hidden;
                    lblUsername.Visibility = Visibility.Hidden;
                    pwdPassword.Visibility = Visibility.Hidden;
                    lblPassword.Visibility = Visibility.Hidden;
                    sbarItemMessage.Content = "Don't Forget To Log Out When Finshed...";

                    mnuMain.IsEnabled = true;
                    showUserTabs();
                    lblGreeting.Content = "Welcome " + _user.FirstName + " " + _user.LastName;
                    if(_user.Roles.Count > 0)
                    {
                        var rolesString = _user.Roles[0];
                        for (int i = 1; i < _user.Roles.Count; i++)
                        {
                            if (i < _user.Roles.Count - 1)
                            {
                                rolesString += ", " + _user.Roles[i];
                            }
                            else if (_user.Roles.Count == 2 && i == _user.Roles.Count - 1)
                            {
                                rolesString += " and " + _user.Roles[i];
                            }
                            else
                            {
                                rolesString += ", and " + _user.Roles[i];
                            }
                        }

                        txtblRoles.Text = "Your are logged in as " + rolesString;
                    }
                    else
                    {
                        txtblRoles.Text = "You do not have any assigned roles. Contact the Park Staff Administrator";
                    }
                }
                catch (Exception ex)
                {

                    pwdPassword.Clear();
                    txtUsername.Clear();
                    txtUsername.Focus();
                    MessageBox.Show(ex.Message + "\n\n" + ex.InnerException.Message);
                }
            }
            else
            {
                _user = null;
                resetWindow();
            }
        }

        private void pwdPassword_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                btnLogIn_Click(sender, e);
            }
        }

        private void tabParkAdmin_GotFocus(object sender, RoutedEventArgs e)
        {
            if (((TabItem)sender).Visibility == Visibility.Visible)
            {
                try
                {
                    var workerManager = new WorkerManager();
                    if (dgUserList.ItemsSource == null)
                    {
                        dgUserList.ItemsSource = workerManager.RetrieveWorkersByActive();
                        dgUserList.Columns.Remove(dgUserList.Columns[0]);
                        dgUserList.Columns.Remove(dgUserList.Columns[0]);
                        dgUserList.Columns.Remove(dgUserList.Columns[0]);
                        dgUserList.Columns[0].Header = "Worker ID";
                        dgUserList.Columns[1].Header = "First Name";
                        dgUserList.Columns[2].Header = "Last Name";
                        dgUserList.Columns[3].Header = "Street Address";
                        dgUserList.Columns[4].Header = "ZIP Code";
                        dgUserList.Columns[5].Header = "Phone";
                        dgUserList.Columns[6].Header = "Email";
                        dgUserList.Columns.Remove(dgUserList.Columns[7]);
                        dgUserList.Columns[7].Header = "Start Date";
                        dgUserList.Columns.Remove(dgUserList.Columns[8]);
                        
                    }
                }
                catch (Exception ex)
                {

                    MessageBox.Show(ex.Message);
                }
            }
        }

        private void tabQuarterMaster_GotFocus(object sender, RoutedEventArgs e)
        {
            if (((TabItem)sender).Visibility == Visibility.Visible)
            {
                try
                {

                    var toolManager = new ToolManager();
                    var projectManager = new ProjectManager();
                    if(dgToolList.ItemsSource == null)
                    {
                        dgToolList.ItemsSource = toolManager.RetrieveAllTools();
                        dgToolList.Columns[0].Header = "Tool ID";
                        dgToolList.Columns[1].Header = "Tool Description";
                    }
                }
                catch(Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }

            }
        }

        private void tabMaintenance_GotFocus(object sender, RoutedEventArgs e)
        {
            if (((TabItem)sender).Visibility == Visibility.Visible)
            {
                try
                {
                    var toolManager = new ToolManager();
                    var projectManager = new ProjectManager();
                    if (dgProjectList.ItemsSource == null)
                    {
                        dgProjectList.ItemsSource = projectManager.RetrieveAllProjects();
                        dgProjectList.Columns[0].Header = "Project ID";
                        dgProjectList.Columns[1].Header = "Worker ID";
                        dgProjectList.Columns[2].Header = "Paid";
                        dgProjectList.Columns[3].Header = "Start Date";
                        dgProjectList.Columns[4].Header = "Project";
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }

            }
        }


        private void tabWorkload_GotFocus(object sender, RoutedEventArgs e)
        {
            if (((TabItem)sender).Visibility == Visibility.Visible)
            {
                try
                {
                    var workerManager = new WorkerManager();
                    var projectManager = new ProjectManager();
                    List<ProjectViewModel> allProjects = projectManager.RetrieveAllProjects();
                    List<ProjectViewModel> userProjects = new List<ProjectViewModel>();
                    foreach (var project in allProjects)
                    {
                        if (project.WorkerID == _user.UserID)
                        {
                            userProjects.Add(project);
                        }
                    }
                    if (dgTaskList.ItemsSource == null)
                    {
                        dgTaskList.ItemsSource = userProjects;
                        dgTaskList.Columns[0].Header = "Project ID";
                        dgTaskList.Columns[1].Header = "Worker ID";
                        dgTaskList.Columns[2].Header = "Paid";
                        dgTaskList.Columns[3].Header = "Start Date";
                        dgTaskList.Columns[4].Header = "Project";
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }

            }
        }

        private void hideAllTabs()
        {
            foreach (TabItem t in tabSetMain.Items)
            {
                t.Visibility = Visibility.Collapsed;
            }
            tabBlank.Visibility = Visibility.Visible;
            tabBlank.Focus();
        }

        private void showUserTabs()
        {
            foreach (var r in _user.Roles)
            {
                switch (r)
                {
                    case "Park Staff Admin":
                        tabParkAdmin.Visibility = Visibility.Visible;
                        tabParkAdmin.IsSelected = true;
                        break;
                    case "QuarterMaster":
                        tabQuarterMaster.Visibility = Visibility.Visible;
                        tabQuarterMaster.IsSelected = true;
                        break;
                    case "Maintenance Manager":
                        tabMaintenance.Visibility = Visibility.Visible;
                        tabMaintenance.IsSelected = true;
                        break;
                    default:
                        break;
                }
            }
            tabWorkload.Visibility = Visibility.Visible;
            tabBlank.Visibility = Visibility.Collapsed;
            bool isVisible = false;

            /*
             * while (!isVisible)
            {
                List<TabItem> tabs = new List<TabItem>()
                {
                    tabParkAdmin, tabQuarterMaster, tabMaintenance, tabWorkload
                };
                foreach (TabItem item in tabs)
                {
                    if (item.Visibility == Visibility.Visible)
                    {
                        item.IsSelected = true;
                        item.Focus();
                        isVisible = true;
                        break;
                    }
                }
            }
            */
        }

        public void resetWindow()
        {
            hideAllTabs();
            mnuMain.IsEnabled = false;
            txtUsername.Text = "";
            pwdPassword.Password = "";
            btnLogIn.Content = "Log In";
            lblGreeting.Content = "Greetings User";
            txtblRoles.Text = "You Are Not Logged In";
            txtUsername.Visibility = Visibility.Visible;
            lblUsername.Visibility = Visibility.Visible;
            pwdPassword.Visibility = Visibility.Visible;
            lblPassword.Visibility = Visibility.Visible;
            sbarItemMessage.Content = "Please Log In To Continue...";

            dgUserList.ItemsSource = null;
            dgProjectList.ItemsSource = null;
            dgTaskList.ItemsSource = null;
            dgToolList.ItemsSource = null;

            txtUsername.Focus();
        }

        private void mnuItemUpdatePassword_Click(object sender, RoutedEventArgs e)
        {
            if(_user != null)
            {
                var updatePassword = new frmUpdatePassword(_user, _userManager);
                updatePassword.ShowDialog();
            }
            else
            {
                MessageBox.Show("Please Log In First");
            }
        }
        private void editWorker(WorkerViewModel selectedItem)
        {
            var addEditWindow = new frmWorkerDetailAddEdit(selectedItem);
            addEditWindow.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            if (addEditWindow.ShowDialog() == true)
            {
                var workerManager = new WorkerManager();
                dgUserList.ItemsSource = workerManager.RetrieveWorkersByActive();
                dgUserList.Columns.Remove(dgUserList.Columns[0]);
                dgUserList.Columns.Remove(dgUserList.Columns[0]);
                dgUserList.Columns.Remove(dgUserList.Columns[0]);
                dgUserList.Columns[0].Header = "Worker ID";
                dgUserList.Columns[1].Header = "First Name";
                dgUserList.Columns[2].Header = "Last Name";
                dgUserList.Columns[3].Header = "Street Address";
                dgUserList.Columns[4].Header = "ZIP Code";
                dgUserList.Columns[5].Header = "Phone";
                dgUserList.Columns[6].Header = "Email";
                dgUserList.Columns.Remove(dgUserList.Columns[7]);
                dgUserList.Columns[7].Header = "Start Date";
                dgUserList.Columns.Remove(dgUserList.Columns[8]);
            }
        }
        private void mnuItemUpdateAvailability_Click(object sender, RoutedEventArgs e)
        {
            var selectedItem = _user.UserID;
            if (selectedItem.Equals(null))
            {
                MessageBox.Show("You Must Select A User To Edit First!",
                    "Invalid Operation", MessageBoxButton.OK,
                    MessageBoxImage.Information);
            }
            else
            {
                var updateAvailableWindow = new frmUpdateAvailability(selectedItem);
                updateAvailableWindow.WindowStartupLocation = WindowStartupLocation.CenterScreen;
                if (updateAvailableWindow.ShowDialog() == true)
                {
                    var workerManager = new WorkerManager();
                    dgUserList.ItemsSource = workerManager.RetrieveWorkersByActive((bool)chkShowActive.IsChecked);
                    dgUserList.Columns.Remove(dgUserList.Columns[0]);
                    dgUserList.Columns.Remove(dgUserList.Columns[0]);
                    dgUserList.Columns.Remove(dgUserList.Columns[0]);
                    dgUserList.Columns[0].Header = "Worker ID";
                    dgUserList.Columns[1].Header = "First Name";
                    dgUserList.Columns[2].Header = "Last Name";
                    dgUserList.Columns[3].Header = "Street Address";
                    dgUserList.Columns[4].Header = "ZIP Code";
                    dgUserList.Columns[5].Header = "Phone";
                    dgUserList.Columns[6].Header = "Email";
                    dgUserList.Columns.Remove(dgUserList.Columns[7]);
                    dgUserList.Columns[7].Header = "Start Date";
                    dgUserList.Columns.Remove(dgUserList.Columns[8]);
                }
            }
        }

        private void mnuItemEditProfile_Click(object sender, RoutedEventArgs e)
        {
            var selectedItem = _user.UserID;
            if (selectedItem.Equals(null))
            {
                MessageBox.Show("You Must Select A User To Edit First!",
                    "Invalid Operation", MessageBoxButton.OK,
                    MessageBoxImage.Information);
            }
            else
            {
                var addEditWindow = new frmWorkerDetailAddEdit(selectedItem);
                addEditWindow.WindowStartupLocation = WindowStartupLocation.CenterScreen;
                if (addEditWindow.ShowDialog() == true)
                {
                    var workerManager = new WorkerManager();
                    dgUserList.ItemsSource = workerManager.RetrieveWorkersByActive();
                    dgUserList.Columns.Remove(dgUserList.Columns[0]);
                    dgUserList.Columns.Remove(dgUserList.Columns[0]);
                    dgUserList.Columns.Remove(dgUserList.Columns[0]);
                    dgUserList.Columns[0].Header = "Worker ID";
                    dgUserList.Columns[1].Header = "First Name";
                    dgUserList.Columns[2].Header = "Last Name";
                    dgUserList.Columns[3].Header = "Street Address";
                    dgUserList.Columns[4].Header = "ZIP Code";
                    dgUserList.Columns[5].Header = "Phone";
                    dgUserList.Columns[6].Header = "Email";
                    dgUserList.Columns.Remove(dgUserList.Columns[7]);
                    dgUserList.Columns[7].Header = "Start Date";
                    dgUserList.Columns.Remove(dgUserList.Columns[8]);
                }
            }
        }
        private void chkShowActive_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var workerManager = new WorkerManager();
                dgUserList.ItemsSource = workerManager.RetrieveWorkersByActive((bool)chkShowActive.IsChecked);
                dgUserList.Columns.Remove(dgUserList.Columns[0]);
                dgUserList.Columns.Remove(dgUserList.Columns[0]);
                dgUserList.Columns.Remove(dgUserList.Columns[0]);
                dgUserList.Columns[0].Header = "Worker ID";
                dgUserList.Columns[1].Header = "First Name";
                dgUserList.Columns[2].Header = "Last Name";
                dgUserList.Columns[3].Header = "Street Address";
                dgUserList.Columns[4].Header = "ZIP Code";
                dgUserList.Columns[5].Header = "Phone";
                dgUserList.Columns[6].Header = "Email";
                dgUserList.Columns.Remove(dgUserList.Columns[7]);
                dgUserList.Columns[7].Header = "Start Date";
                if ((bool)chkShowActive.IsChecked)
                {
                    dgUserList.Columns.Remove(dgUserList.Columns[8]);
                }
                else
                {
                    dgUserList.Columns[8].Header = "End Date";
                }
                
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message + "\n\n" + ex.InnerException.Message);
            }
        }

        private void btnAddUser_Click(object sender, RoutedEventArgs e)
        {
            addWorker();
        }

        private void addWorker()
        {
            var addEditWindow = new frmWorkerDetailAddEdit();
            addEditWindow.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            if (addEditWindow.ShowDialog() == true)
            {
                var workerManager = new WorkerManager();
                dgUserList.ItemsSource = workerManager.RetrieveWorkersByActive((bool)chkShowActive.IsChecked);
                dgUserList.Columns.Remove(dgUserList.Columns[0]);
                dgUserList.Columns.Remove(dgUserList.Columns[0]);
                dgUserList.Columns.Remove(dgUserList.Columns[0]);
                dgUserList.Columns[0].Header = "Worker ID";
                dgUserList.Columns[1].Header = "Email";
                dgUserList.Columns[2].Header = "First Name";
                dgUserList.Columns[3].Header = "Last Name";
                dgUserList.Columns[4].Header = "Phone";
                dgUserList.Columns[5].Header = "Street Address";
                dgUserList.Columns[6].Header = "ZIP Code";
                dgUserList.Columns.Remove(dgUserList.Columns[7]);
                dgUserList.Columns[7].Header = "Start Date";
                dgUserList.Columns.Remove(dgUserList.Columns[8]);
                
            }
        }

        private void btnEditUser_Click(object sender, RoutedEventArgs e)
        {
            var selectedItem = (WorkerViewModel)dgUserList.SelectedItem;
            if (selectedItem == null)
            {
                MessageBox.Show("You Must Select A User To Edit First!",
                    "Invalid Operation", MessageBoxButton.OK, 
                    MessageBoxImage.Information);
            }
            else
            {
                editWorker(selectedItem);
            }
            
        }

        private void dgUserList_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            var selectedItem = (WorkerViewModel)dgUserList.SelectedItem;
            if (selectedItem == null)
            {
                addWorker();
            }
            else
            {
                editWorker(selectedItem);
            }
            
        }

        private void btnUpdateAvailability_Click(object sender, RoutedEventArgs e)
        {
            var selectedItem = (WorkerViewModel)dgUserList.SelectedItem;
            if (selectedItem == null)
            {
                MessageBox.Show("You Must Select A User To Edit First!",
                    "Invalid Operation", MessageBoxButton.OK,
                    MessageBoxImage.Information);
            }
            else
            {
                var updateAvailableWindow = new frmUpdateAvailability(selectedItem);
                updateAvailableWindow.WindowStartupLocation = WindowStartupLocation.CenterScreen;
                if (updateAvailableWindow.ShowDialog() == true)
                {
                    var workerManager = new WorkerManager();
                    dgUserList.ItemsSource = workerManager.RetrieveWorkersByActive((bool)chkShowActive.IsChecked);
                    dgUserList.Columns.Remove(dgUserList.Columns[0]);
                    dgUserList.Columns.Remove(dgUserList.Columns[0]);
                    dgUserList.Columns.Remove(dgUserList.Columns[0]);
                    dgUserList.Columns[0].Header = "Worker ID";
                    dgUserList.Columns[1].Header = "Email";
                    dgUserList.Columns[2].Header = "First Name";
                    dgUserList.Columns[3].Header = "Last Name";
                    dgUserList.Columns[4].Header = "Phone";
                    dgUserList.Columns[5].Header = "Street Address";
                    dgUserList.Columns[6].Header = "ZIP Code";
                    dgUserList.Columns.Remove(dgUserList.Columns[7]);
                    dgUserList.Columns[7].Header = "Start Date";
                    dgUserList.Columns.Remove(dgUserList.Columns[8]);
                }
            }
            
        }

        private void dgToolList_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            var selectedItem = (Tool)dgToolList.SelectedItem;
            if (selectedItem == null)
            {
                addTool();
            }
            else
            {
                editTool(selectedItem);
            }
        }

        private void editTool(Tool selectedItem)
        {
            var checkoutWindow = new frmToolAddEdit(selectedItem);
            checkoutWindow.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            if (checkoutWindow.ShowDialog() == true)
            {
                MessageBox.Show("Tool Successfully Checked Out/In");
                var toolManager = new ToolManager();
                var projectManager = new ProjectManager();
                if (dgToolList.ItemsSource == null)
                {
                    dgToolList.ItemsSource = toolManager.RetrieveAllTools();
                    dgToolList.Columns[0].Header = "Tool ID";
                    dgToolList.Columns[1].Header = "Tool Description";
                }
            }
        }
        private void addTool()
        {
            var newAddToolWindow = new frmAddTool();
            newAddToolWindow.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            if (newAddToolWindow.ShowDialog() == true)
            {
                MessageBox.Show("Tool Successfully Added");
                var toolManager = new ToolManager();
                var projectManager = new ProjectManager();
                if (dgToolList.ItemsSource == null)
                {
                    dgToolList.ItemsSource = toolManager.RetrieveAllTools();
                    dgToolList.Columns[0].Header = "Tool ID";
                    dgToolList.Columns[1].Header = "Tool Description";
                }
            }
        }

        private void btnAddProjectName_Click(object sender, RoutedEventArgs e)
        {
            var newProjectNameWindow = new frmAddProjectName();
            newProjectNameWindow.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            if (newProjectNameWindow.ShowDialog() == true)
            {
                MessageBox.Show("Project Type Successfully Added");
            }
        }

        private void btnEditProject_Click(object sender, RoutedEventArgs e)
        {
            var selectedItem = (ProjectViewModel)dgProjectList.SelectedItem;
            if (selectedItem == null)
            {
                MessageBox.Show("You Must Select A Task First!", "Invalid Operation", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            else
            {
                editProject(selectedItem);

            }
        }
        private void editProject(ProjectViewModel project)
        {
            var newViewTaskWindow = new frmProjectDetailAddEdit(project, true);
            newViewTaskWindow.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            if (newViewTaskWindow.ShowDialog() == true)
            {
                MessageBox.Show("Project Edit Complete.");
                var toolManager = new ToolManager();
                var projectManager = new ProjectManager();
                if (dgProjectList.ItemsSource == null)
                {
                    dgProjectList.ItemsSource = projectManager.RetrieveAllProjects();
                    dgProjectList.Columns[0].Header = "Project ID";
                    dgProjectList.Columns[1].Header = "Worker ID";
                    dgProjectList.Columns[2].Header = "Paid";
                    dgProjectList.Columns[3].Header = "Start Date";
                    dgProjectList.Columns[4].Header = "Project";
                }
            }
        }

        private void btnAddProject_Click(object sender, RoutedEventArgs e)
        {
            addNewProject();
        }

        private void addNewProject()
        {
            var newViewTaskWindow = new frmProjectDetailAddEdit();
            newViewTaskWindow.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            if (newViewTaskWindow.ShowDialog() == true)
            {
                MessageBox.Show("Project Started.");
                var toolManager = new ToolManager();
                var projectManager = new ProjectManager();
                if (dgProjectList.ItemsSource == null)
                {
                    dgProjectList.ItemsSource = projectManager.RetrieveAllProjects();
                    dgProjectList.Columns[0].Header = "Project ID";
                    dgProjectList.Columns[1].Header = "Worker ID";
                    dgProjectList.Columns[2].Header = "Paid";
                    dgProjectList.Columns[3].Header = "Start Date";
                    dgProjectList.Columns[4].Header = "Project";
                }
            }
        }

        private void btnFinishProject_Click(object sender, RoutedEventArgs e)
        {
            var selectedItem = (ProjectViewModel)dgProjectList.SelectedItem;
            var projectManager = new ProjectManager();
            if (selectedItem == null)
            {
                MessageBox.Show("You Must Select A Project First!",
                    "Invalid Operation", MessageBoxButton.OK,
                    MessageBoxImage.Information);
            }
            MessageBoxResult result = MessageBox.Show("Are You Sure It's Finished?", "Complete Project Confirmation", MessageBoxButton.OKCancel);
            switch (result)
            {
                case MessageBoxResult.OK:
                    projectManager.DeactivateProject(selectedItem);
                    MessageBox.Show("Project Marked as Finished.");
                    break;
                case MessageBoxResult.Cancel:
                    MessageBox.Show("Canceling Request.");
                    break;
                default:
                    break;
            }

        }

        private void btnGetTaskDetail_Click(object sender, RoutedEventArgs e)
        {
            var selectedItem = (ProjectViewModel)dgTaskList.SelectedItem;
            if (selectedItem == null)
            {
                MessageBox.Show("You Must Select A Task First!", "Invalid Operation", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            else
            {
                viewProject(selectedItem);

            }
        }

        private void btnMarkTaskComplete_Click(object sender, RoutedEventArgs e)
        {
            var selectedItem = (ProjectViewModel)dgTaskList.SelectedItem;
            var projectManager = new ProjectManager();
            if (selectedItem == null)
            {
                MessageBox.Show("You Must Select A Project First!",
                    "Invalid Operation", MessageBoxButton.OK,
                    MessageBoxImage.Information);
            }
            MessageBoxResult result = MessageBox.Show("Are You Sure It's Finished?", "Complete Project Confirmation", MessageBoxButton.OKCancel);
            switch (result)
            {
                case MessageBoxResult.OK:
                    projectManager.DeactivateProject(selectedItem);
                    MessageBox.Show("Project Marked as Finished.");
                    break;
                case MessageBoxResult.Cancel:
                    MessageBox.Show("Canceling Request.");
                    break;
                default:
                    break;
            }
        }

        private void btnEditTool_Click(object sender, RoutedEventArgs e)
        {
            var selectedItem = (Tool)dgToolList.SelectedItem;
            if (selectedItem == null)
            {
                MessageBox.Show("You Must Select A Tool First!", "Invalid Operation", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            else
            {
                editTool(selectedItem);
            }
            
        }

        private void btnAddTool_Click(object sender, RoutedEventArgs e)
        {
            addTool();
        }

        private void dgTaskList_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            var selectedItem = (ProjectViewModel)dgTaskList.SelectedItem;
            if (selectedItem == null)
            {
                MessageBox.Show("You Must Select A Task First!", "Invalid Operation", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            else
            {
                viewProject(selectedItem);
            }
            
        }

        private void viewProject(ProjectViewModel project)
        {
            var newViewTaskWindow = new frmProjectDetailAddEdit(project, false);
            newViewTaskWindow.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            if (newViewTaskWindow.ShowDialog() == true)
            {
                MessageBox.Show("Get to Work!");
            }
        }
    }
}
