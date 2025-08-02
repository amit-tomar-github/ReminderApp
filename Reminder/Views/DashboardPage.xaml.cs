using Reminder.ViewModels;

namespace Reminder.Views
{
    public partial class DashboardPage : ContentPage
    {
        public DashboardPage()
        {
            InitializeComponent();
            BindingContext = new DashboardViewModel();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            if (BindingContext is DashboardViewModel vm && vm.LoadPaymentsCommand.CanExecute(null))
                vm.LoadPaymentsCommand.Execute(null);
        }
    }
}