using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Microsoft.Office.Interop.Outlook;
using OutlookGTP.UI;
using Exception = System.Exception;
using Outlook = Microsoft.Office.Interop.Outlook;
using System.Text.RegularExpressions;
using System.Runtime.InteropServices;

namespace OutlookGTD.Logic
{
    public class TaskBodyParser
    {
        private TaskItem _taskItem;
        private Stores _stores;
        private Folder _rootFolder;

        public TaskBodyParser(TaskItem taskItem, Stores stores)
        {
            _taskItem = taskItem;
            _stores = stores;
        }

        // Parse links of format
        // MailLink=\\daniel.sundberg@tekis.se\Inbox:<EntryId>
        public List<MessageWrapper> ParseBody()
        {
            List<MessageWrapper> messages = new List<MessageWrapper>();

            if (_taskItem.Body != null)
            {
                using (StringReader stringReader = new StringReader(_taskItem.Body))
                {
                    while (stringReader.Peek() > 0)
                    {
                        string line = stringReader.ReadLine();
                        if (line.StartsWith("MailLink"))
                        {
                            string folderPath, entryId, guid;

                            GetFolderPathAndEntryId(line, out folderPath, out entryId, out guid);

                            string store;
                            List<string> folders;

                            ParseStoreAndFolders(folderPath, out store, out folders);

                            bool itemHasMoved;
                            string newFolderPath;
                            MailItem mailItem = GetMailItem(store, entryId, guid, out itemHasMoved, out newFolderPath);

                            if (itemHasMoved)
                            {
                                // Update task mail link
                            }
                            if (mailItem != null)
                            {
                                MessageWrapper messageWrapper = new MessageWrapper();
                                messageWrapper.Subject = mailItem.Subject;
                                messageWrapper.Sender = mailItem.SenderName;
                                messageWrapper.Body = TaskBodyParser.RemoveHyperLinks(mailItem.Body);
                                messages.Add(messageWrapper);
                            }
                            else
                            {
                                MessageWrapper messageWrapper = new MessageWrapper();
                                messageWrapper.Subject = "One mail item not found";
                                messages.Add(messageWrapper);
                            }
                        }
                    }
                }
            }
            return messages;
        }

        private MailItem GetMailItem(
            string store, 
            string entryId, 
            string guid, 
            out bool itemHasMoved, 
            out string newFolderPath)
        {
            newFolderPath = string.Empty;

            // First try to get mail from where it was last time (store and EntryId)
            Store currentStore = GetCurrentStore(store);

            try
            {
                MailItem mailItem2 = currentStore.Session.GetItemFromID(entryId, currentStore.StoreID);
                if (mailItem2 != null)
                {
                    itemHasMoved = false;
                    return mailItem2;
                }
            }
            catch (COMException comEx)
            {
                if (comEx.ErrorCode != -2147221233) // Check for item not found
                {
                    throw;
                }
            }
            itemHasMoved = true;

            // Then search the current store, if we're on exchange the item gets a new EntryId when moved 
            // between folders, this is probably the most likely case...will however slow it down for 
            // POP3 and IMAP users....
            bool found;
            MailItem mailItem = GetMailItemFromStore(guid, currentStore, out found, out newFolderPath);
            if (found)
            {
                return mailItem;
            }

            // The search all other stores
            MailItem mailItem3 = GetMailItemFromAllStoresButCurrent(guid, currentStore, out found, out newFolderPath);
            
            return mailItem3;
        }

        private MailItem GetMailItemFromAllStoresButCurrent(string guid, Store currentStore, out bool found, out string newFolderPath)
        {
            found = false;
            newFolderPath = string.Empty;

            foreach (Store store in _stores)
            {
                if (store.StoreID != currentStore.StoreID)
                {
                    MailItem mailItem = GetMailItemFromStore(guid, store, out found, out newFolderPath);
                    if (found)
                    {
                        return mailItem;
                    }
                }
            }
            return null;
        }

        private MailItem GetMailItemFromStore(string guidToFind, Store currentStore, out bool found, out string newFolderPath)
        {
            newFolderPath = string.Empty;
            found = false;

            // Go through all items in root folder
            Folder rootFolder = (Folder)currentStore.GetRootFolder();

            MailItem mailItem = SearchFolder(guidToFind, rootFolder, out found, out newFolderPath);

            return mailItem;
        }

        private MailItem SearchFolder(string guidToFind, Folder folder, out bool found, out string newFolderPath)
        {
            found = false;
            newFolderPath = string.Empty;

            MailItem mailItem = FindMailItem(guidToFind, folder, out found, out newFolderPath);
            if (found)
            {
                return mailItem;
            }

            // Go through sub folders
            foreach (Folder subFolder in folder.Folders)
            {
                string folderPath = subFolder.FolderPath;
                MailItem mailItem2 = SearchFolder(guidToFind, subFolder, out found, out newFolderPath);
                if (found)
                {
                    return mailItem2;
                }
            }
            return null;
        }

        private MailItem FindMailItem(string guidToFind, Folder folder, out bool found, out string newFolderPath)
        {
            found = false;
            newFolderPath = string.Empty;

            if (folder.DefaultItemType == OlItemType.olMailItem)
            {
                foreach (MailItem item in folder.Items)
                {
                    UserProperty userProperty = Utils.GetGtdGuidFromMailItem(item);
                    if (userProperty != null)
                    {
                        if (guidToFind.Equals(userProperty.Value.ToString()))
                        {
                            found = true;
                            newFolderPath = (item.Parent as Folder).FolderPath;
                            return item;
                        }
                    }
                }
            }
            return null;
        }

        public List<TaskItem> FindAllTasks()
        {
            List<TaskItem> taskList = new List<TaskItem>();
            foreach (Store store in _stores)
            {
                FindTasksInStore(taskList, store);
            }
            return taskList;
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


        Folder FindFolder(Folders folders, string folderToFind)
        {
            Folder foundFolder = null;
            foreach (Folder f in folders)
            {
                if (f.Name == folderToFind)
                {
                    foundFolder = f;
                    break;
                }
            }
            return foundFolder;
        }

        private Store GetCurrentStore(string store)
        {
            Store currentStore = null;
            foreach (Store s in _stores)
            {
                if (s.DisplayName == store)
                {
                    currentStore = s;
                }
            }
            if (currentStore == null)
            {
                throw new ArgumentException("Invalid store");
            }
            return currentStore;
        }

        public static void GetFolderPathAndEntryId(string mailLinkLine, out string folderPath, out string entryId, out string guid)
        {
            string data = mailLinkLine.Substring(9);
            var keys = data.Split(new[] { ':' });
            try
            {
                folderPath = keys[0];
                entryId = keys[1];
                guid = keys[2];
            }
            catch (IndexOutOfRangeException exception)
            {
                throw new FormatException("Mail link is of invalid format", exception);
            }
        }

        public static void ParseStoreAndFolders(string folderPath, out string store, out List<string> folders)
        {
            folders = new List<string>();
            store = string.Empty;

            var folderPathParts = folderPath.Split(new char[] { '\\' });
            if (folderPathParts.Length > 3)
            {
                store = folderPathParts[2];
                for (int i = 3; i < folderPathParts.Length; i++)
                {
                    folders.Add(folderPathParts[i]);
                }
            }
            else
            {
                throw new FormatException(@"Folder path is of invalid format, should be something like: \\your.name@domain.com\Inbox");
            }   
        }

        public static string RemoveHyperLinks(string htmlBody)
        {            
            return Regex.Replace(htmlBody, "HYPERLINK \".*\"", "");
        }
    }
}
