using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using MahApps.Metro.Controls.Dialogs;
using Reminder.Model;
using Reminder.Services;
using System;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Threading;

namespace Reminder.ViewModel
{
    public class MainViewModel : ViewModelBase
    {
        private readonly IRemindersRepository _remindersRepo;
        private readonly IDialogCoordinator _dialogService;
        private readonly ICustomDialogService _customDialogService;
    
        private string _selectedPath;

        public MainViewModel(IRemindersRepository repo, IDialogCoordinator dialogService,
            ICustomDialogService customDialogService)
        {
            _remindersRepo = repo;
            _dialogService = dialogService;
            _customDialogService = customDialogService;

            SelectedPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), "reminders.xml");
            SortedReminders = CollectionViewSource.GetDefaultView(_remindersRepo.Reminders);
            SortedReminders.SortDescriptions.Add(new SortDescription("NextAppearanceTime", ListSortDirection.Ascending));
            
            DispatcherTimerSetup();

            SaveCommand = new RelayCommand(SaveReminders);
            CreateCommand = new RelayCommand(CreateReminder);
            DeleteCommand = new RelayCommand(DeleteReminder);
            SetPathCommand = new RelayCommand(SetPath);
        }

        public RelayCommand SaveCommand { get; set; }
        public RelayCommand CreateCommand { get; set; }
        public RelayCommand DeleteCommand { get; set; }
        public RelayCommand SetPathCommand { get; set; }

        public ICollectionView SortedReminders { get; set; }
        public Model.Reminder SelectedReminder { get; set; }

        public string SelectedPath
        {
            get { return _selectedPath; }
            set
            {
                _selectedPath = value;
                RaisePropertyChanged("SelectedPath", string.Empty, value, true);
                RemindersSetup();
            }
        }

        public async void SaveReminders()
        {
            _remindersRepo.SaveChanges();
            await _dialogService.ShowMessageAsync(this, "SUCCESS", "Your changes have been saved!");
        }

        public void CreateReminder()
        {
            Messenger.Default.Send(new NotificationMessage("ShowCreateReminderWindow"));
        }

        public async void DeleteReminder()
        {
            if (SelectedReminder != null)
            {
                _remindersRepo.Delete(SelectedReminder);
            }
            else
            {
                await _dialogService.ShowMessageAsync(this, "ERROR", "Reminder has not been selected! Try again!");
            }
        }

        public async void SetPath()
        {
            string _path = _customDialogService.ShowFileDialog();

            if (_path != null)
            {
                if (Path.GetExtension(_path) == ".xml")
                {
                    SelectedPath = _path;
                }
                else
                {
                    await _dialogService.ShowMessageAsync(this, "ERROR", "Not possible set a file path! Try again!");
                }
            }
        }

        private void RemindersSetup()
        {
            foreach (var reminder in _remindersRepo.Reminders)
            {
                reminder.CalculateNextAppearanceTime();
            }
        }

        private void DispatcherTimerSetup()
        {
            DispatcherTimer dispatcherTimer = new DispatcherTimer();
            dispatcherTimer.Interval = TimeSpan.FromSeconds(1);
            dispatcherTimer.Tick += CheckRemindersAppearance;
            dispatcherTimer.Start();
        }

        private void CheckRemindersAppearance(object sender, EventArgs e)
        {
            var remindersToAppear = SortedReminders.SourceCollection.Cast<Model.Reminder>()
                                 .Where(r => r.NextAppearanceTime < DateTime.Now && r.IsAppeared == false);

            foreach (var reminder in remindersToAppear)
            {
                var showReminder = Task.Run(() => {
                    reminder.IsAppeared = true;
                    _customDialogService.ShowMessageBox(reminder.Name, reminder.Description);
                });

                showReminder.ContinueWith((antecedent) =>
                {
                    reminder.IsAppeared = false;

                    if (reminder.Interval == TimeSpan.Zero)
                    {
                        App.Current.Dispatcher.Invoke(() => _remindersRepo.Delete(reminder));
                        _remindersRepo.SaveChanges();
                    }
                    else
                    {
                        reminder.CalculateNextAppearanceTime();
                    }
                });

                SortedReminders.Refresh();
            }
        }
    }
}