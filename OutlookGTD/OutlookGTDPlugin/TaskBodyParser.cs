using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Microsoft.Office.Interop.Outlook;
using OutlookGTP.UI;
using Outlook = Microsoft.Office.Interop.Outlook;

namespace OutlookGTDPlugin
{
    class TaskBodyParser
    {
        private TaskItem _taskItem;
        private Folder _rootFolder;

        public TaskBodyParser(TaskItem taskItem, Folder rootFolder)
        {
            _taskItem = taskItem;
            _rootFolder = rootFolder;
        }

        // Parse links of format
        // MailLink=\\daniel.sundberg@tekis.se\Inbox:<EntryId>
        // TODO: get items from deeper folder levels
        public List<MessageWrapper> ParseBody()
        {
            List<MessageWrapper> messages = new List<MessageWrapper>();

            using (StringReader stringReader = new StringReader(_taskItem.Body))
            {
                while (stringReader.Peek() > 0)
                {
                    string line = stringReader.ReadLine();
                    if (line.StartsWith("MailLink"))
                    {
                        string data = line.Substring(9);
                        var keys = data.Split(new[] { ':' });
                        var entryId = keys[1];

                        var folderPath = keys[0].Split(new char[] { '\\' });
                        string folder = folderPath[folderPath.Length - 1];

                        MailItem mailItem = GetMailItem(folder, entryId);
                        
                        MessageWrapper messageWrapper = new MessageWrapper();
                        messageWrapper.Subject = mailItem.Subject;
                        messageWrapper.Sender = mailItem.SenderName;
                        messageWrapper.Body = mailItem.Body;

                        messages.Add(messageWrapper);
                    }
                }
            }
            return messages;
        }

        private MailItem GetMailItem(string folder, string entryId)
        {
            Folders childFolders = _rootFolder.Folders;
            if (childFolders.Count > 0)
            {
                Folder currentFolder = null;
                foreach (Folder childFolder in childFolders)
                {
                    if (childFolder.Name == folder)
                    {
                        currentFolder = childFolder;
                        break;
                    }
                }
                if (currentFolder != null)
                {
                    foreach (MailItem mailItem in currentFolder.Items)
                    {
                        if (mailItem.EntryID == entryId)
                        {
                            return mailItem;
                        }
                    }
                }
            }

            return null;
        }


    }
}
