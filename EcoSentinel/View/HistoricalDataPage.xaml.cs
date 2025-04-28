using EcoSentinel.ViewModel;

namespace EcoSentinel.View;

public partial class HistoricalDataPage : ContentPage
{
	public HistoricalDataPage()
	{
		InitializeComponent();
		BindingContext = new HistoricalPageViewModel();
	}
}