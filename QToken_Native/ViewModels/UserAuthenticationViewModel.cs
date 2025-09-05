using QToken_Native.API;
using QToken_Native.Models;
using System.ComponentModel;
using System.Text;
using System.Text.Json;


namespace QToken_Native.ViewModels
{
    public class UserAuthenticationViewModel : INotifyPropertyChanged
    {
        // Shared properties
        public string UserName { get; set; }
        public string Password { get; set; }

        // Registration-specific
        public string Name { get; set; }
        public string Speciality { get; set; }
        public string Role { get; set; } = "user";

        // View toggling
        public bool IsLoginVisible { get; set; } = true;
        public bool IsRegistrationVisible => !IsLoginVisible;

        public Command ToggleViewCommand { get; }
        public Command RegisterCommand { get; }
        public Command LoginCommand { get; }

        public UserAuthenticationViewModel()
        {
            ToggleViewCommand = new Command(() => IsLoginVisible = !IsLoginVisible);
            RegisterCommand = new Command(async () => await RegisterAsync());
            LoginCommand = new Command(async () => await LoginAsync());
        }

        private async Task RegisterAsync()
        {
            if (string.IsNullOrWhiteSpace(UserName) || string.IsNullOrWhiteSpace(Password))
            {
                await Application.Current.MainPage.DisplayAlert("Validation", "Username and password are required.", "OK");
                return;
            }

            var dto = new RegisterDTO
            {
                Name = Name,
                UserName = UserName,
                Password = Password,
                Speciality = Speciality,
                Role = Role
            };

            var json = JsonSerializer.Serialize(dto);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            using var client = new HttpClient();

            try
            {
                var response = await client.PostAsync($"{APIHost.Host}{APIEndpoints.Register}", content);

                if (response.IsSuccessStatusCode)
                {
                    var result = await response.Content.ReadAsStringAsync();
                    await Application.Current.MainPage.DisplayAlert("🎉 Success", "Registration successful!", "OK");

                    // Optionally clear fields or navigate to login
                    Name = UserName = Password = Speciality = string.Empty;
                    OnPropertyChanged(nameof(Name));
                    OnPropertyChanged(nameof(UserName));
                    OnPropertyChanged(nameof(Password));
                    OnPropertyChanged(nameof(Speciality));
                }
                else
                {
                    var error = await response.Content.ReadAsStringAsync();
                    await Application.Current.MainPage.DisplayAlert("⚠️ Error", error, "OK");
                }
            }
            catch (Exception ex)
            {
                string message;

                if (ex.Message.Contains("Cleartext HTTP traffic"))
                {
                    message = "⚠️ Your device is blocking non-secure connections.\n\nPlease use HTTPS or enable cleartext traffic for localhost.";
                }
                else
                {
                    message = $"🚨 Unexpected error:\n{ex.Message}";
                }

                await Application.Current.MainPage.DisplayAlert("Connection Error", message, "OK");

            }
        }

        private async Task LoginAsync()
        {
            var dto = new UserLoginDTO
            {
                UserName = UserName,
                Password = Password
            };

            var json = JsonSerializer.Serialize(dto);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            using var client = new HttpClient();

            try
            {
                var response = await client.PostAsync($"{APIHost.Host}{APIEndpoints.Login}", content);

                if (response.IsSuccessStatusCode)
                {
                    var result = await response.Content.ReadAsStringAsync();
                    await Application.Current.MainPage.DisplayAlert("✅ Login", "Welcome back!", "OK");

                    // Optionally navigate to dashboard or store user info
                }
                else
                {
                    var error = await response.Content.ReadAsStringAsync();
                    await Application.Current.MainPage.DisplayAlert("❌ Login Failed", error, "OK");
                }
            }
            catch (Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert("🚨 Error", ex.Message, "OK");
            }
        }


        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged(string name) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }
}
