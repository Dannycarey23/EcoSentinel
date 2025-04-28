using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using CommunityToolkit.Mvvm.ComponentModel;
using EcoSentinel.Model;

namespace EcoSentinel.ViewModel
{
    public partial class HistoricalDataPageViewModel : INotifyPropertyChanged
    {
        private DatabaseService _databaseService;
        private ObservableCollection<SensorModel> _sensorData;
        public ObservableCollection<AirDataModel> AirData { get; set; } = new();
        public ObservableCollection<WaterDataModel> WaterData { get; set; } = new();
        public ObservableCollection<WeatherDataModel> WeatherData { get; set; } = new();
        public Command<SensorModel> NavigateCommand { get; }
        private string _currentDataType;
        public string CurrentDataType
        {
            get => _currentDataType;
            set
            {
                _currentDataType = value;
                OnPropertyChanged(nameof(CurrentDataType));
                OnPropertyChanged(nameof(CurrentData));
            }
        }

        public ObservableCollection<SensorModel> SensorData
        {
            get => _sensorData;
            set
            {
                _sensorData = value;
                OnPropertyChanged(nameof(SensorData));
            }
        }

        public ObservableCollection<object> CurrentData
        {
            get
            {
                return CurrentDataType switch
                {
                    "air" => new ObservableCollection<object>(AirData),
                    "water" => new ObservableCollection<object>(WaterData),
                    "weather" => new ObservableCollection<object>(WeatherData),
                    _ => new ObservableCollection<object>(),
                };
            }
        }

        public HistoricalDataPageViewModel()
        {
            _databaseService = new DatabaseService();
            _sensorData = new ObservableCollection<SensorModel>();
            NavigateCommand = new Command<SensorModel>(OnNavigate);
            LoadSensorData();
            LoadHistoricalData();
        }

        private void LoadSensorData()
        {
            var sensorData = _databaseService.PopulateSensorData();
            SensorData = new ObservableCollection<SensorModel>(sensorData);
        }

        private void LoadHistoricalData()
        {
            var airData = _databaseService.PopulateAirData()
                .OrderByDescending(x => x.date)
                .ThenByDescending(x => x.time);
            var waterData = _databaseService.PopulateWaterData()
                .OrderByDescending(x => x.date)
                .ThenByDescending(x => x.time);
            var weatherData = _databaseService.PopulateWeatherData()
                .OrderByDescending(x => x.date)
                .ThenByDescending(x => x.time);
            AirData = new ObservableCollection<AirDataModel>(airData);
            WaterData = new ObservableCollection<WaterDataModel>(waterData);
            WeatherData = new ObservableCollection<WeatherDataModel>(weatherData);
        }

        private void OnNavigate(SensorModel sensor)
        {
            Console.WriteLine($"Loading {sensor.sensorType} data...");
            CurrentDataType = sensor.sensorType;
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
