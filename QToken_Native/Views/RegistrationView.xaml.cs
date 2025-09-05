using QToken_Native.ViewModels;

namespace QToken_Native.Views;

public partial class RegistrationView : ContentView
{
	public RegistrationView()
	{
		InitializeComponent();
        BindingContext = new UserAuthenticationViewModel();
    }
}