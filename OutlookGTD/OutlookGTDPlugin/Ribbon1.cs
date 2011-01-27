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

namespace OutlookGTDPlugin
{
    [ComVisible(true)]
    public class Ribbon1 : IRibbonExtensibility
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
            taskItem.Body = Utils.RemoveHyperLinks(mailItem.Body) + "\n\n" + Utils.BuildMailItemLink(mailItem, folder, guid);
            taskItem.Display();
        }


        public void LinkToTaskClicked(IRibbonControl control)
        {
            //List<TaskItem> taskList = FindAllTasks(_application.Session.Stores as Stores);
            List<TaskItem> taskList = FindDefaultTasks();
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
                        stringBuilder.AppendLine(Utils.BuildMailItemLink(mailItem, folder, guid));
                        taskItem.Body = stringBuilder.ToString();
                        taskItem.Display();
                    }
                }
            }
            
        }

        public List<TaskItem> FindDefaultTasks()
        {
            List<TaskItem> taskList = new List<TaskItem>();

            Folder taskFolder = (Folder)_application.Session.GetDefaultFolder(OlDefaultFolders.olFolderTasks);
            FindTasksInFolderNonRecursive(taskList, taskFolder);
            return taskList;
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

        private void FindTasksInFolderNonRecursive(List<TaskItem> taskList, Folder folder)
        {
            if (folder.DefaultItemType == OlItemType.olTaskItem)
            {
                foreach (TaskItem taskItem in folder.Items)
                {
                    taskList.Add(taskItem);
                }
            }
        }

        public void Ribbon1_Load(IRibbonUI ribbonUI)
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
