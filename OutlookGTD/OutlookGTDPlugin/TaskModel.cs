using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Office.Interop.Outlook;

namespace OutlookGTDPlugin
{
    public class TaskModel
    {
        public static List<TaskItem> FindDefaultTasks(Application application)
        {
            List<TaskItem> taskList = new List<TaskItem>();

            Folder taskFolder = (Folder)application.Session.GetDefaultFolder(OlDefaultFolders.olFolderTasks);
            FindTasksInFolderNonRecursive(taskList, taskFolder);
            return taskList;
        }

        public static List<TaskItem> FindAllTasks(Stores stores)
        {
            List<TaskItem> taskList = new List<TaskItem>();
            foreach (Store store in stores)
            {
                FindTasksInStore(taskList, store);
            }
            return taskList;
        }

        private static void FindTasksInStore(List<TaskItem> taskList, Store store)
        {
            FindTasksInFolder(taskList, store.GetRootFolder() as Folder);
        }

        private static void FindTasksInFolder(List<TaskItem> taskList, Folder folder)
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

        private static void FindTasksInFolderNonRecursive(List<TaskItem> taskList, Folder folder)
        {
            if (folder.DefaultItemType == OlItemType.olTaskItem)
            {
                foreach (TaskItem taskItem in folder.Items)
                {
                    taskList.Add(taskItem);
                }
            }
        }
    }
}
