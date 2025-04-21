using System;
using CommunityToolkit.Mvvm.ComponentModel;

namespace EcoSentinel;

public class User
{
    public string username;
    private string password;
    public string role;

    public User()
    {
        username = "Admin";
        password = "EcoSentinel25";
        role = "Admin";
    }

    public bool LoginAuthenticated(string u, string p)
    {
        if(u == this.username && p == this.password)
        {
            return true;
        }
        else 
        {
            return false;
        }
    }

}