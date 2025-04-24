using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Threading.Tasks;
using System.Windows.Input;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using EcoSentinel.Model;
using EcoSentinel.View;

namespace EcoSentinel.ViewModel;

public partial class AdministrationPageViewModel : ObservableObject
{
    private ObservableCollection<User> _userList;
    private readonly DatabaseService db;
    private readonly UserService _userService;

    public AdministrationPageViewModel(UserService userService)
    {
        _userService = userService;
        db = new DatabaseService();
        PopulateUserList();
    }

    public ObservableCollection<User> UserList
    {
        get => _userList;
        set => SetProperty(ref _userList, value);
    }

    [RelayCommand]
    public async Task PopulateUserList()
    {
        UserList = (ObservableCollection<User>)db.PopulateUserData();
    }

    // [RelayCommand]
    // public async Task AddUser()
    // {
    //     string u="newUser";
    //     string p="NewPassword";
    //     string r="EnvSci";
    //     string em="newUser@email.com";
    //     string fn="New";
    //     string ln = "User";
    //     db.AddUserData(u,p,r,em,fn,ln);
    // }

    // [RelayCommand]
    // public async Task DeleteUser()
    // {
    //     string u = "newUser";
    //     db.DeleteUserData(u);
    // }
    
    // [RelayCommand]
    // public async Task PasswordReset()
    // {
    //     string u = "newUser";
    //     string p = "newerPassword";
    //     db.SetPasswordData(u,p);
    // }


}
