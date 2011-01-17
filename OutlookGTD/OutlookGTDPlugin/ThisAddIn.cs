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
        private TaskGTDView _taskPaneControl;
        private Microsoft.Office.Tools.CustomTaskPane _customTaskPane;
        private Ribbon1 _ribbon1;
        private Explorer _activeExplorer;

        private void ThisAddIn_Startup(object sender, System.EventArgs e)
        {

            _taskPaneControl = new TaskGTDView();
            _customTaskPane = this.CustomTaskPanes.Add(_taskPaneControl, "Task info");
            _customTaskPane.Visible = true;

            _activeExplorer = Application.ActiveExplorer();
            _activeExplorer.SelectionChange += new ExplorerEvents_10_SelectionChangeEventHandler(ThisAddIn_SelectionChange);

            _ribbon1.SetApplication(Application);
        }

        private void ThisAddIn_SelectionChange()
        {
            if (Application.ActiveExplorer().Selection.Count > 0)
            {
                var item = Application.ActiveExplorer().Selection[1];

                if (item is TaskItem)
                {
                    TaskItem taskItem = item as TaskItem;
                    _taskPaneControl.Subject = taskItem.Subject;
                    _taskPaneControl.FolderPath = "";
                    _taskPaneControl.EntryId = "";

                    TaskBodyParser taskBodyParser = new TaskBodyParser(taskItem, Application.Session.Stores as Stores);
                    var messages = taskBodyParser.ParseBody();

                    _taskPaneControl.SetLinkedMessages(messages);
                }
                else if (item is MailItem)
                {
                    MailItem mailItem = item as MailItem;
                    _taskPaneControl.Subject = mailItem.Subject;
                    _taskPaneControl.FolderPath = mailItem.Application.ActiveExplorer().CurrentFolder.FolderPath;

                    UserProperty userProperty = Utils.GetGtdGuidFromMailItem(mailItem);
                    if (userProperty != null)
                    {
                        _taskPaneControl.EntryId = userProperty.Value.ToString();
                    }
                    else
                    {
                        _taskPaneControl.EntryId = mailItem.EntryID;
                    }

                    //_taskPaneControl.EntryId = guid;
                    _taskPaneControl.ClearLinkedMessages();
                }
            }
        }

        private void ThisAddIn_Shutdown(object sender, System.EventArgs e)
        {

        }

        protected override Microsoft.Office.Core.IRibbonExtensibility CreateRibbonExtensibilityObject()
        {
            _ribbon1 = new Ribbon1();
            return _ribbon1;
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
