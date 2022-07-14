using System.Security.Cryptography;
using System.Text;
using TemplateHandler;

namespace TemplateHandlerTests
{
    [TestClass]
    public class TemplateWriterTests
    {
        private string testRelPath { get; set; }

        private string getHasAsString(byte[] hash)
        {
            StringBuilder Sb = new StringBuilder();
            foreach (byte b in hash)
            {
                Sb.Append(b.ToString("x2"));
            }

            return Sb.ToString();
        }





        [TestCleanup()]
        public void Cleanup()
        {
            string masterDocumentsDir = Path.Combine(Directory.GetCurrentDirectory(), "MasterDocuments");
            if (Directory.Exists(masterDocumentsDir))
            {
                Directory.Delete(masterDocumentsDir, true);
            }
        }

        [TestInitialize()]
        public void Initialize()
        {
            if (System.OperatingSystem.IsWindows())
            {
                testRelPath = "docCollection1\\10\\1.docx";
            }

            else
            {
                testRelPath = "docCollection1/10/1.docx";
            }
        }




        [TestMethod]
        public void TestTemplateWriterInsertTemplateCorrect()
        {
            ArchiveFile file = new ArchiveFile(1, "38479384dhfhedfheiufh", testRelPath,
                                                "WEIIHFRIWEHF136287136821634", "fmt/40", "Word file", 1, 200, null);
            string destinationRoot = Path.Combine(Directory.GetCurrentDirectory(), "MasterDocuments");

            byte[] templateContent = File.ReadAllBytes("Images/file_not_preservable.tif");
            string outputFile = TemplateWriter.InsertTemplate(file, destinationRoot, templateContent);

            // The expected path is destination root combined with the parent of testRelPath combined with "1.tif".
            string testRelPathParent = Directory.GetParent(testRelPath).FullName;
            string expected = Path.Combine(destinationRoot, testRelPathParent, "1.tif");
           
            Assert.AreEqual(expected, outputFile);
            Assert.IsTrue(File.Exists(outputFile));


        }

        [TestMethod()]
        public void GetTemplateFileTestValidID()
        {
            // Setup
            int testID = 1;
            SHA256 sha256 = SHA256.Create();

            byte[] templateContent = TemplateWriter.GetTemplateFile(testID);
            byte[] expectedContent = File.ReadAllBytes("Images/file_empty.tif");

            // Compute the hash of th two files.
            byte[] templateHash = sha256.ComputeHash(templateContent);
            byte[] expectedHash = sha256.ComputeHash(expectedContent);

            // Convert the hash to a string.
            string templateHashString = getHasAsString(templateHash);
            string expectedHashString = getHasAsString(expectedHash);

            // Assert that the string hahshes are equal.
            Assert.AreEqual(expectedHashString, templateHashString);

        }

        [TestMethod()]
        public void GetTemplateFileTestNonValidID()
        {
            int testID = 500;
            Assert.ThrowsException<ArgumentException>(() => TemplateWriter.GetTemplateFile(testID));
        }
    }
}