using System;
using CommunityToolkit.Mvvm.ComponentModel;
using EcoSentinel.Model;

namespace EcoSentinel.ViewModel;

public partial class FooterPageViewModel : ObservableObject
{
    private readonly UserService _userService;
    [ObservableProperty]
    public string currentUsername; 

    public FooterPageViewModel(UserService userService)
    {
        _userService= userService ?? throw new ArgumentNullException(nameof(userService));
        currentUsername = _userService.username; 
    }
}
