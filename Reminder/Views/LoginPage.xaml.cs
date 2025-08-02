namespace Reminder.Views
{
    public partial class LoginPage : ContentPage
    {
        private bool _isPinSet = false;
        private bool _isAnimating;
        private readonly string _pinFilePath;
        private const string PinFileName = "app.pin";

        public LoginPage()
        {
            InitializeComponent();
            // Get the path to the file where the PIN will be stored.
            // FileSystem.AppDataDirectory is a safe place for application data.
            _pinFilePath = Path.Combine(FileSystem.AppDataDirectory, PinFileName);
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();
            await StartEntryAnimation();

            // Check if the PIN file exists
            bool pinExists = File.Exists(_pinFilePath);

            // Show the appropriate UI based on whether a PIN has been set.
            LoginButton.IsVisible = pinExists;
            SetPinButton.IsVisible = !pinExists;

            PinEntry.Focus();
        }

        private async Task StartEntryAnimation()
        {
            LoginControls.Opacity = 0;
            LogoImage.Scale = 0.7;
            LogoImage.Rotation = -10;

            await Task.WhenAll(
                LogoImage.ScaleTo(1, 700, Easing.SpringOut),
                LogoImage.RotateTo(0, 700, Easing.SpringOut),
                LoginControls.FadeTo(1, 500)
            );
        }

        private async void OnLoginClicked(object sender, EventArgs e)
        {
            if (_isAnimating) return;
            _isAnimating = true;

            try
            {
                // Read the PIN from the file.
                string savedPin = await File.ReadAllTextAsync(_pinFilePath);

                if (PinEntry.Text == savedPin)
                {
                    await AnimateLoginSuccess();
                    Application.Current.MainPage = new AppShell();
                }
                else
                {
                    await AnimateLoginError();
                }
            }
            finally
            {
                _isAnimating = false;
            }
        }

        private async void OnSavePinClicked(object sender, EventArgs e)
        {
            string newPin = PinEntry.Text;

            if (string.IsNullOrWhiteSpace(newPin) || newPin.Length != 4)
            {
                await DisplayAlert("Error", "Please enter a valid 4-digit PIN.", "OK");
                return;
            }

            try
            {
                // Get the path to the app's data directory.
                string pinFilePath = Path.Combine(FileSystem.AppDataDirectory, PinFileName);

                // Save the PIN to the file.
                await File.WriteAllTextAsync(pinFilePath, newPin);

                LoginButton.IsVisible = true;
                SetPinButton.IsVisible = false;
                PinEntry.Text = string.Empty;

                PinEntry.Focus();

                await DisplayAlert("Success", "PIN has been saved successfully!", "OK");

            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", $"Could not save PIN: {ex.Message}", "OK");
            }
        }

        private async Task AnimateLoginSuccess()
        {
            await Task.WhenAll(
                LoginButton.ScaleTo(0.9, 100, Easing.SpringIn),
                LoginButton.ScaleTo(1, 100, Easing.SpringOut)
            );

            await Task.WhenAll(
                LoginControls.FadeTo(0, 300),
                this.ScaleTo(1.1, 300)
            );
        }

        private async Task AnimateLoginError()
        {
            // Shake animation
            uint timeout = 50;
            await LoginButton.TranslateTo(-15, 0, timeout);
            await LoginButton.TranslateTo(15, 0, timeout);
            await LoginButton.TranslateTo(-10, 0, timeout);
            await LoginButton.TranslateTo(10, 0, timeout);
            await LoginButton.TranslateTo(0, 0, timeout);

            PinEntry.Text = string.Empty;
            await DisplayAlert("Error", "Invalid PIN", "OK");
        }
    }
}