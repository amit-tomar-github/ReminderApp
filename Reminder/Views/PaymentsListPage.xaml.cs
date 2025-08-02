using Reminder.ViewModels;

namespace Reminder.Views
{
    public partial class PaymentsListPage : ContentPage
    {
        public PaymentsListPage()
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