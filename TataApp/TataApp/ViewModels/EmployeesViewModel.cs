namespace TataApp.ViewModels
{
    using GalaSoft.MvvmLight.Command;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.ComponentModel;
    using System.Linq;
    using System.Threading.Tasks;
    using System.Windows.Input;
    using TataApp.Models;
    using TataApp.Services;
    using Xamarin.Forms;
    public class EmployeesViewModel : INotifyPropertyChanged
    {
        #region Events
        public event PropertyChangedEventHandler PropertyChanged;
        #endregion

        #region Attributes
        ApiService apiService;
        DialogService dialogService;
        bool isRefreshing;
        string filter;
        List<Employee> employees;
        Employee me;
        #endregion

        #region Properties
        public ObservableCollection<EmployeeItemViewModel> MyEmployees
        {
            get;
            set;
        }

        public bool IsRefreshing
        {
            set
            {
                if (isRefreshing != value)
                {
                    isRefreshing = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("IsRefreshing"));
                }
            }
            get
            {
                return isRefreshing;
            }
        }

        public string Filter
        {
            set
            {
                if (filter != value)
                {
                    filter = value;
                    if (string.IsNullOrEmpty(filter))
                    {
                        ReloadEmployees();
                    }
                    else
                    {
                        Search();
                    }

                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Filter"));
                }
            }
            get
            {
                return filter;
            }
        }
        #endregion

        #region Constructors
        public EmployeesViewModel()
        {
            instance = this;

            apiService = new ApiService();
            dialogService = new DialogService();

            var mainViewModel = MainViewModel.GetInstance();
            me = mainViewModel.Employee;

            MyEmployees = new ObservableCollection<EmployeeItemViewModel>();
        }
        #endregion

        #region Singleton
        static EmployeesViewModel instance;

        public static EmployeesViewModel GetInstance()
        {
            if (instance == null)
            {
                instance = new EmployeesViewModel();
            }

            return instance;
        }
        #endregion

        #region Methods
        async Task LoadEmployees()
        {
            IsRefreshing = true;

            var checkConnetion = await apiService.CheckConnection();
            if (!checkConnetion.IsSuccess)
            {
                IsRefreshing = false;
                await dialogService.ShowMessage("Error", checkConnetion.Message);
                return;
            }

            var urlAPI = Application.Current.Resources["URLAPI"].ToString();
            var response = await apiService.GetList<Employee>(
                urlAPI,
                "/api",
                "/Employees",
                me.TokenType,
                me.AccessToken);

            if (!response.IsSuccess)
            {
                IsRefreshing = false;
                await dialogService.ShowMessage("Error", response.Message);
                return;
            }

            employees = (List<Employee>)response.Result;
            ReloadEmployees();
            IsRefreshing = false;
        }

        void ReloadEmployees()
        {
            MyEmployees.Clear();
            foreach (var employee in employees
                     .OrderBy(e => e.FirstName)
                     .ThenBy(e => e.LastName))
            {
                if (employee.EmployeeId != me.EmployeeId)
                {
                    MyEmployees.Add(new EmployeeItemViewModel
                    {
                        Address = employee.Address,
                        Document = employee.Document,
                        DocumentTypeId = employee.DocumentTypeId,
                        Email = employee.Email,
                        EmployeeCode = employee.EmployeeCode,
                        EmployeeId = employee.EmployeeId,
                        FirstName = employee.FirstName,
                        LastName = employee.LastName,
                        LoginTypeId = employee.LoginTypeId,
                        Phone = employee.Phone,
                        Picture = employee.Picture,
                    });
                }
            }
        }
        #endregion

        #region Commands
        public ICommand RefreshCommand
        {
            get { return new RelayCommand(Refresh); }
        }

        public async void Refresh()
        {
            await LoadEmployees();
        }

        public ICommand SearchCommand
        {
            get { return new RelayCommand(Search); }
        }

        void Search()
        {
            MyEmployees.Clear();
            foreach (var employee in employees
                     .Where(e => e.FirstName.ToLower().Contains(Filter.ToLower()) ||
                                 e.LastName.ToLower().Contains(Filter.ToLower()))
                     .OrderBy(e => e.FirstName)
                     .ThenBy(e => e.LastName))
            {
                if (employee.EmployeeId != me.EmployeeId)
                {
                    MyEmployees.Add(new EmployeeItemViewModel
                    {
                        Address = employee.Address,
                        Document = employee.Document,
                        DocumentTypeId = employee.DocumentTypeId,
                        Email = employee.Email,
                        EmployeeCode = employee.EmployeeCode,
                        EmployeeId = employee.EmployeeId,
                        FirstName = employee.FirstName,
                        LastName = employee.LastName,
                        LoginTypeId = employee.LoginTypeId,
                        Phone = employee.Phone,
                        Picture = employee.Picture,
                    });
                }
            }
        }
        #endregion
    }
}
