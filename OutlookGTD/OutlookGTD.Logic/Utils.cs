using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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


    }
}
