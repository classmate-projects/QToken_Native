namespace QToken_Native.Pages;

public partial class HomePage : ContentPage
{
	public HomePage()
	{
		InitializeComponent();
		BindingContext = new ViewModels.HomePageViewModel();
    }
}