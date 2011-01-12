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
                            string folderPath, entryId;
                            GetFolderPathAndEntryId(line, out folderPath, out entryId);

                            string store;
                            List<string> folders;

                            ParseStoreAndFolders(folderPath, out store, out folders);

                            MailItem mailItem = GetMailItem(store, folders, entryId);
                            MessageWrapper messageWrapper = new MessageWrapper();
                            messageWrapper.Subject = mailItem.Subject;
                            messageWrapper.Sender = mailItem.SenderName;
                            messageWrapper.Body = TaskBodyParser.RemoveHyperLinks(mailItem.Body);

                            messages.Add(messageWrapper);
                        }
                    }
                }
            }
            return messages;
        }

        private MailItem GetMailItem(string store, List<string> folders, string entryId)
        {
            Store currentStore = GetCurrentStore(store);
            Folders childFolders = currentStore.GetRootFolder().Folders;

            MailItem mailItem2 = currentStore.Session.GetItemFromID(entryId, currentStore.StoreID);

            return mailItem2;

            //Folder currentFolder = null;
            //foreach (string folder in folders)
            //{
            //    currentFolder = FindFolder(childFolders, folder);
            //    if (currentFolder != null)
            //    {
            //        childFolders = currentFolder.Folders;
            //    }
            //}
            //if (currentFolder != null)
            //{
            //    foreach (MailItem mailItem in currentFolder.Items)
            //    {
            //        if (mailItem.EntryID == entryId)
            //        {
            //            return mailItem;
            //        }
            //    }
            //}
            //return null;
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

        public static void GetFolderPathAndEntryId(string mailLinkLine, out string folderPath, out string entryId)
        {
            string data = mailLinkLine.Substring(9);
            var keys = data.Split(new[] { ':' });
            try
            {
                folderPath = keys[0];
                entryId = keys[1];
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
