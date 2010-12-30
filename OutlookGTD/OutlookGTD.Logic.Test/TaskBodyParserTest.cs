using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OutlookGTD.Logic;

namespace OutlookGTD.Logic.Test
{
    [TestClass]
    public class TaskBodyParserTest
    {

        [TestMethod]
        public void TestMethod1()
        {
            string entryId;
            string folderPath;
            TaskBodyParser.GetFolderPathAndEntryId("", out folderPath, out entryId);
        }
    }
}
