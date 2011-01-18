using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using Microsoft.Office.Interop.Outlook;

namespace OutlookGTD.Logic
{
    public class Utils
    {
        public static string GTD_GUID = "EEEC6A13-753A-4F1D-9F03-B6AB29ACEB03";

        public static UserProperty GetGtdGuidFromMailItem(MailItem mailItem)
        {
            UserProperty property = null;
            property = mailItem.UserProperties.Find(GTD_GUID);
            return property;
        }

        public static string BuildMailItemLink(MailItem mailItem, string folderPath, string guid)
        {
            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.Append("MailLink=");
            stringBuilder.Append(folderPath);
            stringBuilder.Append(":");
            stringBuilder.Append(mailItem.EntryID);
            stringBuilder.Append(":");
            stringBuilder.Append(guid);
            return stringBuilder.ToString();
        }

        public static string BuildMailItemLink(MailItem mailItem, Folder folder, string guid)
        {
            return BuildMailItemLink(mailItem, folder.FolderPath, guid);
        }

        public static string RemoveHyperLinks(string htmlBody)
        {            
            return Regex.Replace(htmlBody, "HYPERLINK \".*\"", "");
        }
    }
}
