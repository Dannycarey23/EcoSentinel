using System;
using CommunityToolkit.Mvvm.ComponentModel;
using EcoSentinel.Model;

namespace EcoSentinel;

public class User
{
    public DatabaseService db;
    public string username { get; set; }
    public string password { get; set; }
    public string role { get; set; }
    public string email { get; set; }
    public string fname { get; set; }
    public string lname { get; set; }

    public User()
    {
        db = new DatabaseService();
    }

    public bool LoginAuthenticated(string u, string p)
    {
        bool valid = false;
            
        foreach (var item in db.PopulateUserData())
        { 
            if(u == item.username && p == item.password)
            {
                valid = true;
            }                
        }
        return valid;
    }

}