using System.Collections.ObjectModel;
using System.Windows.Input;
using Reminder.Models;
using Reminder.Services;
using Reminder.Views;

namespace Reminder.ViewModels
{
    public class PaymentsListViewModel : BindableObject
    {
        private readonly DatabaseService _dbService;
        public ObservableCollection<Payment> Payments { get; } = new();

        public ICommand LoadPaymentsCommand { get; }
        public ICommand EditPaymentCommand { get; }
        public ICommand AddPaymentCommand { get; }
        public ICommand DeletePaymentCommand { get; }

        public PaymentsListViewModel()
        {
            _dbService = App.Database;
            LoadPaymentsCommand = new Command(async () => await LoadPaymentsAsync());
            EditPaymentCommand = new Command<Payment>(async (payment) => await EditPaymentAsync(payment));
            AddPaymentCommand = new Command(async () => await AddPaymentAsync());
            DeletePaymentCommand = new Command<Payment>(async (payment) => await DeletePaymentAsync(payment));
        }

        private async Task LoadPaymentsAsync()
        {
            try
            {
                Payments.Clear();
                var items = await _dbService.GetAllPaymentsAsync();
                foreach (var item in items)
                {
                    Payments.Add(item);
                }
            }
            catch (Exception ex)
            {
                await Shell.Current.DisplayAlert("Error", 
                    "Failed to load payments: " + ex.Message, "OK");
            }
        }

        private async Task EditPaymentAsync(Payment payment)
        {
            if (payment == null) return;

            var page = new AddReminderPage(payment);
            page.PaymentSaved += async (s, e) => await LoadPaymentsAsync();
            
            await Shell.Current.Navigation.PushAsync(page);
        }

        private async Task AddPaymentAsync()
        {
            var page = new AddReminderPage();
            page.PaymentSaved += async (s, e) => await LoadPaymentsAsync();
            
            await Shell.Current.Navigation.PushAsync(page);
        }

        private async Task DeletePaymentAsync(Payment payment)
        {
            if (payment == null || payment.Id == 0) {
                await Shell.Current.DisplayAlert("Error", "Cannot delete: Payment not found or invalid.", "OK");
                return;
            }

            bool confirm = await Shell.Current.DisplayAlert(
                "Delete Reminder", 
                $"Are you sure you want to delete {payment.Name}?", 
                "Yes", "No");

            if (!confirm) return;

            try
            {
                await _dbService.DeletePaymentAsync(payment);
                await LoadPaymentsAsync();
            }
            catch (Exception ex)
            {
                await Shell.Current.DisplayAlert("Error",
                    "Failed to delete payment: " + ex.Message, "OK");
            }
        }
    }
}