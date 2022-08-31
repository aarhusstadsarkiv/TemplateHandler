using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;
using TemplateHandler;

namespace TemplateHandlerTests
{
    [TestClass]
    public class ArchiveFileTests
    {
        private TestArchiveFileContext archiveFileContext { get; set; }
        private ArchiveFile file1;
        private ArchiveFile file2;
        private ArchiveFile file3;
        private string checksumFilePath;

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
            }

            file1 = new ArchiveFile(1, "182091849843", "testData/1.pdf", "7592745734CFC1", "fmt/20", "Acrobat PDF 1.6", 1, 2000, null);
            file2 = new ArchiveFile(2, "282091849843", "testData/2.png", "8592745734CFC1", "fmt/11",
                            "Portable Network Graphics (PNG) version 1", 1, 2000, null);
            file3 = new ArchiveFile(3, "282091849844", "testData/3.png", "8592745734CFC1", "fmt/11",
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

            List<ArchiveFile> retrievedFiles = ArchiveFile.GetArchiveFiles("8592745734CFC1", archiveFileContext);
            CollectionAssert.Contains(retrievedFiles, file2);
            CollectionAssert.Contains(retrievedFiles, file3);
            CollectionAssert.DoesNotContain(retrievedFiles, file1);

            Assert.IsTrue(retrievedFiles.Count == 2);

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
