﻿using System;
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
        public TaskDisplayItem(Microsoft.Office.Interop.Outlook.TaskItem taskItem)
        {
            _subject = taskItem.Subject;
            _date = taskItem.DueDate;
            _taskItem = taskItem;
        }

        public event PropertyChangedEventHandler PropertyChanged;
        private Microsoft.Office.Interop.Outlook.TaskItem _taskItem;

        public Microsoft.Office.Interop.Outlook.TaskItem TaskItem
        {
            get { return _taskItem; }
            set { _taskItem = value; }
        }
    }

    public class SelectTaskViewModel : INotifyPropertyChanged
    {
        ObservableCollection<TaskDisplayItem> _taskDisplayItems = new ObservableCollection<TaskDisplayItem>();
        
        public SelectTaskViewModel(List<Microsoft.Office.Interop.Outlook.TaskItem> taskList)
        {
            foreach (Microsoft.Office.Interop.Outlook.TaskItem taskItem in taskList)
            {
                _taskDisplayItems.Add(new TaskDisplayItem(taskItem));
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
                
                var view = CollectionViewSource.GetDefaultView(_taskDisplayItems);
                view.Filter = null;
                view.Filter = i => ((TaskDisplayItem)i).Subject.ToLower().Contains(_searchFilter.ToLower());
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        private string _searchFilter;

        internal void SelectPrev()
        {
            var dv = CollectionViewSource.GetDefaultView(_taskDisplayItems);
            dv.MoveCurrentToPrevious();
            if (dv.IsCurrentBeforeFirst)
            {
                dv.MoveCurrentToFirst();
            }
        }

        internal void SelectNext()
        {
            var dv = CollectionViewSource.GetDefaultView(_taskDisplayItems);
            dv.MoveCurrentToNext();
            if (dv.IsCurrentAfterLast)
            {
                dv.MoveCurrentToLast();
            }
        }

        internal Microsoft.Office.Interop.Outlook.TaskItem GetSelectedTask()
        {
            var dv = CollectionViewSource.GetDefaultView(_taskDisplayItems);
            if (dv.IsCurrentAfterLast || dv.IsCurrentBeforeFirst)
            {
                dv.MoveCurrentToNext();
            }
            if (dv.CurrentItem != null)
                return ((TaskDisplayItem)dv.CurrentItem).TaskItem;
            else
                return null;
        }
    }
}
