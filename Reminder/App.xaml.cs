using Microsoft.Maui.Controls.Platform;
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
        }

        protected override Window CreateWindow(IActivationState activationState)
        {
            var window = new Window(new LoginPage())
            {
                Title = "Reminder App"
            };

            // Handle when the window is created
            window.Created += (s, e) =>
            {
                // Any additional window initialization can go here
            };

            return window;
        }
    }
}