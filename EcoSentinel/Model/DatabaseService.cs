using System;
using System.Collections;
using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using Microsoft.Data.SqlClient;

namespace EcoSentinel.Model;

public class DatabaseService
{
    public const string ConnectionString = "Server=tcp:eco-sentinel-v1.database.windows.net,1433;Initial Catalog=EcoSentinel_db;Persist Security Info=False;User ID=es_admin;Password=EcoSentinel25;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;";

    public IEnumerable<User> PopulateUserData()
    {
        var items = new ObservableCollection<User>();
        using (SqlConnection conn = new SqlConnection(ConnectionString))
        {
            conn.Open();
            using (SqlCommand cmd = new SqlCommand("SELECT username, password, role, email, fname, lname FROM dbo.users", conn))
            {
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while(reader.Read())
                    {
                        items.Add(new User
                        {
                            username = reader["username"].ToString(),
                            password = reader["password"].ToString(),
                            role = reader["role"].ToString(),
                            email = reader["email"].ToString(),
                            fname = reader["fname"].ToString(),
                            lname = reader["lname"].ToString()
                        });
                    }
                }
            }

            return items;
        }
    }

    public IEnumerable<SensorModel> PopulateSensorData()
    {
        var items = new ObservableCollection<SensorModel>();
        using (SqlConnection con = new SqlConnection(ConnectionString))
        {
            con.Open();
            using (SqlCommand cmd = new SqlCommand("SELECT sensorId, sensorType, sensorStatus, latitude, longitude, siteName, siteType FROM dbo.sensors", con))
            {
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        items.Add(new SensorModel
                        {
                            sensorId = (int)reader["sensorId"],
                            sensorType = (string)reader["sensorType"],
                            sensorStatus = (string)reader["sensorStatus"],
                            latitude = Convert.ToDouble(reader["latitude"]),
                            longitude = Convert.ToDouble(reader["longitude"]),
                            siteName = (string)reader["siteName"],
                            siteType = (string)reader["siteType"]
                        });
                    }
                }
                
            } return items;
        }

    }
}