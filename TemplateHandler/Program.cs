using System.Threading.Tasks;
namespace TemplateHandler
{
    internal class Program
    {
        static void Main(string[] args)
        {
            if (args[0] == "--help")
            {
                string helpText = File.ReadAllText("Readme.txt");
                Console.WriteLine(helpText);
            }

            else if (args[0].Contains("files.db") == false)
            {
                Console.WriteLine("Invalid argument specified for database.");
                Console.WriteLine("Run program with --help to get documentation on how to use the tool");
            }

            else
            {
                
                // Use named variables or Enumeration type.
                string dbPath = args[0];
                string queryParameter = args[1];
                int templateID;
                int.TryParse(args[2], out templateID);
                string destinationRoot = args[3];
                
                ArchiveFileContext db = new ArchiveFileContext(dbPath);
                
                

                try
                {
                    byte[] templateContent = TemplateWriter.GetTemplateFile(templateID);
                    List<ArchiveFile> files = GetArchiveFiles(queryParameter, db);

                    List<Task> tasks = new List<Task>();

                    foreach (ArchiveFile file in files)
                    {
                        // Create an action delegate that specifies the task to run.
                        Action<object> action = (object file) =>
                        {
                            TemplateWriter.InsertTemplate(file, destinationRoot, templateContent);
                        };

                        // Create a task object based on the action
                        // and add it to the list of tasks.
                        Task t1 = new Task(action, file);
                        tasks.Add(t1);
                    }

                    Console.WriteLine(String.Format("Found {0} files. Inserting templates."), files.Count);
                    foreach (Task task in tasks)
                    {
                        task.Start();
                    }
                   
                    Task.WaitAll(tasks.ToArray());
                    Console.WriteLine("Finished adding templates.");
                }

                catch (NoFilesFoundException e) 
                {     
                    Console.WriteLine(e.Message);
                    Console.WriteLine("Closing application.");
                    Environment.Exit(1);
                }

                catch(ArgumentException e)
                {
                    Console.WriteLine(e.Message);
                    Environment.Exit(1);
                }

                catch (Exception e) 
                { 
                    Console.WriteLine(e.StackTrace);
                    Console.WriteLine("Closing application.");
                    Environment.Exit(1);
                }
                 
               
            }
        }

        private static List<ArchiveFile> GetArchiveFiles(string queryParameter, ArchiveFileContext db)
        {
            List<ArchiveFile> files;
            // If the query parameter is less than 10 chars, we have a puid.
            if (queryParameter.Length < 10)
            {
                
                files = db.Files.Where(f => f.Puid == queryParameter).ToList();
            }
            
            // Else, it must be a checksum.
            else
            {
                
                files = db.Files.Where(f => f.Checksum == queryParameter).ToList();
            }

            if(files.Count > 0)
            {
                return files;
            }

            else
            {
                throw new NoFilesFoundException("There where no files with the specified query parameter: " + queryParameter);
            }
        }
        
       
    }
}