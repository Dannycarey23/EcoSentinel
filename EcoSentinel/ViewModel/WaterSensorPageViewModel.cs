using CommunityToolkit.Mvvm.ComponentModel;
using EcoSentinel.Model;
using System.Collections.ObjectModel;
using System.ComponentModel;

namespace EcoSentinel.ViewModel;

public partial class WaterSensorPageViewModel : INotifyPropertyChanged
{
    private DatabaseService _databaseService;
    private ObservableCollection<SensorModel> _sensorData;
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

    public WaterSensorPageViewModel()
    {
        _databaseService = new DatabaseService();
        _sensorData = new ObservableCollection<SensorModel>();
        NavigateCommand = new Command<SensorModel>(OnNavigate);
        LoadData();
    }
    private void LoadData()
    {
        var dataList = _databaseService.PopulateSensorData();
        SensorData = new ObservableCollection<SensorModel>(dataList);
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
