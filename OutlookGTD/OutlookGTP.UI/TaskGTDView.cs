using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace OutlookGTD.UI
{
    public partial class TaskGTDView : UserControl
    {
        public TaskGTDView()
        {
            InitializeComponent();
        }

        public string Subject
        {
            get { return _subjectTextBox.Text; }
            set { _subjectTextBox.Text = value; }
        }

        public string Folder
        {
            get { return _folderTextBox.Text; }
            set { _folderTextBox.Text = value; }
        }

        public string ConversationId
        {
            get { return _conversationIdTextBox.Text; }
            set { _conversationIdTextBox.Text = value; }
        }

        public string Store
        {
            get { return _storeTextBox.Text; }
            set { _storeTextBox.Text = value; }
        }

        public void ShowMessage(string message)
        {
            MessageBox.Show(message);
        }
    }
}

