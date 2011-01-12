using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using Microsoft.Office.Core;
using Microsoft.Office.Interop.Outlook;
using OutlookGTP.UI;
using OutlookGTD.Logic;
using Outlook = Microsoft.Office.Interop.Outlook;
using Office = Microsoft.Office.Core;
using OutlookGTDPlugin;

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

            Application.ItemLoad += new Outlook.ApplicationEvents_11_ItemLoadEventHandler(Application_ItemLoad);
            Application.ActiveExplorer().BeforeItemPaste += new ExplorerEvents_10_BeforeItemPasteEventHandler(ThisAddIn_BeforeItemPaste);            

        }

        private void ThisAddIn_BeforeItemPaste(ref object ClipboardContent, MAPIFolder Target, ref bool Cancel)

        {
            Console.WriteLine("Paste");
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
            if (_taskItem != null)
            {
                _taskPaneControl.Subject = _taskItem.Subject;
                _taskPaneControl.FolderPath = "";
                _taskPaneControl.EntryId = "";

                TaskBodyParser taskBodyParser = new TaskBodyParser(_taskItem, Application.Session.Stores as Stores);
                var messages = taskBodyParser.ParseBody();

                _taskPaneControl.SetLinkedMessages(messages);
            }
            else if (_mailItem != null)
            {
                _taskPaneControl.Subject = _mailItem.Subject;
                _taskPaneControl.FolderPath = _mailItem.Application.ActiveExplorer().CurrentFolder.FolderPath;
                _taskPaneControl.EntryId = _mailItem.EntryID;
                _taskPaneControl.ClearLinkedMessages();

                
            }
        }

        private void ThisAddIn_Shutdown(object sender, System.EventArgs e)
        {

        }

        protected override Microsoft.Office.Core.IRibbonExtensibility CreateRibbonExtensibilityObject()
        {
            return new Ribbon1();
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
