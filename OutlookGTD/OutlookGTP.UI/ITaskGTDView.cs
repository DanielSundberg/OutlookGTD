using System;
using System.Windows.Forms;
namespace OutlookGTP.UI
{
    public interface ITaskGTDView
    {
        void ClearLinkedMessages();
        string EntryId { get; set; }
        string FolderPath { get; set; }
        event OutlookGTD.UI.TaskGTDView.MailClickedEventHandler MailClicked;
        void SetLinkedMessages(System.Collections.Generic.List<MessageWrapper> subjects);
        void ShowMessage(string message);
        string Subject { get; set; }
        UserControl GetUserControl();
    }
}
