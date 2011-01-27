using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Microsoft.Office.Interop.Outlook;
using Office = Microsoft.Office.Core;

namespace OutlookGTDPlugin
{
    public partial class TaskListView : Form
    {
        private Stores _stores;
        public TaskListView()
        {
            InitializeComponent();
            _listView.ListViewItemSorter = new ListViewItemComparer();
        }

        public TaskListView(Stores stores)
        {
            InitializeComponent();
            _listView.ListViewItemSorter = new ListViewItemComparer();
            _stores = stores;
        }

        public void SetItems(List<TaskItem> taskList)
        {
            _listView.Items.Clear();
            foreach (TaskItem item in taskList)
            {
                if ((item.Status == OlTaskStatus.olTaskInProgress) ||
                    (item.Status == OlTaskStatus.olTaskNotStarted) ||
                    (item.Status == OlTaskStatus.olTaskWaiting))
                {

                    ListViewItem listViewItem = new ListViewItem(new string[] { item.Subject, GetDueDate(item) });
                    listViewItem.Tag = item;
                    _listView.Items.Add(listViewItem);
                }
            }
            _listView.Sort();
        }

        private static string GetDueDate(TaskItem item)
        {
            if (item.DueDate.Date.Equals(new DateTime(4501,1,1)))
            {
                return "No date set";
            }
            else
            {
                return item.DueDate.ToShortDateString();
            }
        }

        public TaskItem GetSelectedTask()
        {
            if (_listView.SelectedItems.Count > 0)
            {
                return (TaskItem)_listView.SelectedItems[0].Tag;
            }
            return null;
        }

        private void _listView_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            DialogResult = DialogResult.OK;
        }

        private void _okButton_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.OK;
        }

        private void _showAllButton_Click(object sender, EventArgs e)
        {
            SetItems(TaskModel.FindAllTasks(_stores));
        }

        private void _cancelButton_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
        }

        class ListViewItemComparer : IComparer
        {
            public int Compare(object x, object y)
            {
                {
                    int returnVal;
                    // Determine whether the type being compared is a date type.
                    try
                    {
                        // Parse the two objects passed as a parameter as a DateTime.
                        System.DateTime firstDate = 
                                DateTime.Parse(((ListViewItem)x).SubItems[1].Text);
                        System.DateTime secondDate = 
                                DateTime.Parse(((ListViewItem)y).SubItems[1].Text);
                        // Compare the two dates.
                        returnVal = DateTime.Compare(firstDate, secondDate);
                    }
                    // If neither compared object has a valid date format, compare
                    // as a string.
                    catch 
                    {
                        // Compare the two items as a string.
                        returnVal = String.Compare(((ListViewItem)x).SubItems[1].Text,
                                    ((ListViewItem)y).SubItems[1].Text);
                    }
                    // Determine whether the sort order is descending.
                    //if (order == SortOrder.Descending)
                    //// Invert the value returned by String.Compare.
                    //    returnVal *= -1;
                    return returnVal;
                }

            }
        }

    }
}
