using System;
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
        public TaskListView()
        {
            InitializeComponent();
        }

        public void SetItems(List<TaskItem> taskList)
        {
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
    }
}
