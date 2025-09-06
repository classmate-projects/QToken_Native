using QToken_Native.ViewModels;

namespace QToken_Native.Pages;

public partial class UserAuthentication : ContentPage
{
	public UserAuthentication()
	{
		InitializeComponent();
        BindingContext = new UserAuthenticationViewModel();
    }
}