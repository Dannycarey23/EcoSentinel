using System;
using CommunityToolkit.Mvvm.ComponentModel;
using EcoSentinel.Model;

namespace EcoSentinel;

public class User
{
    public DatabaseService db;
    public string username;
    public string password;
    public string role;
    public string email;
    public string fname;
    public string lname;

    public User()
    {
        db = new DatabaseService();
    }

    public bool LoginAuthenticated(string u, string p)
    {
        bool valid = false;
        foreach (var item in db.PopulateUserData())
        { 
            if(valid == false)
            {
                if(u == item.username && p == item.password)
                {
                    valid = true;
                }
                else
                {
                    valid = false;
                }
            }
        }

        return valid;

    }

}