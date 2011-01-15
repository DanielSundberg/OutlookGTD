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

            // TODO: display create task form here
            TaskItem taskItem = _application.CreateItem(OlItemType.olTaskItem);
            taskItem.Subject = mailItem.Subject;
            taskItem.Body = BuildMailItemLink(mailItem, folder, guid);
            taskItem.Save();

            MessageBox.Show("Task saved: " + taskItem.Subject);
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

        public void LinkToTaskClicked(IRibbonControl control)
        {
            //MailItem item = control.;
            //var application = MefContainer.GetExportedValue<Application>();
            //var explorer = application.ActiveExplorer();
            //var selection = explorer.Selection.Cast<MailItem>();
            //var mailitem = selection.ElementAt(0);
            //var folder = mailitem.Parent as Folder;


            // Create guid for mail here
            //Guid guid = new Guid();
            //_mailItem.UserProperties.Add("OutlookGTD", OlUserPropertyType.olText).Value = guid.ToString();

            //UserProperty property = null;
            //property = _mailItem.UserProperties.Find("OutlookGTD");
            //if (property != null)
            //{
            //    string str = property.Value.ToString();
            //}
            MessageBox.Show("Link to task clicked");
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
