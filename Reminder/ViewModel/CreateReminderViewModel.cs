using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using MahApps.Metro.Controls.Dialogs;
using Reminder.Model;
using System;

namespace Reminder.ViewModel
{
    public class CreateReminderViewModel : ViewModelBase
    {
        private readonly IRemindersRepository _remindersRepo;
        private readonly IDialogCoordinator _dialogService;

        private Model.Reminder _currentReminder;

        public CreateReminderViewModel(IRemindersRepository repo, IDialogCoordinator dialogService)
        {
            _remindersRepo = repo;
            _dialogService = dialogService;

            CurrentReminder = new Model.Reminder();
        
            CreateCommand = new RelayCommand(CreateReminder);
            CancelCommand = new RelayCommand(CancelCreation);
        }

        public RelayCommand CreateCommand { get; set; }
        public RelayCommand CancelCommand { get; set; }

        public Model.Reminder CurrentReminder
        {
            get { return _currentReminder ?? new Model.Reminder(); }
            set { _currentReminder = value; }
        }

        public async void CreateReminder()
        {
            if (string.IsNullOrEmpty(CurrentReminder.Name)
                || string.IsNullOrEmpty(CurrentReminder.Description))
            {
                await _dialogService
                    .ShowMessageAsync(this, "ERROR", "Please make sure name and description fields are not empty!");
            }
            else if (CurrentReminder.BeginTime <= DateTime.Now)
            {
                await _dialogService
                   .ShowMessageAsync(this, "ERROR", "Please make sure your date and time value is in the future!");
            }
            else
            {
                _remindersRepo.Insert(CurrentReminder);
                CloseCreateReminderWindow();
            }
        }

        public void CancelCreation()
        {
            CloseCreateReminderWindow();
        }

        private void CloseCreateReminderWindow()
        {
            CurrentReminder = new Model.Reminder();
            Messenger.Default.Send(new NotificationMessage("CloseCreateReminderWindow"));
        }
    }
}
