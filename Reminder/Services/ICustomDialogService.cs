namespace Reminder.Services
{
    public interface ICustomDialogService
    {
        string ShowFileDialog();
        void ShowMessageBox (string title, string msg);
    }
}
