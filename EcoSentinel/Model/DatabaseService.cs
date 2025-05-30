using System;
using System.Collections;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
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

    public void AddUserData(string u, string p, string r, string em, string fn, string ln)
    {
        using (SqlConnection conn = new SqlConnection(ConnectionString))
        {
            conn.Open();
            using (SqlCommand cmd = new SqlCommand("INSERT INTO users (username, [password], role, email, fname, lname) VALUES (@u, @p, @r, @em, @fn, @ln)", conn))
            {
                cmd.Parameters.AddWithValue("@u", u);
                cmd.Parameters.AddWithValue("@p", p);
                cmd.Parameters.AddWithValue("@r", r);
                cmd.Parameters.AddWithValue("@em", em);
                cmd.Parameters.AddWithValue("@fn", fn);
                cmd.Parameters.AddWithValue("@ln", ln);

                cmd.ExecuteNonQuery();
            }
        }
    }


    public void DeleteUserData(string u)
    {
        using (SqlConnection conn = new SqlConnection(ConnectionString))
        {
            conn.Open();
            using (SqlCommand cmd = new SqlCommand("DELETE FROM users WHERE username = @u", conn))
            {
                cmd.Parameters.AddWithValue("@u", u);
                cmd.ExecuteNonQuery();
            }
        }
    }

    public void SetPasswordData(string u,string p)
    {
        using (SqlConnection conn = new SqlConnection(ConnectionString))
        {
            conn.Open();
            using (SqlCommand cmd = new SqlCommand("UPDATE users SET [password] = @p WHERE username = @u;", conn))
            {
                cmd.Parameters.AddWithValue("@u", u);
                cmd.Parameters.AddWithValue("@p", p);
                cmd.ExecuteNonQuery();
            }
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
                            date = DateOnly.FromDateTime((DateTime)reader["date"]),
                            time = (TimeSpan)reader["time"],
                            nitrateMgl1 = reader["nitrateMgl1"] is DBNull ? 0.0f : Convert.ToSingle(reader["nitrateMgl1"]),
                            nitrateLessThanMgL1 = reader["nitrateLessThanMgL1"] is DBNull ? 0.0f : Convert.ToSingle(reader["nitrateLessThanMgL1"]),
                            phosphateMgl1 = reader["phosphateMgl1"] is DBNull ? 0.0f : Convert.ToSingle(reader["phosphateMgl1"]),
                            ecCfu100ml = reader["ecCfu100ml"] is DBNull ? 0.0f : Convert.ToSingle(reader["ecCfu100ml"])
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
                            elevation = reader["elevation"] is DBNull ? 0.0f : Convert.ToSingle(reader["elevation"]),
                            utcOffsetSeconds = (int)reader["utcOffsetSeconds"],
                            timezone = (string)reader["timezone"],
                            timezoneAbr = (string)reader["timezoneAbr"],
                            date = DateOnly.FromDateTime((DateTime)reader["date"]),
                            time = (TimeSpan)reader["time"],
                            temp2m = reader["temp2m"] is DBNull ? 0.0f : Convert.ToSingle(reader["temp2m"]),
                            relativeHumidity2m = reader["relativeHumidity2m"] is DBNull ? 0.0f : Convert.ToSingle(reader["relativeHumidity2m"]),
                            windSpeed = reader["windSpeed"] is DBNull ? 0.0f : Convert.ToSingle(reader["windSpeed"]),
                            windDirection = (int)reader["windDirection"]
                        });
                    }
                }
            }
            return items;

        }
    }
}