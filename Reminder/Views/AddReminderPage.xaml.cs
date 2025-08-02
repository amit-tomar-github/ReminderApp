using Reminder.Models;
using Reminder.Services;

namespace Reminder.Views
{
    public partial class AddReminderPage : ContentPage
    {
        private Payment _payment;
        private readonly DatabaseService _dbService;
        public event EventHandler PaymentSaved;

        public AddReminderPage(Payment payment = null)
        {
            InitializeComponent();
            _dbService = App.Database;
            _payment = payment ?? new Payment { DueDate = DateTime.Now };
            BindingContext = _payment;

            Title = payment == null ? "Add Reminder" : "Edit Reminder";
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            
            // Animate entry fields
            uint delay = 100;
            Animation entryAnimation = new Animation();
            
            void AddAnimation(View view, uint index)
            {
                view.Opacity = 0;
                view.TranslationX = 50;
                
                entryAnimation.Add(0, 1, new Animation(v => view.Opacity = v, 0, 1));
                entryAnimation.Add(0, 1, new Animation(v => view.TranslationX = v, 50, 0));
            }

            AddAnimation(NameEntry, 0);
            AddAnimation(DescEntry, 1);
            AddAnimation(DueDatePicker, 2);
            AddAnimation(AmountEntry, 3);
            AddAnimation(SaveButton, 4);

            entryAnimation.Commit(this, "EntryAnimation", 16, 400, Easing.CubicOut);
        }

        private async void OnSaveClicked(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(_payment.Name))
            {
                await DisplayAlert("Error", "Please enter a name", "OK");
                return;
            }

            if (_payment.Amount <= 0)
            {
                await DisplayAlert("Error", "Please enter a valid amount", "OK");
                return;
            }

            SaveButton.IsEnabled = false;
            ActivityIndicator.IsVisible = true;

            try
            {
                await _dbService.SavePaymentAsync(_payment);
                PaymentSaved?.Invoke(this, EventArgs.Empty);
                await Navigation.PopAsync();
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", "Failed to save reminder: " + ex.Message, "OK");
            }
            finally
            {
                SaveButton.IsEnabled = true;
                ActivityIndicator.IsVisible = false;
            }
        }
    }
}