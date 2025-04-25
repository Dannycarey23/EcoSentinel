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

public partial class AdministrationPageViewModel : INotifyPropertyChanged
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


    public event PropertyChangedEventHandler? PropertyChanged;
    protected virtual void OnPropertyChanged(string propertyName)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }

    [RelayCommand]
    public async Task AddUser()
    {
        string u, p, r, em, fn,ln;
        u="newUser";
        p="newUserPassword";
        r="OpsMan";
        em="newuser@email.com";
        fn="new";
        ln="user";
        db.AddUserData(u,p,r,em,fn,ln);
        db.PopulateUserData();
    }

    [RelayCommand]
    public async Task DeleteUser()
    {
        string u = "newUser";
        db.DeleteUserData(u);
        db.PopulateUserData();
    }

    [RelayCommand]
    public async Task PasswordReset()
    {
        string u ="Admin";
        string p ="EcoSentinel25";
        db.SetPasswordData(u,p);
    }
}

