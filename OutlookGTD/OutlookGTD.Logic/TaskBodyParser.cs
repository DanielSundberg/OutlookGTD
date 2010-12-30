using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Microsoft.Office.Interop.Outlook;
using OutlookGTP.UI;
using Exception = System.Exception;
using Outlook = Microsoft.Office.Interop.Outlook;

namespace OutlookGTD.Logic
{
    public class TaskBodyParser
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
                        string folderPath, entryId;
                        GetFolderPathAndEntryId(line, out folderPath, out entryId);
                        
                        var folders = folderPath.Split(new char[] { '\\' });
                        
                        string folder = folders[folders.Length - 1];

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
    }
}
