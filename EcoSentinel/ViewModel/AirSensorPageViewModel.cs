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
        LoadData();
    }
    private void LoadData()
    {
        var dataList = _databaseService.PopulateSensorData();
        SensorData = new ObservableCollection<SensorModel>(dataList);
    }
    public event PropertyChangedEventHandler? PropertyChanged;
    protected virtual void OnPropertyChanged(string propertyName)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
