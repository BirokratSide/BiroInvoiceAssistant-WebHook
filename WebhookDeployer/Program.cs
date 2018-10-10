using si.birokrat.next.common.build;
using si.birokrat.next.common.logging;
using si.birokrat.next.common.registration;
using si.birokrat.next.common.shell;
using System;
using System.IO;
using System.Text.RegularExpressions;

namespace si.birokrat.next.deployer_local
{
    public class Program
    {
        
        private const string DEPLOY_DIRECTORY = "deploy_local";

        public static void Main(string[] args)
        {
            Console.WriteLine("[Publishing Webhook]\n");

            
            PublishProject("InvoiceAssistantWebhook");
            
            Console.WriteLine("\nPress any key to exit...");
            Console.ReadLine();
        }

        private static void PublishProject(string projectName) {
            bool success = false;

            Console.Write($"Publishing {projectName} project");

            string solutionPath = Build.SolutionPath;
            string projectPath = Path.Combine(solutionPath, projectName);
            string projectDeployPath = Path.Combine(solutionPath, DEPLOY_DIRECTORY, projectName);
            Directory.Delete(projectDeployPath, recursive: true);

            success = PublishCoreProject(solutionPath, projectPath, projectDeployPath);

            if (!success)
            {
                Console.WriteLine("...ERROR\n\nPress any key to exit.");
                Console.ReadLine();
                Environment.Exit(-1);
            }



            Console.WriteLine("SUCCESS");
        }
       
        private static bool PublishCoreProject(string solutionPath, string projectPath, string projectDeployPath)
        {
            string command = $"dotnet publish \"{projectPath}\" " +
                $"--configuration Release " +
                $"--framework netcoreapp2.0 " +
                $"--output \"{projectDeployPath}\" " +
                $"--verbosity normal";
            string result = CommandPrompt.Execute(command);

            return result.Contains("Build succeeded");
        }
    }
}