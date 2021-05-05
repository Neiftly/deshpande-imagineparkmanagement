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
using System.Windows.Shapes;

namespace WpfPresentation
{
    /// <summary>
    /// Interaction logic for frmUpdateAvailability.xaml
    /// </summary>
    public partial class frmUpdateAvailability : Window
    {
        WorkerViewModel _worker;
        private IWorkerManager _workerManager = new WorkerManager();
        private List<int> _oldAvailability;
        private List<int> _newAvailability = new List<int>();
        public frmUpdateAvailability(WorkerViewModel worker)
        {
            _worker = worker;
            _oldAvailability = _workerManager.RetrieveAvailabilityByID(_worker.WorkerID);
            _newAvailability.Add(0);
            _newAvailability.Add(0);
            _newAvailability.Add(0);
            _newAvailability.Add(0);
            _newAvailability.Add(0);
            _newAvailability.Add(0);
            _newAvailability.Add(0);
            InitializeComponent();
        }
        public frmUpdateAvailability(int workerID)
        {
            _oldAvailability = _workerManager.RetrieveAvailabilityByID(workerID);
            _newAvailability.Add(0);
            _newAvailability.Add(0);
            _newAvailability.Add(0);
            _newAvailability.Add(0);
            _newAvailability.Add(0);
            _newAvailability.Add(0);
            _newAvailability.Add(0);
            InitializeComponent();
        }

        private void btnEditSave_Click(object sender, RoutedEventArgs e)
        {
            if ((string)(btnEditSave.Content) == "Edit")
            {
                setupEdit();
            }
            else
            {
                _newAvailability[0] = getSunday();
                _newAvailability[1] = getMonday();
                _newAvailability[2] = getTuesday();
                _newAvailability[3] = getWednesday();
                _newAvailability[4] = getThursday();
                _newAvailability[5] = getFriday();
                _newAvailability[6] = getSaturday();
                try
                {
                    _workerManager.UpdateWorkerAvailability(_worker.WorkerID, _oldAvailability, _newAvailability);
                    this.DialogResult = true;
                }
                catch (Exception ex)
                {
                    resetAvailability();
                    MessageBox.Show(ex.Message + "\n\n" + ex.InnerException.Message);
                }
            }
        }

        private void setupEdit()
        {
            btnEditSave.Content = "Save";
            enableAllRadioButtons();
        }

        private void enableAllRadioButtons()
        {
            radSun0.IsEnabled = true;
            radSun1.IsEnabled = true;
            radSun2.IsEnabled = true;
            radSun3.IsEnabled = true;
            radMon0.IsEnabled = true;
            radMon1.IsEnabled = true;
            radMon2.IsEnabled = true;
            radMon3.IsEnabled = true;
            radTues0.IsEnabled = true;
            radTues1.IsEnabled = true;
            radTues2.IsEnabled = true;
            radTues3.IsEnabled = true;
            radWed0.IsEnabled = true;
            radWed1.IsEnabled = true;
            radWed2.IsEnabled = true;
            radWed3.IsEnabled = true;
            radThurs0.IsEnabled = true;
            radThurs1.IsEnabled = true;
            radThurs2.IsEnabled = true;
            radThurs3.IsEnabled = true;
            radFri0.IsEnabled = true;
            radFri1.IsEnabled = true;
            radFri2.IsEnabled = true;
            radFri3.IsEnabled = true;
            radSat0.IsEnabled = true;
            radSat1.IsEnabled = true;
            radSat2.IsEnabled = true;
            radSat3.IsEnabled = true;
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
        }

        private void setSunday(int avail)
        {
            //string controlName = "radSun0";
            string controlName = "radSun" + avail;
            RadioButton radbtn = (RadioButton)this.FindName(controlName);
            radbtn.IsChecked = true;
        }

        private void setMonday(int avail)
        {
            //string controlName = "radMon0";
            string controlName = "radMon" + avail;
            RadioButton radbtn = (RadioButton)this.FindName(controlName);
            radbtn.IsChecked = true;
        }

        private void setTuesday(int avail)
        {
            // string controlName = "radTues0";
            string controlName = "radTues" + avail;
            RadioButton radbtn = (RadioButton)this.FindName(controlName);
            radbtn.IsChecked = true;
        }
        private void setWednesday(int avail)
        {
            // string controlName = "radWed0";
            string controlName = "radWed" + avail;
            RadioButton radbtn = (RadioButton)this.FindName(controlName);
            radbtn.IsChecked = true;
        }
        private void setThursday(int avail)
        {
            // string controlName = "radThurs0";
            string controlName = "radThurs" + avail;
            RadioButton radbtn = (RadioButton)this.FindName(controlName);
            radbtn.IsChecked = true;
        }
        private void setFriday(int avail)
        {
            // string controlName = "radFri0";
            string controlName = "radFri" + avail;
            RadioButton radbtn = (RadioButton)this.FindName(controlName);
            radbtn.IsChecked = true;
        }
        private void setSaturday(int avail)
        {
            // string controlName = "radSat0";
            string controlName = "radSat" + avail;
            RadioButton radbtn = (RadioButton)this.FindName(controlName);
            radbtn.IsChecked = true;
        }
        private int getSunday()
        {
            int result = 0;
            if ((bool)radSun1.IsChecked)
            {
                result = 1;
            }
            else if ((bool)radSun2.IsChecked)
            {
                result = 2;
            }
            else if ((bool)radSun3.IsChecked)
            {
                result = 3;
            }
            return result;
        }
        private int getMonday()
        {
            int result = 0;
            if ((bool)radMon1.IsChecked)
            {
                result = 1;
            }
            else if ((bool)radMon2.IsChecked)
            {
                result = 2;
            }
            else if ((bool)radMon3.IsChecked)
            {
                result = 3;
            }
            return result;
        }
        private int getTuesday()
        {
            int result = 0;
            if ((bool)radTues1.IsChecked)
            {
                result = 1;
            }
            else if ((bool)radTues2.IsChecked)
            {
                result = 2;
            }
            else if ((bool)radTues3.IsChecked)
            {
                result = 3;
            }
            return result;
        }
        private int getWednesday()
        {
            int result = 0;
            if ((bool)radWed1.IsChecked)
            {
                result = 1;
            }
            else if ((bool)radWed2.IsChecked)
            {
                result = 2;
            }
            else if ((bool)radWed3.IsChecked)
            {
                result = 3;
            }
            return result;
        }
        private int getThursday()
        {
            int result = 0;
            if ((bool)radThurs1.IsChecked)
            {
                result = 1;
            }
            else if ((bool)radThurs2.IsChecked)
            {
                result = 2;
            }
            else if ((bool)radThurs3.IsChecked)
            {
                result = 3;
            }
            return result;
        }
        private int getFriday()
        {
            int result = 0;
            if ((bool)radFri1.IsChecked)
            {
                result = 1;
            }
            else if ((bool)radFri2.IsChecked)
            {
                result = 2;
            }
            else if ((bool)radFri3.IsChecked)
            {
                result = 3;
            }
            return result;
        }
        private int getSaturday()
        {
            int result = 0;
            if ((bool)radSat1.IsChecked)
            {
                result = 1;
            }
            else if ((bool)radSat2.IsChecked)
            {
                result = 2;
            }
            else if ((bool)radSat3.IsChecked)
            {
                result = 3;
            }
            return result;
        }
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            if (_worker == null)
            {
                tbkTitle.Text = "Your Availability";
            }
            else
            {
                tbkTitle.Text = "Availability for " + _worker.FirstName + " " + _worker.LastName;
            }
            
            resetAvailability();
        }

        private void resetAvailability()
        {
            setSunday(_oldAvailability[0]);
            setMonday(_oldAvailability[1]);
            setTuesday(_oldAvailability[2]);
            setWednesday(_oldAvailability[3]);
            setThursday(_oldAvailability[4]);
            setFriday(_oldAvailability[5]);
            setSaturday(_oldAvailability[6]);
        }
    }
}
