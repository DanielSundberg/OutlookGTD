using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace OutlookGTP.UI
{
    public partial class TaskGTDViewWPFHost : UserControl, ITaskGTDView
    {
        public TaskGTDViewWPFHost()
        {
            InitializeComponent();

            _taskViewModel = new TaskViewModel();
            _taskViewWPF.DataContext = _taskViewModel;
        }

        public UserControl GetUserControl()
        {
            return this;
        }

        public void ClearLinkedMessages()
        {
            //_taskViewWPF.ClearLinkedMessages();
        }

        public string EntryId
        {
            get
            {
                return _taskViewModel.EntryId; ;
            }
            set
            {
                _taskViewModel.EntryId = value;
            }
        }

        public string FolderPath
        {
            get
            {
                return _taskViewModel.FolderPath;
            }
            set
            {
                _taskViewModel.FolderPath = value;
            }
        }

        public event OutlookGTD.UI.TaskGTDView.MailClickedEventHandler MailClicked;
        private TaskViewModel _taskViewModel;

        public void SetLinkedMessages(List<MessageWrapper> subjects)
        {
            _taskViewModel.SetLinkedMessages(subjects);
        }

        public void ShowMessage(string message)
        {
            _taskViewModel.ShowMessage(message);
        }

        public string Subject
        {
            get
            {
                return _taskViewModel.Subject;
            }
            set
            {
                _taskViewModel.Subject = value;
            }
        }
    }
}
