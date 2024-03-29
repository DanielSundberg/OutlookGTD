﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Microsoft.Office.Interop.Outlook;
using OutlookGTD.Common;
using OutlookGTP.UI;
using Exception = System.Exception;
using Outlook = Microsoft.Office.Interop.Outlook;
using System.Text.RegularExpressions;
using System.Runtime.InteropServices;

namespace OutlookGTD.Logic
{
    public class TaskBodyParser : IMailItemFinder
    {
        private TaskItem _taskItem;
        private Stores _stores;

        public TaskBodyParser(Stores stores)
        {
            _stores = stores;
        }

        public TaskBodyParser(TaskItem taskItem, Stores stores)
        {
            _taskItem = taskItem;
            _stores = stores;
        }

        public List<MessageWrapper> ParseBody()
        {
            List<MessageWrapper> messages = new List<MessageWrapper>();

            if (_taskItem.Body != null)
            {
                Dictionary<string, string> replaceDictionary = new Dictionary<string, string>();
                using (StringReader stringReader = new StringReader(_taskItem.Body))
                {
                    while (stringReader.Peek() > 0)
                    {
                        string line = stringReader.ReadLine();
                        try
                        {
                            if (line.StartsWith("MailLink"))
                            {
                                string folderPath, entryId, guid, subject;

                                GetFolderPathAndEntryId(line, out folderPath, out entryId, out guid, out subject);

                                string store;
                                List<string> folders;

                                ParseStoreAndFolders(folderPath, out store, out folders);

                                MailItem mailItem = TryGetMailItem(store, entryId);

                                MessageWrapper messageWrapper = new MessageWrapper(this);
                                if (mailItem != null)
                                {
                                    FillMessageWrapperFromMailItem(mailItem, messageWrapper);
                                }
                                else
                                {
                                    messageWrapper.Subject = subject;
                                    messageWrapper.Sender = string.Empty;
                                    messageWrapper.Body = string.Empty;
                                    messageWrapper.EntryId = entryId;
                                    messageWrapper.InvalidLink = true;
                                    messageWrapper.StoreId = store;
                                    messageWrapper.Guid = guid;
                                }
                                messageWrapper.LinkLine = line;
                                messages.Add(messageWrapper);
                            }
                        }
                        catch (FormatException)
                        {
                            // TODO: don't swallow exception
                        }
                    }
                    // Update mail links
                    //if (replaceDictionary.Count > 0)
                    //{w
                    //    foreach (string key in replaceDictionary.Keys)
                    //    {
                    //        _taskItem.Body = _taskItem.Body.Replace(key, replaceDictionary[key]);
                    //    }
                    //    _taskItem.Save();
                    //}
                }
            }
            return messages;
        }

        private static void FillMessageWrapperFromMailItem(MailItem mailItem, IMessageWrapper messageWrapper)
        {
            messageWrapper.Subject = mailItem.Subject;
            messageWrapper.Sender = mailItem.SenderName;
            string body = Utils.RemoveHyperLinks(mailItem.Body);
            messageWrapper.Body = body;
            if (mailItem.Parent is Folder)
            {
                messageWrapper.StoreId = (mailItem.Parent as Folder).StoreID;
            }
            messageWrapper.EntryId = mailItem.EntryID;
        }

        private MailItem TryGetMailItem(string store, string entryId)
        {

            Store currentStore = GetCurrentStore(store);

            try
            {
                MailItem mailItem2 = currentStore.Session.GetItemFromID(entryId, currentStore.StoreID);
                if (mailItem2 != null)
                {
                    // TODO: check if the user property is set, if not, set it
                    // IMAP messages will drop user properties when copied to another folder
                    // POP3 and exchange messages will keep user properties
                    // IMAP messages will however keep user properties when moved to a local folder
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
            return null;
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
                    // TODO: check if the user property is set, if not, set it
                    // IMAP messages will drop user properties when copied to another folder
                    // POP3 and exchange messages will keep user properties
                    // IMAP messages will however keep user properties when moved to a local folder
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
            // IMAP messages will be found by GetItemFromId above unless it has been moved to local storage
            // POP3 I don't know at the moment
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
            // Go through all items in root folder
            Folder rootFolder = (Folder)currentStore.GetRootFolder();

            MailItem mailItem = SearchFolder(guidToFind, rootFolder, out found, out newFolderPath);

            return mailItem;
        }

        private MailItem SearchFolder(string guidToFind, Folder folder, out bool found, out string newFolderPath)
        {
            MailItem mailItem = FindMailItem(guidToFind, folder, out found, out newFolderPath);
            if (found)
            {
                return mailItem;
            }

            // Go through sub folders
            foreach (Folder subFolder in folder.Folders)
            {
                string folderPath = subFolder.FolderPath; // For debug
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
                string folderName = folder.Name; // For debug
                foreach (var item in folder.Items)
                {
                    if (item is MailItem)
                    {
                        MailItem mailItem = item as MailItem;
                        UserProperty userProperty = Utils.GetGtdGuidFromMailItem(mailItem);
                        if (userProperty != null)
                        {
                            if (guidToFind.Equals(userProperty.Value.ToString()))
                            {
                                found = true;
                                newFolderPath = (mailItem.Parent as Folder).FolderPath;
                                return mailItem;
                            }
                        }
                    }
                }
            }
            return null;
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

        public static void GetFolderPathAndEntryId(string mailLinkLine, out string folderPath, out string entryId, out string guid, out string subject)
        {
            subject = string.Empty;
            string data = mailLinkLine.Substring(9);
            var keys = data.Split(new[] { ':' }, 4);
            try
            {
                folderPath = keys[0];
                entryId = keys[1];
                guid = keys[2];
                // Check if we found a subject
                if (keys.Count() > 3)
                {
                    subject = keys[3];
                }
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


        public void Get(IMessageWrapper messageWrapper, string storeId, string entryId, string guid)
        {
            bool itemHasMoved;
            string newFolderPath;
            MailItem mailItem = GetMailItem(storeId, entryId, guid, out itemHasMoved, out newFolderPath);
            FillMessageWrapperFromMailItem(mailItem, messageWrapper);

            // We still have an instance of the task item
            _taskItem.Body = _taskItem.Body.Replace(messageWrapper.LinkLine, Utils.BuildMailItemLink(mailItem, newFolderPath, guid));
            //_taskItem.Session.Application.
        }

    }
}
