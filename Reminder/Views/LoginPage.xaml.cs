namespace Reminder.Views
{
    public partial class LoginPage : ContentPage
    {
        private const string PIN = "9897";
        private bool _isAnimating;

        public LoginPage()
        {
            InitializeComponent();
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();
            await StartEntryAnimation();
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
                if (PinEntry.Text == PIN)
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