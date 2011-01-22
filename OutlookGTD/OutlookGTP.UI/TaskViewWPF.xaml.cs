using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace OutlookGTP.UI
{
    /// <summary>
    /// Interaction logic for TaskViewWPF.xaml
    /// </summary>
    public partial class TaskViewWPF : UserControl
    {
        public TaskViewWPF()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, RoutedEventArgs e)
        {

        }

        public void ClearLinkedMessages()
        {
        }

        public string EntryId
        {
            get
            {
                return string.Empty;
            }
            set
            {
            }
        }

        public string FolderPath
        {
            get
            {
                return string.Empty;
            }
            set
            {
            }
        }

        public event OutlookGTD.UI.TaskGTDView.MailClickedEventHandler MailClicked;

        public void SetLinkedMessages(List<MessageWrapper> subjects)
        {
        }

        public void ShowMessage(string message)
        {
        }

        public string Subject
        {
            get
            {
                return string.Empty;
            }
            set
            {
            }
        }

        private void lstData_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            MessageWrapper messageWrapper = lstData.SelectedItem as MessageWrapper;
            if (messageWrapper != null)
            {
                txtBody.Text = messageWrapper.Body;
            }
            else
            {
                txtBody.Text = "";
            }
        }
    }
}
