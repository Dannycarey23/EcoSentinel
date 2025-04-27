using EcoSentinel.View;
using EcoSentinel.Model;
using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace EcoSentinel.ViewModel
{
    public partial class AdministrationPageViewModel : ObservableObject
    {
        private readonly DatabaseService db;
        private ObservableCollection<User> _userList;

        public ObservableCollection<User> UserList
        {
            get => _userList;
            set
            {
                _userList = value;
                OnPropertyChanged(nameof(UserList));
            }
        }

        public AdministrationPageViewModel()
        {
            _userList = new ObservableCollection<User>();
            db = new DatabaseService();
            PopulateUserList();
        }

        [RelayCommand]
        public async Task PopulateUserList()
        {
            var dataList = db.PopulateUserData();
            UserList = new ObservableCollection<User>(dataList);
        }

        [RelayCommand]
        public async Task AddUser()
        {
            var popup = new AddUserForm();
            var viewModel = new AddUserFormViewModel();
            viewModel.OnSubmit += async (userInput) =>
            {
                db.AddUserData(userInput.username, userInput.password, userInput.role, userInput.email, userInput.fname, userInput.lname);
                await PopulateUserList();
                await Shell.Current.GoToAsync("//AdministrationPage"); // Navigate to AdministrationPage using Shell
            };
            popup.BindingContext = viewModel;
            await Application.Current.MainPage.Navigation.PushAsync(popup);
        }

        [RelayCommand]
        public async Task DeleteUser()
        {
            var popup = new DeleteUserForm();
            var viewModel = new DeleteUserFormViewModel();
            viewModel.OnSubmit += async (userInput) =>
            {
                db.DeleteUserData(userInput.username);
                await PopulateUserList();
                await Shell.Current.GoToAsync("//AdministrationPage"); // Navigate to AdministrationPage using Shell
            };
            popup.BindingContext = viewModel;
            await Application.Current.MainPage.Navigation.PushAsync(popup);
        }

        [RelayCommand]
        public async Task PasswordReset()
        {
            var popup = new ResetPasswordForm();
            var viewModel = new ResetPasswordFormViewModel();
            viewModel.OnSubmit += async (userInput) =>
            {
                db.SetPasswordData(userInput.username, userInput.password);
                await PopulateUserList();
                await Shell.Current.GoToAsync("//AdministrationPage"); // Navigate to AdministrationPage using Shell
            };
            popup.BindingContext = viewModel;
            await Application.Current.MainPage.Navigation.PushAsync(popup);
            
        }
    }
}
