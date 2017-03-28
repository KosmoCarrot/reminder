using GalaSoft.MvvmLight;
using System;
using System.Xml.Serialization;

namespace Reminder.Model
{
    [Serializable]
    public class Reminder : ObservableObject
    {
        [XmlIgnore]
        public bool IsAppeared = false;
        
        private DateTime _beginTime = DateTime.Now;
        private TimeSpan _interval = TimeSpan.Zero;
        private DateTime _nextAppearance;

        public Reminder()
        {

        }

        public string Name { get; set; }
        public string Description { get; set; }

        public DateTime BeginTime
        {
            get { return _beginTime; }
            set
            {
                _beginTime = value;
                NextAppearanceTime = value;
            }
        }

        [XmlIgnore]
        public TimeSpan Interval
        {
            get { return _interval; }
            set { _interval = value; }
        }

        [XmlElement("Interval")]
        public long IntervalTicks
        {
            get { return _interval.Ticks; }
            set { _interval = new TimeSpan(value); }
        }

        [XmlIgnore]
        public DateTime NextAppearanceTime
        {
            get
            {
                return _nextAppearance;
            }
            set
            {
                _nextAppearance = value;
                RaisePropertyChanged("NextAppearanceTime");
            }
        }

        public void CalculateNextAppearanceTime()
        {
            DateTime next = BeginTime;

            while (next <= DateTime.Now)
            {
                next = next.Add(Interval);
            };

            NextAppearanceTime = next;
        }

    }
}
