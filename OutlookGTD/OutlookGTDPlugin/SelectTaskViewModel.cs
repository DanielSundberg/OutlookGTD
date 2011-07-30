using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.ComponentModel;
using System.Windows.Data;

namespace OutlookGTDPlugin
{
    public class TaskDisplayItem : INotifyPropertyChanged
    {
        private string _subject;
        private DateTime _date;

        public DateTime Date
        {
            get { return _date; }
            set
            {
                _date = value;
                if (PropertyChanged != null)
                {
                    PropertyChanged(this, new PropertyChangedEventArgs("Date"));
                }
            }
        }
        public string DateString
        {
            get
            {
                return _date.ToShortDateString();
            }
        }

        private bool _visible = true;

        public bool Visible
        {
            get { return _visible; }
            set 
            { 
                _visible = value;
                if (PropertyChanged != null)
                {
                    PropertyChanged(this, new PropertyChangedEventArgs("Visible"));
                }
            }
        }
        public string Subject
        {
            get { return _subject; }
            set { _subject = value; }
        }
        public TaskDisplayItem()
        {
        }
        public TaskDisplayItem(string title, DateTime date)
        {
            _subject = title;
            _date = date;
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }

    public class SelectTaskViewModel : INotifyPropertyChanged
    {
        ObservableCollection<TaskDisplayItem> _taskDisplayItems = new ObservableCollection<TaskDisplayItem>();
        
        public SelectTaskViewModel(List<Microsoft.Office.Interop.Outlook.TaskItem> taskList)
        {
            foreach (Microsoft.Office.Interop.Outlook.TaskItem taskItem in taskList)
            {
                _taskDisplayItems.Add(new TaskDisplayItem(taskItem.Subject, taskItem.DueDate));
            }
        }

        public ObservableCollection<TaskDisplayItem> TaskDisplayItems
        {
            get
            {
                return _taskDisplayItems;
            }
        }

        public TaskDisplayItem FirstVisible()
        {
            return _taskDisplayItems.FirstOrDefault(t => t.Visible);
        }

        public string SearchFilter 
        {
            get
            {
                return _searchFilter;
            }
            set
            {
                _searchFilter = value;
                if (string.IsNullOrEmpty(_searchFilter))
                {
                    //_allAnimals.ForEach(a => a.Visible = System.Windows.Visibility.Visible);
                    foreach (var a in _taskDisplayItems)
                    {
                        a.Visible = true;
                    }
                }
                else
                {
                    //_allAnimals.ForEach(a => a.Visible = (a.Name.ToLower().Contains(_searchFilter.ToLower()) ? System.Windows.Visibility.Visible : System.Windows.Visibility.Collapsed));
                    foreach (var a in _taskDisplayItems)
                    {
                        if (a.Subject.ToLower().Contains(_searchFilter.ToLower()))
                            a.Visible = true;
                        else
                            a.Visible = false;
                    }
                }
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        private string _searchFilter;
    }
}
