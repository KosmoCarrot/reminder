using GalaSoft.MvvmLight.Messaging;
using MahApps.Metro.Controls;
using Reminder.View;

namespace Reminder
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : MetroWindow
    {
        public MainWindow()
        {
            InitializeComponent();
            Messenger.Default.Register<NotificationMessage>(this, NotificationMessageReceived);
        }

        private void NotificationMessageReceived(NotificationMessage msg)
        {
            switch (msg.Notification)
            {
                case "ShowCreateReminderWindow":
                    var window = new CreateReminderWindow();
                    window.ShowDialog();
                    break;
            }
        }
    }
}
