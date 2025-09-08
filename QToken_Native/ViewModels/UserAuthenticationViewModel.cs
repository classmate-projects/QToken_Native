using QToken_Native.API;
using QToken_Native.Models;
using QToken_Native.Pages;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using System.Windows.Input;


namespace QToken_Native.ViewModels
{
    public class UserAuthenticationViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string propertyName) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

        #region Properties

        //Pickers and selections
        public ObservableCollection<Specialty> Specialties { get; set; } = new();

        private Specialty _selectedSpecialty;
        public Specialty SelectedSpecialty
        {
            get => _selectedSpecialty;
            set
            {
                if (_selectedSpecialty != value)
                {
                    _selectedSpecialty = value;
                    OnPropertyChanged(nameof(SelectedSpecialty));
                }
            }
        }

        // Shared properties
        public string UserName { get; set; }
        public string Password { get; set; }

        // Registration-specific
        public string Name { get; set; }
        public string Speciality { get; set; }

        // Observable properties for UI state

        private bool _isLoginVisible;
        public bool IsLoginVisible
        {
            get => _isLoginVisible;
            set
            {
                _isLoginVisible = value;
                OnPropertyChanged(nameof(IsLoginVisible));
            }
        }

        private bool _isRegistrationVisible;
        public bool IsRegistrationVisible
        {
            get => _isRegistrationVisible;
            set
            {
                if (_isRegistrationVisible != value)
                {
                    _isRegistrationVisible = value;
                    OnPropertyChanged(nameof(IsRegistrationVisible));
                }
            }
        }

        private string _role;
        public string Role
        {
            get => _role;
            set
            {
                if (_role != value)
                {
                    _role = value;
                    OnPropertyChanged(nameof(Role));
                }
            }
        }

        #endregion

        #region Commands
        public Command RegisterCommand { get; }
        public Command LoginCommand { get; }
        public ICommand ItemViewController => new Command(async () =>
        {
            IsLoginVisible = !IsLoginVisible;
            OnPropertyChanged(nameof(IsLoginVisible));

            IsRegistrationVisible = !IsRegistrationVisible;
            OnPropertyChanged(nameof(IsRegistrationVisible));

            await Task.CompletedTask;
        });
        #endregion


        public UserAuthenticationViewModel()
        {
            RegisterCommand = new Command(async () => await RegisterAsync());
            LoginCommand = new Command(async () => await LoginAsync());
        }


        #region Private Methods
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
                Speciality = SelectedSpecialty.Name,
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
                    IsLoginVisible = true;
                    IsRegistrationVisible = false;
                    Role="User";
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
                    Application.Current.MainPage = new NavigationPage(new HomePage());
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

        #endregion

        #region Public Methods

        public async Task InitializeViewStateAsync()
        {
            try
            {
                using var client = new HttpClient();
                client.BaseAddress = new Uri(APIHost.Host);

                bool adminExists = await client.GetFromJsonAsync<bool>(APIEndpoints.HaveAdmin);

                if (!adminExists)
                    Role = "Admin";
                else
                    Role = "User";
                IsLoginVisible = adminExists;
                IsRegistrationVisible = !adminExists;
            }
            catch (Exception ex)
            {
                IsLoginVisible = true;
                IsRegistrationVisible = false;

                await Application.Current.MainPage.DisplayAlert(
                    "Connection Error",
                    "Unable to verify admin . Please check your network or contact support.",
                    "OK"
                );
            }
        }
        public async Task LoadSpecialtiesAsync()
        {
            using var client = new HttpClient();
            client.BaseAddress = new Uri(APIHost.Host);

            var specialties = await client.GetFromJsonAsync<List<Specialty>>(APIEndpoints.GetSpecialities);

            if (specialties != null)
            {
                Specialties.Clear();
                foreach (var s in specialties)
                    Specialties.Add(s);
            }
        }

        #endregion
        
    }
}
