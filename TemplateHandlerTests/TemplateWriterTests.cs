using Microsoft.VisualStudio.TestTools.UnitTesting;
using TemplateHandler;
using System.Resources;


namespace TemplateHandlerTests
{
    [TestClass]
    public class TemplateWriterTests
    {
        private string testRelPath { get; set; }

        [TestCleanup()]
        public void Cleanup()
        {
            string masterDocumentsDir = Path.Combine(Directory.GetCurrentDirectory(), "MasterDocuments");
            Directory.Delete(masterDocumentsDir, true);
        }

        [TestInitialize()]
        public void Initialize()
        {
            if (System.OperatingSystem.IsWindows())
            {
                testRelPath = "docCollection1\\10\\1.tif";
            }

            else
            {
                testRelPath = "docCollection1/10/1.docx";
            }
        }




        [TestMethod]
        public void TestTemplateWriterInsertTemplate()
        {
            ArchiveFile file = new ArchiveFile(1, "38479384dhfhedfheiufh", testRelPath,
                                                "WEIIHFRIWEHF136287136821634", "fmt/40", "Word file", 1, 200, null);
            string destinationRoot = Path.Combine(Directory.GetCurrentDirectory(), "MasterDocuments");

            byte[] templateContent = Resource.file_not_preservable;
            string outputFile = TemplateWriter.InsertTemplate(file, destinationRoot, templateContent);
            string expected = Path.Combine(destinationRoot, testRelPath);
            Assert.AreEqual(expected, outputFile);
            Assert.IsTrue(File.Exists(outputFile));


        }

        [TestMethod()]
        public void GetTemplateFileTest()
        {
            Assert.Fail();
        }
    }
}