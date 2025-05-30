using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using CommunityToolkit.Mvvm.ComponentModel;
using EcoSentinel.Model;
namespace EcoSentinel.ViewModel;


public partial class AirSensorPageViewModel : INotifyPropertyChanged
{
    private DatabaseService _databaseService;
    private ObservableCollection<SensorModel> _sensorData;
    public ObservableCollection<AirDataModel> AirData { get; set; } = new();
    public Command<SensorModel> NavigateCommand { get; }

    public ObservableCollection<SensorModel> SensorData
    {
        get => _sensorData;
        set
        {
            _sensorData = value;
            OnPropertyChanged(nameof(SensorData));
        }
    }

    public AirSensorPageViewModel()
    {
        _databaseService = new DatabaseService();
        _sensorData = new ObservableCollection<SensorModel>();   
        NavigateCommand = new Command<SensorModel>(OnNavigate);
        LoadSensorData();
        LoadAirSensorData();
    }
    private void LoadSensorData()
    {
        var sensorData = _databaseService.PopulateSensorData();
        SensorData = new ObservableCollection<SensorModel>(sensorData);        
    }

    private void LoadAirSensorData()
    {
        var airData = _databaseService.PopulateAirData()
            .OrderByDescending(x => x.date)
            .ThenByDescending(x => x.time)
            .Take(10);
        AirData = new ObservableCollection<AirDataModel>(airData);       
    }

    private async void OnNavigate(SensorModel sensor)
    {
        Console.WriteLine($"Navigating to {sensor.sensorType} page...");

        if (sensor.sensorType == "air")
        {
            await Shell.Current.GoToAsync($"///{nameof(AirSensorPage)}");
        }
        else if (sensor.sensorType == "water")
        {
            await Shell.Current.GoToAsync(($"///{nameof(WaterSensorPage)}"));
        }
        else if (sensor.sensorType == "weather")
        {
            await Shell.Current.GoToAsync($"///{nameof(WeatherSensorPage)}");
        }
    }
    public event PropertyChangedEventHandler? PropertyChanged;
    protected virtual void OnPropertyChanged(string propertyName)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
