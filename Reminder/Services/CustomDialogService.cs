using Microsoft.Win32;
using System.Windows;

namespace Reminder.Services
{
    class CustomDialogService : ICustomDialogService
    {
        public string ShowFileDialog()
        {
            OpenFileDialog dlg = new OpenFileDialog();
   
            dlg.DefaultExt = ".xml"; 
            dlg.Filter = "Text documents (.xml)|*.xml"; 

            bool? result = dlg.ShowDialog();

            if (result == true)
            {
                return dlg.FileName;
            }
            else
            {
                return null;
            }
        }

        public void ShowMessageBox(string title, string msg)
        {
            MessageBox.Show(msg, title);
        }
    }
}
