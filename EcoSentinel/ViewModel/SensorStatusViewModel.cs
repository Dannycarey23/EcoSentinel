using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EcoSentinel.Model;

namespace EcoSentinel.ViewModel
{
    public partial class SensorStatusViewModel : INotifyPropertyChanged
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

        public SensorStatusViewModel()
        {
            _databaseService = new DatabaseService();
            _sensorData = new ObservableCollection<SensorModel>();
            LoadSensorData();
        }

        private void LoadSensorData()
        {
            var sensorData = _databaseService.PopulateSensorData();
            SensorData = new ObservableCollection<SensorModel>(sensorData);
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }


    }
}
