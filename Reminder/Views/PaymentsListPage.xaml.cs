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
    }
}