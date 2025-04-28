using Microsoft.Maui.Controls;
using EcoSentinel.ViewModel;

namespace EcoSentinel.Selectors
{
    public class DataTypeTemplateSelector : DataTemplateSelector
    {
        public DataTemplate AirDataTemplate { get; set; }
        public DataTemplate WaterDataTemplate { get; set; }
        public DataTemplate WeatherDataTemplate { get; set; }

        protected override DataTemplate OnSelectTemplate(object item, BindableObject container)
        {
            var viewModel = (HistoricalDataPageViewModel)((BindableObject)container).BindingContext;

            return viewModel.CurrentDataType switch
            {
                "air" => AirDataTemplate,
                "water" => WaterDataTemplate,
                "weather" => WeatherDataTemplate,
                _ => null,
            };
        }
    }
}
