using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using OutlookGTP.UI;

namespace OutlookGTD.UI
{
    public partial class TaskGTDView : UserControl
    {
        public delegate void MailClickedEventHandler(MessageWrapper messageWrapper);
        public event MailClickedEventHandler MailClicked;

        public void ShowMessage(string message)
        {
            MessageBox.Show(message);
        }

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

        public void SetLinkedMessages(List<MessageWrapper> subjects)
        {
            // TODO: Check if node.Tag leaks memory
            _conversationTtreeView.Nodes.Clear();
            foreach (var item in subjects)
            {
                TreeNode node = new TreeNode(string.Format("{0} ({1})", item.Subject, item.Sender));
                node.Tag = item;
                _conversationTtreeView.Nodes.Add(node);
            }
        }

        private void _conversationTtreeView_NodeMouseClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            MessageWrapper messageWrapper = e.Node.Tag as MessageWrapper;
            if (messageWrapper != null)
            {
                _bodyTextBox.Text = messageWrapper.Body;
            }
            else 
            {
                _bodyTextBox.Text = "";
            }
        }

        public void ClearLinkedMessages()
        {
            _conversationTtreeView.Nodes.Clear();
            _bodyTextBox.Text = "";
        }

        private void _conversationTtreeView_NodeMouseDoubleClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            if (MailClicked != null)
            {
                MailClicked(_conversationTtreeView.SelectedNode.Tag as MessageWrapper);
            }
        }
    }
}

