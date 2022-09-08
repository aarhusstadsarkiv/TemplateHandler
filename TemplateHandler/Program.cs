using System.Reflection;

[assembly: AssemblyVersion("1.0.4.1")]

namespace TemplateHandler
{
    internal class Program
    {
        static void Main(string[] args)
        {
            string execPath = AppDomain.CurrentDomain.BaseDirectory;
            if (args[0] == "--help")
            {
                string helpTextFile = Path.Combine(execPath, "Readme.txt");
                string helpText = File.ReadAllText(helpTextFile);
                Console.WriteLine(helpText);
            }

            else if (args[0] == "--version")
            {
                string version = Assembly.GetExecutingAssembly().GetName().Version!.ToString();
                Console.WriteLine("This is TemplateHandler version: " + version);
            }

            else if (args[0].EndsWith("files.db") == false)
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
                    byte[] templateContent = TemplateWriter.GetTemplateFile(templateID, execPath);
                    List<ArchiveFile> files = ArchiveFile.GetArchiveFiles(queryParameter, db);

                    if (files.Count == 0)
                    {
                        Console.WriteLine("No files found. Closing application");
                        Environment.Exit(0);
                    }

                    List<Task> tasks = new List<Task>();

                    foreach (ArchiveFile file in files)
                    {
                        // Create an action delegate that specifies the task to run.
                        Action<object?> action = (object? file) =>
                        {
                            if (file != null)
                            {
                                TemplateWriter.InsertTemplate(file, destinationRoot, templateContent);
                            }
                        };

                        // Create a task object based on the action
                        // and add it to the list of tasks.
                        Task t1 = new Task(action, file);
                        tasks.Add(t1);
                    }

                    Console.WriteLine(String.Format("Found {0} files. Inserting templates.", files.Count));
                    foreach (Task task in tasks)
                    {
                        task.Start();
                    }

                    Task.WaitAll(tasks.ToArray());
                    Console.WriteLine("Finished adding templates.");
                }



                catch (ArgumentException e)
                {
                    Console.WriteLine(e.Message);
                    Environment.Exit(1);
                }

                catch(FileNotFoundException e)
                {
                    Console.WriteLine(e.Message);
                    Environment.Exit(1);
                }
                catch (Exception e)
                {
                    Console.WriteLine("Exception message: ");
                    Console.WriteLine(e.Message);

                    Console.WriteLine("Stacktrace: ");
                    Console.WriteLine(e.StackTrace);
                    Console.WriteLine("Closing application.");
                    Environment.Exit(1);
                }


            }
        }

    }
}