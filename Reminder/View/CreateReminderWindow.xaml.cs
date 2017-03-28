using GalaSoft.MvvmLight.Messaging;
using MahApps.Metro.Controls;

namespace Reminder.View
{
    /// <summary>
    /// Interaction logic for CreateReminderWindow.xaml
    /// </summary>
    public partial class CreateReminderWindow : MetroWindow
    {
        public CreateReminderWindow()
        {
            InitializeComponent();
            Messenger.Default.Register<NotificationMessage>(this, NotificationMessageReceived);
        }

        private void NotificationMessageReceived(NotificationMessage msg)
        {
            switch (msg.Notification)
            {
                case "CloseCreateReminderWindow":
                    Close();
                    break;
            }
        }
    }
}
