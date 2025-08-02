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
    }
}