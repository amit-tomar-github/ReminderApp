using Reminder.Services;
using Reminder.Views;

namespace Reminder
{
    public partial class App : Application
    {
        public static DatabaseService Database { get; private set; }

        public App()
        {
            InitializeComponent();
            Database = new DatabaseService();
            MainPage = new LoginPage();
        }
    }
}