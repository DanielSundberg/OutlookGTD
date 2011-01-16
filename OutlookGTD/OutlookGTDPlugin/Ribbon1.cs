using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using Microsoft.Office.Interop.Outlook;
using Office = Microsoft.Office.Core;
using System.Diagnostics;
using Microsoft.Office.Core;
using System.Windows.Forms;
using OutlookGTD.Logic;

// TODO:  Follow these steps to enable the Ribbon (XML) item:

// 1: Copy the following code block into the ThisAddin, ThisWorkbook, or ThisDocument class.

//  protected override Microsoft.Office.Core.IRibbonExtensibility CreateRibbonExtensibilityObject()
//  {
//      return new Ribbon1();
//  }

// 2. Create callback methods in the "Ribbon Callbacks" region of this class to handle user
//    actions, such as clicking a button. Note: if you have exported this Ribbon from the Ribbon designer,
//    move your code from the event handlers to the callback methods and modify the code to work with the
//    Ribbon extensibility (RibbonX) programming model.

// 3. Assign attributes to the control tags in the Ribbon XML file to identify the appropriate callback methods in your code.  

// For more information, see the Ribbon XML documentation in the Visual Studio Tools for Office Help.


namespace OutlookGTDPlugin
{
    [ComVisible(true)]
    public class Ribbon1 : Office.IRibbonExtensibility
    {
        private Office.IRibbonUI ribbon;
        private Microsoft.Office.Interop.Outlook.Application _application;

        public Ribbon1()
        {
        }

        public void SetApplication(Microsoft.Office.Interop.Outlook.Application application)
        {
            _application = application;
        }

        #region IRibbonExtensibility Members

        public string GetCustomUI(string ribbonID)
        {
            return GetResourceText("OutlookGTDPlugin.Ribbon1.xml");
        }

        #endregion

        #region Ribbon Callbacks
        public void CreateTaskClicked(IRibbonControl control)
        {            
            var selection = _application.ActiveExplorer().Selection.Cast<MailItem>();
            var mailItem = selection.ElementAt(0);
            
            var folder = mailItem.Parent as Folder;

            // Create guid for mail here
            string guid = GetNewOrExistingGuid(mailItem);

            TaskItem taskItem = _application.CreateItem(OlItemType.olTaskItem);
            taskItem.Subject = mailItem.Subject;
            taskItem.Body = "\n\n" + BuildMailItemLink(mailItem, folder, guid);
            taskItem.Display();
        }


        public void LinkToTaskClicked(IRibbonControl control)
        {
            List<TaskItem> taskList = FindAllTasks(_application.Session.Stores as Stores);
            using (TaskListView taskListView = new TaskListView())
            {
                taskListView.SetItems(taskList);
                if (taskListView.ShowDialog() == DialogResult.OK)
                {
                    TaskItem taskItem = taskListView.GetSelectedTask();
                    if (taskItem != null)
                    {
                        var selection = _application.ActiveExplorer().Selection.Cast<MailItem>();
                        var mailItem = selection.ElementAt(0);
                        var folder = mailItem.Parent as Folder;

                        // Create guid for mail here
                        string guid = GetNewOrExistingGuid(mailItem);

                        // Append mail link to task
                        StringBuilder stringBuilder = new StringBuilder(taskItem.Body);
                        stringBuilder.AppendLine();
                        stringBuilder.AppendLine(BuildMailItemLink(mailItem, folder, guid));
                        taskItem.Body = stringBuilder.ToString();
                        taskItem.Display();
                    }
                }
            }
            
        }

        public List<TaskItem> FindAllTasks(Stores stores)
        {
            List<TaskItem> taskList = new List<TaskItem>();
            foreach (Store store in stores)
            {
                FindTasksInStore(taskList, store);
            }
            return taskList;
        }

        private string BuildMailItemLink(MailItem mailItem, Folder folder, string guid)
        {
            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.Append("MailLink=");
            stringBuilder.Append(folder.FolderPath);
            stringBuilder.Append(":");
            stringBuilder.Append(mailItem.EntryID);
            stringBuilder.Append(":");
            stringBuilder.Append(guid);
            return stringBuilder.ToString();
        }

        private static string GetNewOrExistingGuid(MailItem mailItem)
        {
            UserProperty property = Utils.GetGtdGuidFromMailItem(mailItem);
            if (property != null)
            {
                return property.Value.ToString();
            }

            Guid guid = Guid.NewGuid();
            mailItem.UserProperties.Add(Utils.GTD_GUID, OlUserPropertyType.olText).Value = guid.ToString();
            return guid.ToString();
        }


        private void FindTasksInStore(List<TaskItem> taskList, Store store)
        {
            FindTasksInFolder(taskList, store.GetRootFolder() as Folder);
        }

        private void FindTasksInFolder(List<TaskItem> taskList, Folder folder)
        {
            if (folder.DefaultItemType == OlItemType.olTaskItem)
            {
                foreach (TaskItem taskItem in folder.Items)
                {
                    taskList.Add(taskItem);
                }
            }
            foreach (Folder f in folder.Folders)
            {
                FindTasksInFolder(taskList, f);
            }
        }

        public void Ribbon1_Load(Office.IRibbonUI ribbonUI)
        {
            this.ribbon = ribbonUI;
        }

        #endregion

        #region Helpers

        private static string GetResourceText(string resourceName)
        {
            Assembly asm = Assembly.GetExecutingAssembly();
            string[] resourceNames = asm.GetManifestResourceNames();
            for (int i = 0; i < resourceNames.Length; ++i)
            {
                if (string.Compare(resourceName, resourceNames[i], StringComparison.OrdinalIgnoreCase) == 0)
                {
                    using (StreamReader resourceReader = new StreamReader(asm.GetManifestResourceStream(resourceNames[i])))
                    {
                        if (resourceReader != null)
                        {
                            return resourceReader.ReadToEnd();
                        }
                    }
                }
            }
            return null;
        }

        #endregion
    }
}
