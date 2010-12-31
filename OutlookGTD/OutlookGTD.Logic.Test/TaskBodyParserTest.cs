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
        public void TestSimpleCorrectLine()
        {
            //\\daniel.sundberg@tekis.se\ByggR\Driftsättningar\Enköping
            //\\daniel.sundberg@tekis.se\Inbox

            string line = @"MailLink=\\daniel.sundberg@tekis.se\Inbox:ENTRY_ID";
            string entryId;
            string folderPath;
            TaskBodyParser.GetFolderPathAndEntryId(line, out folderPath, out entryId);
            Assert.AreEqual(@"\\daniel.sundberg@tekis.se\Inbox", folderPath);
            Assert.AreEqual("ENTRY_ID", entryId);
        }

        [TestMethod]
        [ExpectedException(typeof(FormatException))]
        public void TestLineWithoutEntryId()
        {
            //\\daniel.sundberg@tekis.se\ByggR\Driftsättningar\Enköping
            //\\daniel.sundberg@tekis.se\Inbox

            string line = @"MailLink=\\daniel.sundberg@tekis.se\Inbox";
            string entryId;
            string folderPath;
            TaskBodyParser.GetFolderPathAndEntryId(line, out folderPath, out entryId);
        }

        [TestMethod]
        public void TestFolderPathOneLevel()
        {
            //\\daniel.sundberg@tekis.se\ByggR\Driftsättningar\Enköping
            //\\daniel.sundberg@tekis.se\Inbox

            string folderPath = @"\\daniel.sundberg@tekis.se\Inbox";

            var folders = TaskBodyParser.ParseFolders(folderPath);

            Assert.AreEqual(1, folders.Count);
            Assert.AreEqual("Inbox", folders[0]);
        }

        [TestMethod]
        public void TestFolderPathThreeLevels()
        {
            //\\daniel.sundberg@tekis.se\ByggR\Driftsättningar\Enköping
            //\\daniel.sundberg@tekis.se\Inbox

            string folderPath = @"\\daniel.sundberg@tekis.se\ByggR\Driftsättningar\Enköping";

            var folders = TaskBodyParser.ParseFolders(folderPath);

            Assert.AreEqual(3, folders.Count);
            Assert.AreEqual("ByggR", folders[0]);
            Assert.AreEqual("Driftsättningar", folders[1]);
            Assert.AreEqual("Enköping", folders[2]);
        }

        [TestMethod]
        [ExpectedException(typeof(FormatException))]
        public void TestFolderPathOfInvalidFormatNoFolders()
        {
            string folderPath = @"\\daniel.sundberg@tekis.se";
            var folders = TaskBodyParser.ParseFolders(folderPath);
        }
    }
}
