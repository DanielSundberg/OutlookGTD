using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using Microsoft.Office.Core;
using Microsoft.Office.Interop.Outlook;
using Outlook = Microsoft.Office.Interop.Outlook;
using Office = Microsoft.Office.Core;

namespace OutlookGTD.UI
{
    public partial class ThisAddIn
    {
        private TaskItem _taskItem;
        private MailItem _mailItem;
        private TaskGTDView _taskPaneControl;
        private Microsoft.Office.Tools.CustomTaskPane _customTaskPane;
        

        private void ThisAddIn_Startup(object sender, System.EventArgs e)
        {

            _taskPaneControl = new TaskGTDView();
            _customTaskPane = this.CustomTaskPanes.Add(_taskPaneControl, "Task info");
            _customTaskPane.Visible = true;


            //Outlook.Folder rootStoreFolder = Application.Session.DefaultStore.GetRootFolder() as Outlook.Folder;
            //foreach (Outlook.Folder f in rootStoreFolder.Folders)
            //{
            // TODO: is it "Tasks" in all languages, can we look at child item type instead?
            //if (f.Name == "Tasks")
            //{
            //    foreach(Outlook.TaskItem taskItem in f.Items)
            //    {
            //        Debug.WriteLine(taskItem.Subject + ":" + taskItem.Body);
            //    }
            //}
            //}

            Application.ItemLoad += new Outlook.ApplicationEvents_11_ItemLoadEventHandler(Application_ItemLoad);
        }

        private void Application_ItemLoad(object Item)
        {
            _taskItem = null;
            _mailItem = null;
            if (Item is Outlook.TaskItem)
            {

                var taskItem = Item as Outlook.TaskItem;
                _taskItem = taskItem;
                taskItem.Read += new Outlook.ItemEvents_10_ReadEventHandler(taskItem_Read);

                // Open side bar
                //_customTaskPane.Visible = true;
            }
            else if (Item is MailItem)
            {
                var mailItem = Item as MailItem;
                _mailItem = mailItem;
                mailItem.Read += new ItemEvents_10_ReadEventHandler(taskItem_Read);
            }
            else
            {
                // Hide side bar
                //_customTaskPane.Visible = false;
            }
        }
      
        private void taskItem_Read()
        {
            //Debug.WriteLine(_taskItem.Subject + ":" + _taskItem.Body);
            if (_taskItem != null)
            {
                _taskPaneControl.Subject = _taskItem.Subject;
                _taskPaneControl.Folder = _taskItem.Application.ActiveExplorer().CurrentFolder.Name;

            }
            else if (_mailItem != null)
            {
                _taskPaneControl.Subject = _mailItem.Subject;
                _taskPaneControl.Folder = _mailItem.Application.ActiveExplorer().CurrentFolder.Name;
                //_mailItem.Id
                
                // TODO: visa subject, folderpath, EntryId
                // TODO: gör länk i task-body:n med detta
                // TODO: parsa länk och visa i listvy i side:baren för en task

                //_taskPaneControl.ConversationId =
                //_mailItem.Application.ActiveExplorer().CurrentFolder.FolderPath;
                    //_mailItem.ConversationID;
                
            }
        }

        private void ThisAddIn_Shutdown(object sender, System.EventArgs e)
        {

        }

        #region VSTO generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InternalStartup()
        {
            this.Startup += new System.EventHandler(ThisAddIn_Startup);
            this.Shutdown += new System.EventHandler(ThisAddIn_Shutdown);
        }

        #endregion
    }
}
