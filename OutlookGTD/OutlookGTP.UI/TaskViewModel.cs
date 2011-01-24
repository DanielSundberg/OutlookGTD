using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.Collections.ObjectModel;
using System.Windows.Forms;
using System.Windows.Input;

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
            MessageBox.Show(message);
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

        public void MailDoubleClick(MessageWrapper messageWrapper)
        {
            if (MailClicked != null)
            {
                MailClicked(messageWrapper);
            }
        }

        public int SelectedIndex
        {
            get { return _selectedIndex; }
            set
            {
                _selectedIndex = value;
                if (PropertyChanged != null)
                {
                    PropertyChanged(this, new PropertyChangedEventArgs("SelectedIndex"));
                }
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        private string _entryId;
        private string _folderPath;
        private string _subject;
        private string _body;
        private int _selectedIndex;

        public void Clear()
        {
            MessageWrapperList.Clear();
            Subject = "";
        }
        
        public class DelegateCommand : ICommand
        {
            private bool _isEnabled;
            private Action _onExecute;

            public DelegateCommand(Action executeHandler)
            {
                _isEnabled = true;
                _onExecute = executeHandler;
            }

            public bool IsEnabled
            {
                get { return _isEnabled; }
                set
                {
                    _isEnabled = value;
                    if (CanExecuteChanged != null)
                    {
                        CanExecuteChanged(this, EventArgs.Empty);
                    }
                }
            }


            public bool CanExecute(object parameter)
            {
                return _isEnabled;
            }

            public event EventHandler CanExecuteChanged;

            public void Execute(object parameter)
            {
                _onExecute();
            }
        }

    }
}
