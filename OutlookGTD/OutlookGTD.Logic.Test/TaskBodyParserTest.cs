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

            string line = @"MailLink=\\daniel.sundberg@tekis.se\Inbox:ENTRY_ID:GUID-GUID";
            string entryId;
            string folderPath;
            string guid;
            TaskBodyParser.GetFolderPathAndEntryId(line, out folderPath, out entryId, out guid);
            Assert.AreEqual(@"\\daniel.sundberg@tekis.se\Inbox", folderPath);
            Assert.AreEqual("ENTRY_ID", entryId);
            Assert.AreEqual("GUID-GUID", guid);
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
            string guid;
            TaskBodyParser.GetFolderPathAndEntryId(line, out folderPath, out entryId, out guid);
        }

        [TestMethod]
        public void TestFolderPathOneLevel()
        {
            //\\daniel.sundberg@tekis.se\ByggR\Driftsättningar\Enköping
            //\\daniel.sundberg@tekis.se\Inbox

            string folderPath = @"\\daniel.sundberg@tekis.se\Inbox";

            List<string> folders;
            string store;
            TaskBodyParser.ParseStoreAndFolders(folderPath, out store, out folders);

            Assert.AreEqual("daniel.sundberg@tekis.se", store);
            Assert.AreEqual(1, folders.Count);
            Assert.AreEqual("Inbox", folders[0]);
        }

        [TestMethod]
        public void TestFolderPathThreeLevels()
        {
            //\\daniel.sundberg@tekis.se\ByggR\Driftsättningar\Enköping
            //\\daniel.sundberg@tekis.se\Inbox

            string folderPath = @"\\daniel.sundberg@tekis.se\ByggR\Driftsättningar\Enköping";

            List<string> folders;
            string store;
            TaskBodyParser.ParseStoreAndFolders(folderPath, out store, out folders);

            Assert.AreEqual("daniel.sundberg@tekis.se", store);
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
            List<string> folders;
            string store;
            TaskBodyParser.ParseStoreAndFolders(folderPath, out store, out folders);
        }

        [TestMethod]
        public void TestRemoveHyperLinksFromMail()
        {
            string mailBody = "From: HYPERLINK \"mailto:hi-sung.kim@tobii.com\"Hi-Sung Kim\n" +
                "To: HYPERLINK \"mailto:per.neihoff@now.se\"per.neihoff@now.se ";
            string expectedBody = "From: Hi-Sung Kim\n" +
                "To: per.neihoff@now.se ";

            string cleanBody = TaskBodyParser.RemoveHyperLinks(mailBody);
            Assert.AreEqual(expectedBody, cleanBody);
        }
    }
}
