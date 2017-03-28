using GalaSoft.MvvmLight.Messaging;
using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Xml.Serialization;

namespace Reminder.Model
{
    public class RemindersRepository : IRemindersRepository
    {        
        private readonly XmlSerializer _serializer = new XmlSerializer(typeof(ObservableCollection<Reminder>));
        private string _path;

        public RemindersRepository()
        {
            Messenger.Default.Register<PropertyChangedMessage<string>>(this, (s) => { Path = s.NewValue; });
        }

        public ObservableCollection<Reminder> Reminders { get; set; } = new ObservableCollection<Reminder>();

        public string Path
        {
            get { return _path; }
            set
            {
                _path = value;
                SetReminders();
            }
        }

        public ObservableCollection<Reminder> GetAll()
        {
            try
            {
                using (TextReader textReader = new StreamReader(File.Open(_path, FileMode.OpenOrCreate)))
                {
                    return ((ObservableCollection<Reminder>)_serializer.Deserialize(textReader));
                }
            }
            catch(Exception)
            {
                return null;
            }
        }

        public void Insert(Reminder reminder)
        {
            Reminders.Add(reminder);
        }

        public void Delete(Reminder reminder)
        {
            Reminders.Remove(reminder);
        }

        public void SaveChanges()
        {
            using (TextWriter textWriter = new StreamWriter(_path))
            {
                _serializer.Serialize(textWriter, Reminders);
            }
        }

        private void SetReminders()
        {
            Reminders.Clear();

            ObservableCollection<Reminder> reminders = GetAll();
            
            if (reminders != null)
            {
                foreach (Reminder reminder in reminders)
                {
                    Reminders.Add(reminder);
                }
            }
        }
    }
}
