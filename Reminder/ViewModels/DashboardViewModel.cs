using System.Collections.ObjectModel;
using System.Windows.Input;
using Reminder.Models;
using Reminder.Services;
using Reminder.Views;

namespace Reminder.ViewModels
{
    public class DashboardViewModel : BindableObject
    {
        private readonly DatabaseService _dbService;
        private decimal _pendingAmount;
        private int _pendingCount;

        public ObservableCollection<Payment> PendingPayments { get; } = new();
        
        public decimal PendingAmount
        {
            get => _pendingAmount;
            private set
            {
                if (_pendingAmount != value)
                {
                    _pendingAmount = value;
                    OnPropertyChanged();
                }
            }
        }

        public int PendingCount
        {
            get => _pendingCount;
            private set
            {
                if (_pendingCount != value)
                {
                    _pendingCount = value;
                    OnPropertyChanged();
                }
            }
        }

        public ICommand LoadPaymentsCommand { get; }
        public ICommand CompletePaymentCommand { get; }

        public DashboardViewModel()
        {
            _dbService = App.Database;
            LoadPaymentsCommand = new Command(async () => await LoadPaymentsAsync());
            CompletePaymentCommand = new Command<Payment>(async (payment) => await CompletePaymentAsync(payment));
        }

        private async Task LoadPaymentsAsync()
        {
            try
            {
                PendingPayments.Clear();
                var items = await _dbService.GetCurrentMonthPaymentsAsync();
                var pendingItems = items;
                
                foreach (var item in pendingItems)
                {
                    PendingPayments.Add(item);
                }

                // Update summary
                PendingAmount = pendingItems.Sum(p => p.Amount);
                PendingCount = pendingItems.Count;
            }
            catch (Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert("Error", 
                    "Failed to load payments: " + ex.Message, "OK");
            }
        }

        private async Task CompletePaymentAsync(Payment payment)
        {
            if (payment == null) return;

            try
            {
                // update as next month's payment
                payment.DueDate = payment.DueDate.AddMonths(1);

                await _dbService.SavePaymentAsync(payment);
          
                // Refresh the list and summary
                await LoadPaymentsAsync();

                await Application.Current.MainPage.DisplayAlert("Success", 
                    "Payment completed and scheduled for next month", "OK");
            }
            catch (Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert("Error",
                    "Failed to complete payment: " + ex.Message, "OK");
            }
        }
    }
}