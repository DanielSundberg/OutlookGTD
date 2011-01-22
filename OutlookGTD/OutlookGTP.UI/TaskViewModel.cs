using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.Collections.ObjectModel;

namespace OutlookGTP.UI
{
    public class TaskViewModel : INotifyPropertyChanged
    {
        private ObservableCollection<MessageWrapper> _messageWrapperList = new ObservableCollection<MessageWrapper>();

        public string EntryId
        {
            get
            {
                return _entryId;
            } 
            set
            {
                _entryId = value;
                if (PropertyChanged != null)
                {
                    PropertyChanged(this, new PropertyChangedEventArgs("EntryId"));
                }
            }
        }

        public string FolderPath
        {
            get
            {
                return _folderPath;
            }
            set
            {
                _folderPath = value;
                if (PropertyChanged != null)
                {
                    PropertyChanged(this, new PropertyChangedEventArgs("FolderPath"));
                }

            }
        }

        public event OutlookGTD.UI.TaskGTDView.MailClickedEventHandler MailClicked;

        public void SetLinkedMessages(List<MessageWrapper> messages)
        {
            _messageWrapperList.Clear();
            messages.ForEach(m => _messageWrapperList.Add(m));
        }

        public ObservableCollection<MessageWrapper> MessageWrapperList
        {
            get
            {
                return _messageWrapperList;
            }
        }

        public void ShowMessage(string message)
        {
            //_taskViewWPF.ShowMessage(message);
        }

        public string Subject
        {
            get
            {
                return _subject;
            }
            set
            {
                _subject = value;
                Body = "";
                if (PropertyChanged != null)
                {
                    PropertyChanged(this, new PropertyChangedEventArgs("Subject"));
                }
            }
        }

        public string Body
        {
            get
            {
                return _body;
            }
            set
            {
                _body = value;
                if (PropertyChanged != null)
                {
                    PropertyChanged(this, new PropertyChangedEventArgs("Body"));
                }

            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        private string _entryId;
        private string _folderPath;
        private string _subject;
        private string _body;
    }
}
