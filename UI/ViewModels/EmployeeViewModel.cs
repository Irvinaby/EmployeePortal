using Caliburn.Micro;
using EmployeePortal.ApiHandler;
using EmployeePortal.ApiHandler.Model;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using UI.Common;
using UI.Utils;

namespace UI.ViewModels
{
    public class EmployeeViewModel : Screen
    {
        #region Private members

        private bool m_IsWorking;

        private bool m_HasDisplayedResults;

        private string m_Status;

        private IEmployeeHandler m_EmployeeHandler;

        private ICsvReporter m_CsvReporter;

        private BindableCollection<Employee> m_Employees;

        private Employee m_SelectedEmployee;

        private Employee m_EmployeeToSearch;

        private uint m_PageNumber;

        private uint m_TotalPageNumber;

        private ICommand m_AddCommand;

        private ICommand m_SearchCommand;

        private ICommand m_EditCommand;

        private ICommand m_DeleteCommand;

        #endregion

        #region Properties

        public bool HasDisplayedResult
        {
            get => m_HasDisplayedResults;
            set
            {
                m_HasDisplayedResults = value;
                NotifyOfPropertyChange(() => HasDisplayedResult);
            }
        }

        public bool IsWorking
        {
            get => m_IsWorking;
            set
            {
                m_IsWorking = value;
                NotifyOfPropertyChange(() => IsWorking);
            }
        }

        public string Status
        {
            get => m_Status;
            set
            {
                m_Status = value;
                NotifyOfPropertyChange(() => Status);
            }
        }

        public uint CurrentPage
        {
            get => m_PageNumber;
            set
            {
                m_PageNumber = value;
                NotifyOfPropertyChange(() => CurrentPage);
            }
        }

        public uint TotalPageNumber
        {
            get => m_TotalPageNumber;
            set
            {
                m_TotalPageNumber = value;
                NotifyOfPropertyChange(() => TotalPageNumber);
            }
        }

        public Employee SelectedEmployee
        {
            get => m_SelectedEmployee;
            set
            {
                m_SelectedEmployee = value;
                NotifyOfPropertyChange(() => SelectedEmployee);
            }
        }

        public Employee EmployeeToSearch
        {
            get => m_EmployeeToSearch;
            set
            {
                m_EmployeeToSearch = value;
                NotifyOfPropertyChange(() => EmployeeToSearch);
            }
        }

        public BindableCollection<Employee> Employees
        {
            get => m_Employees;
            set
            {
                m_Employees = value;
                HasDisplayedResult = m_Employees.Any();
                NotifyOfPropertyChange(() => Employees);
            }
        }

        public ICommand AddCommand
        {
            get
            {
                if (m_AddCommand == null)
                {
                    m_AddCommand = new RelayCommand(
                        param => AddEmployee()
                    );
                }
                return m_AddCommand;
            }
        }

        public ICommand SearchCommand
        {
            get
            {
                if (m_SearchCommand == null)
                {
                    m_SearchCommand = new RelayCommand(
                        param => SearchEmployee()
                    );
                }
                return m_SearchCommand;
            }
        }

        public ICommand EditCommand
        {
            get
            {
                if (m_EditCommand == null)
                {
                    m_EditCommand = new RelayCommand(
                        param => EditEmployee()
                    );
                }
                return m_EditCommand;
            }
        }

        public ICommand DeleteCommand
        {
            get
            {
                if (m_DeleteCommand == null)
                {
                    m_DeleteCommand = new RelayCommand(
                        param => DeleteEmployee()
                    );
                }
                return m_DeleteCommand;
            }
        }

        #endregion

        #region Constructor

        public EmployeeViewModel()
        {
            CurrentPage = 1; //Page number starts at 1
            ApiHandler.InitializeClient();
            m_EmployeeHandler = new EmployeeHandler();
            m_CsvReporter = new CsvReporter();
            EmployeeToSearch = new Employee();
            LoadEmployeeData();
        }

        //Ctor with injection for testing
        public EmployeeViewModel(IEmployeeHandler handler, ICsvReporter reporter)
        {
            m_EmployeeHandler = handler;
            m_CsvReporter = reporter;
        }

        #endregion

        #region Actions
        
        public async void EditEmployee()
        {
            UpdateStatus("Working..", true);
            var updateResult = await m_EmployeeHandler.UpdateEmployee(SelectedEmployee);
            UpdateStatus(updateResult, false);
        }

        public async void DeleteEmployee()
        {
            UpdateStatus("Working..", true);
            var deleteResult = await m_EmployeeHandler.DeleteEmployee(SelectedEmployee.Id);
            UpdateStatus(deleteResult, false);
        }

        public async void AddEmployee()
        {
            UpdateStatus("Adding..", true);
            var createResult = await m_EmployeeHandler.CreateEmployee(SelectedEmployee);
            UpdateStatus(createResult, false);
        }

        public async void SearchEmployee()
        {
            UpdateStatus("Loading..", true);
            var employees = await m_EmployeeHandler.SearchEmployees(EmployeeToSearch);
            Employees = new BindableCollection<Employee>(employees);
            UpdateStatus($"Search yielded {Employees.Count} results.", false);
        }

        public async Task LoadEmployeeData()
        {
            UpdateStatus("Loading..", true);
            var (totalPageNumber, employees) = await m_EmployeeHandler.ListEmployees(m_PageNumber);
            TotalPageNumber = (uint)totalPageNumber;
            Employees = new BindableCollection<Employee>(employees);
            UpdateStatus($"{Employees.Count} employees loaded", false);
        }

        public async Task LoadPreviousPage()
        {
            if(CurrentPage > 1)
            {
                CurrentPage--;
                await LoadEmployeeData();
            }
        }

        public async Task LoadNextPage()
        {
            CurrentPage++;
            await LoadEmployeeData();
        }

        public void ExportToCsv()
        {
            Status = m_CsvReporter.ToCsv(",", Employees);
        }

        #endregion

        #region Private methods

        private void UpdateStatus(string statusMessage, bool isWorking)
        {
            IsWorking = isWorking;
            Status = statusMessage;
        }

        #endregion

    }
}
