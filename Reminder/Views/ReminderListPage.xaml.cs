using Reminder.ViewModels;

namespace Reminder.Views
{
    public partial class ReminderListPage : ContentPage
    {
        public ReminderListPage()
        {
            InitializeComponent();
            BindingContext = new PaymentsListViewModel();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            if (BindingContext is PaymentsListViewModel vm && vm.LoadPaymentsCommand.CanExecute(null))
                vm.LoadPaymentsCommand.Execute(null);
        }
    }
}