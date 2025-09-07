using QToken_Native.ViewModels;

namespace QToken_Native.Pages;

public partial class UserAuthentication : ContentPage
{
	private readonly UserAuthenticationViewModel _viewModel;
    public UserAuthentication()
	{
		InitializeComponent();
        _viewModel = new UserAuthenticationViewModel();
        BindingContext = _viewModel;
    }
    protected override async void OnAppearing()
    {
        base.OnAppearing();
        await _viewModel.LoadSpecialtiesAsync();
    }
}