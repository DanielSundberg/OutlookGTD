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

        public string FolderPath
        {
            get { return _folderTextBox.Text; }
            set { _folderTextBox.Text = value; }
        }

        public string EntryId
        {
            get { return _entryIdTextBox.Text; }
            set { _entryIdTextBox.Text = value; }
        }

        public void ShowMessage(string message)
        {
            MessageBox.Show(message);
        }

        public void SetSubjects(List<MessageWrapper> subjects)
        {
            _conversationTtreeView.Nodes.Clear();
            foreach (var item in subjects)
            {
                TreeNode node = new TreeNode(item.Header);
                node.Tag = item.Body;
                _conversationTtreeView.Nodes.Add(node);
            }
        }

        private void _conversationTtreeView_NodeMouseClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            
            string messageBody = e.Node.Tag as string;
            if (messageBody != null)
            {
                _bodyTextBox.Text = messageBody;
            }
            else
            {
                _bodyTextBox.Text = "";
            }
        }
    }

    public class MessageWrapper
    {
        public string Header { get; set; }
        public string Body { get; set; }
    }
}

