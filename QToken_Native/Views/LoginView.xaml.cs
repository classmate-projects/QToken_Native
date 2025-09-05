using QToken_Native.ViewModels;

namespace QToken_Native.Views;

public partial class LoginView : ContentView
{
	public LoginView()
	{
		InitializeComponent();
        BindingContext = new UserAuthenticationViewModel();
    }
}