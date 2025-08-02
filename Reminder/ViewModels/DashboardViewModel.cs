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
        private decimal _completedAmount;

        public ObservableCollection<Payment> Payments { get; } = new();
        
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

        public decimal CompletedAmount
        {
            get => _completedAmount;
            private set
            {
                if (_completedAmount != value)
                {
                    _completedAmount = value;
                    OnPropertyChanged();
                }
            }
        }

        public ICommand LoadPaymentsCommand { get; }
        public ICommand EditPaymentCommand { get; }
        public ICommand AddPaymentCommand { get; }

        public DashboardViewModel()
        {
            _dbService = App.Database;
            LoadPaymentsCommand = new Command(async () => await LoadPaymentsAsync());
            EditPaymentCommand = new Command<Payment>(async (payment) => await EditPaymentAsync(payment));
            AddPaymentCommand = new Command(async () => await AddPaymentAsync());
            
            LoadPaymentsCommand.Execute(null);
        }

        private async Task LoadPaymentsAsync()
        {
            try
            {
                Payments.Clear();
                var items = await _dbService.GetCurrentMonthPaymentsAsync();
                
                foreach (var item in items)
                {
                    Payments.Add(item);
                }

                // Update summary amounts
                PendingAmount = Payments.Where(p => !p.IsCompleted).Sum(p => p.Amount);
                CompletedAmount = Payments.Where(p => p.IsCompleted).Sum(p => p.Amount);
            }
            catch (Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert("Error", 
                    "Failed to load payments: " + ex.Message, "OK");
            }
        }

        private async Task EditPaymentAsync(Payment payment)
        {
            if (payment == null) return;

            var page = new PaymentEditorPage(payment);
            page.PaymentSaved += async (s, e) => await LoadPaymentsAsync();
            
            await Shell.Current.Navigation.PushAsync(page);
        }

        private async Task AddPaymentAsync()
        {
            var page = new PaymentEditorPage();
            page.PaymentSaved += async (s, e) => await LoadPaymentsAsync();
            
            await Shell.Current.Navigation.PushAsync(page);
        }
    }
}