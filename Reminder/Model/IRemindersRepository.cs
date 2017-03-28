using System.Collections.ObjectModel;

namespace Reminder.Model
{
    public interface IRemindersRepository
    {
       ObservableCollection<Reminder> Reminders { get; set; }

       ObservableCollection<Reminder> GetAll();
       void Insert(Reminder reminder);
       void Delete(Reminder reminder);
       void SaveChanges();
    }
}
