using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Reminder.View
{
    public partial class ListBoxItemDictionary : ResourceDictionary
    {
        public ListBoxItemDictionary()
        {
            InitializeComponent();
        }

        protected void SelectCurrentItem(object sender, KeyboardFocusChangedEventArgs e)
        {
            ListBoxItem item = (ListBoxItem)sender;
            item.IsSelected = true;
        }
    }
}