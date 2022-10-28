using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using System.Data.Common;
using TemplateHandler;

namespace TemplateHandlerTests
{
    [TestClass]
    public class ArchiveFileTests
    {
        // Variables used in the tests.
        // They are initialized in the Initialize function decorated with [TestInitialize()].
        private TestArchiveFileContext archiveFileContext { get; set; }
        private ArchiveFile file1;
        private ArchiveFile file2;
        private ArchiveFile file3;
        private string checksumFilePath;
        private string file1Checksum { get; set; }
        private string file2Checksum { get; set; }

        private void initChecksumFile()
        {
            checksumFilePath = Path.Combine(Directory.GetCurrentDirectory(), "checksums.txt");
            List<string> checksums = new List<string>();
            checksums.Add(file1.Checksum);
            checksums.Add(file2.Checksum);
            checksums.Add(file3.Checksum);
            File.WriteAllLines(checksumFilePath, checksums);
        }

        [TestInitialize()]
        public void Initialize()
        {
            // Randomly generated checksums.
            file1Checksum = "12AAEC23D1B0E0DE561F54F8BB9C45E58806429134224F0A519A59204BC81BE9";
            file2Checksum = "46403F0A3C6948ADAA0C9ABC58845999FD92C9CF629AA375D7171C365BAA4334";
            
            DbConnection _connection = new SqliteConnection("Filename=:memory:");
            _connection.Open();
            DbContextOptions<TestArchiveFileContext> options =
                new DbContextOptionsBuilder<TestArchiveFileContext>().UseSqlite(_connection).Options;

            archiveFileContext = new TestArchiveFileContext(options);

            if (archiveFileContext.Database.EnsureCreated())
            {
                DbCommand createTable = archiveFileContext.Database.GetDbConnection().CreateCommand();
                createTable.CommandText = @"CREATE TABLE IF NOT Exists ""Files"" 
                                            (
	                                            id INTEGER NOT NULL, 
	                                            uuid VARCHAR NOT NULL, 
	                                            relative_path VARCHAR NOT NULL, 
	                                            checksum VARCHAR, 
	                                            puid VARCHAR, 
	                                            signature VARCHAR, 
	                                            is_binary BOOLEAN, 
	                                            file_size_in_bytes INTEGER, 
	                                            warning VARCHAR, 
	                                            PRIMARY KEY (id), 
	                                            CHECK (is_binary IN (0, 1))
                                            )";
                createTable.ExecuteNonQuery();

                createTable.CommandText = @"CREATE TABLE IF NOT Exists ""_ConvertedFiles"" 
                                            (
	                                            file_id INTEGER NOT NULL, 
	                                            uuid VARCHAR NOT NULL, 
                                                PRIMARY KEY (file_id), 
                                            )";
            }

            file1 = new ArchiveFile(1, "182091849843", "testData/1.pdf",
                                    file1Checksum, "fmt/20",
                                    "Acrobat PDF 1.6", 1, 2000, null);

            file2 = new ArchiveFile(2, "282091849843", "testData/2.png", file2Checksum, "fmt/11",
                                    "Portable Network Graphics (PNG) version 1", 1, 2000, null);

            // Note that the checksum for file2 and file3 is identical, as they
            // represent duplicates of the same file in the test.
            file3 = new ArchiveFile(3, "282091849844", "testData/3.png", file2Checksum, "fmt/11",
                                    "Portable Network Graphics (PNG) version 1", 1, 2000, null);
            
            archiveFileContext.AddRange(
                    file1, file2, file3
            );

            archiveFileContext.SaveChanges();

            initChecksumFile();
        }

        [TestCleanup()]
        public void Cleanup()
        {
            archiveFileContext.Database.CloseConnection();
            archiveFileContext.Dispose();
            File.Delete(checksumFilePath);
        }

        // The Arrange step in the following GetArchiveFiles tests happens in the Initialize method.

        [TestMethod]
        public void TestGetArchiveFilesByPuid()
        {

            List<ArchiveFile> retrievedFiles = ArchiveFile.GetArchiveFiles("fmt/20", archiveFileContext);

            CollectionAssert.Contains(retrievedFiles, file1);
            CollectionAssert.DoesNotContain(retrievedFiles, file2);
            Assert.IsTrue(retrievedFiles.Count == 1);

        }

        [TestMethod]
        public void TestGetArchiveFilesByChecksum()
        {

            List<ArchiveFile> retrievedFiles = ArchiveFile.GetArchiveFiles(file2Checksum, archiveFileContext);

            CollectionAssert.Contains(retrievedFiles, file2);
            CollectionAssert.Contains(retrievedFiles, file3);
            CollectionAssert.DoesNotContain(retrievedFiles, file1);
            Assert.IsTrue(retrievedFiles.Count == 2);

        }


        [TestMethod]
        public void TestGetArchiveFilesByChecksumWhereOneCopyOfFileIsConverted()
        {

            // Add file2 as a converted file.
            // The  Cleanup function will dispose archiveFileContext so the change does not persist.
            ConvertedFile convertedFile = new ConvertedFile(2, "282091849843");
            archiveFileContext.AddRange(convertedFile);
            archiveFileContext.SaveChanges();

            List<ArchiveFile> retrievedFiles = ArchiveFile.GetArchiveFiles(file2Checksum, archiveFileContext);

            
            CollectionAssert.DoesNotContain(retrievedFiles, file1);
            CollectionAssert.DoesNotContain(retrievedFiles, file2);
            CollectionAssert.Contains(retrievedFiles, file3);
            Assert.IsTrue(retrievedFiles.Count == 1);

        }

        [TestMethod()]
        public void TestGetArchiveFilesByChecksumFile()
        {

            List<ArchiveFile> retrievedFiles = ArchiveFile.GetArchiveFiles(checksumFilePath, archiveFileContext);

            CollectionAssert.Contains(retrievedFiles, file1);
            CollectionAssert.Contains(retrievedFiles, file2);
            CollectionAssert.Contains(retrievedFiles, file3);
            Assert.IsTrue(retrievedFiles.Count == 3);

        }

    }
}
