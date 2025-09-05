using QToken_Native.Pages;

namespace QToken_Native
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();

            MainPage = new NavigationPage(new UserAuthenticatin());
        }
    }
}
