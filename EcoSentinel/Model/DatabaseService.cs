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

    public IEnumerable<AirDataModel> PopulateAirData()
    {
        var items = new ObservableCollection<AirDataModel>();
        using (SqlConnection con = new SqlConnection(ConnectionString))
        {
            con.Open();
            using (SqlCommand cmd = new SqlCommand("SELECT dataID, sensorID, sensorType, zone, agglomeration, localAuthority, date, time, nitrogenDioxide, sulfurDioxide, pmTwoPointFive, pmTen FROM dbo.airdata", con))
            {
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        items.Add(new AirDataModel
                        {
                            dataID = (int)reader["dataID"],
                            sensorID = (int)reader["sensorID"],
                            sensorType =(string)reader["sensorType"],
                            zone = (string)reader["zone"],
                            agglomeration = (string)reader["agglomeration"],
                            localAuthority = (string)reader["localAuthority"],
                            date = DateOnly.FromDateTime((DateTime)reader["date"]),
                            time = (TimeSpan)reader["time"],
                            nitrogenDioxide = reader["nitrogenDioxide"] is DBNull ? 0.0f : Convert.ToSingle(reader["nitrogenDioxide"]),
                            sulfurDioxide = reader["sulfurDioxide"] is DBNull ? 0.0f : Convert.ToSingle(reader["sulfurDioxide"]),
                            pmTwoPointFive = reader["pmTwoPointFive"] is DBNull ? 0.0f : Convert.ToSingle(reader["pmTwoPointFive"]),
                            pmTen = reader["pmTen"] is DBNull ? 0.0f : Convert.ToSingle(reader["pmTen"])
                        });
                    }
                }

            }
            return items;
        }
    }

    public IEnumerable<WaterDataModel> PopulateWaterData()
    {
        var items = new ObservableCollection<WaterDataModel>();
        using (SqlConnection con = new SqlConnection(ConnectionString))
        {
            con.Open();
            using (SqlCommand cmd = new SqlCommand("SELECT dataID, sensorID, sensorType, date, time, nitrateMgl1, nitrateLessThanMgL1, phosphateMgl1, ecCfu100ml FROM dbo.waterdata", con))
            {
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        items.Add(new WaterDataModel
                        {
                            dataID = (int)reader["dataID"],
                            sensorID = (int)reader["sensorID"],
                            sensorType = (string)reader["sensorType"],
                            date = (DateOnly)reader["date"],
                            time = (TimeSpan)reader["time"],
                            nitrateMgl1 = (float)reader["nitrateMgl1"],
                            nitrateLessThanMgL1 = (float)reader["nitrateLessThanMgL1"],
                            phosphateMgl1 = (float)reader["phosphateMgl1"],
                            ecCfu100ml = (float)reader["ecCfu100ml"]
                        });
                    }
                }
            }
            return items;
        }
    }

    public IEnumerable<WeatherDataModel> PopulateWeatherData()
    {
        var items = new ObservableCollection<WeatherDataModel>();
        using (SqlConnection con = new SqlConnection(ConnectionString))
        {
            con.Open();
            using (SqlCommand cmd = new SqlCommand("SELECT dataID, sensorID, sensorType, elevation, utcOffsetSeconds, timezone, timezoneAbr, date, time, temp2m, relativeHumidity2m, windSpeed, windDirection FROM dbo.weatherdata", con))
            {
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        items.Add(new WeatherDataModel
                        {
                            dataID = (int)reader["dataID"],
                            sensorID = (int)reader["sensorID"],
                            sensorType = (string)reader["sensorType"],
                            elevation = (float)reader["elevation"],
                            utcOffsetSeconds = (int)reader["utcOffsetSeconds"],
                            timezone = (string)reader["timezone"],
                            timezoneAbr = (string)reader["timezoneAbr"],
                            date = (DateOnly)reader["date"],
                            time = (TimeSpan)reader["time"],
                            temp2m = (float)reader["temp2m"],
                            relativeHumidity2m = (float)reader["relativeHumidity2m"],
                            windSpeed = (float)reader["windSpeed"],
                            windDirection = (int)reader["windDirection"]
                        });
                    }
                }
            }
            return items;

        }
    }
}