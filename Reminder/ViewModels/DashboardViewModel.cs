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
            
            LoadPaymentsCommand.Execute(null);
        }

        private async Task LoadPaymentsAsync()
        {
            try
            {
                PendingPayments.Clear();
                var items = await _dbService.GetCurrentMonthPaymentsAsync();
                var pendingItems = items.Where(p => !p.IsCompleted).ToList();
                
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
                // Create next month's payment
                var nextPayment = new Payment
                {
                    Name = payment.Name,
                    Description = payment.Description,
                    Amount = payment.Amount,
                    DueDate = payment.DueDate.AddMonths(1),
                    IsCompleted = false
                };

                // Mark current payment as completed
                payment.IsCompleted = true;
                await _dbService.SavePaymentAsync(payment);
                await _dbService.SavePaymentAsync(nextPayment);

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