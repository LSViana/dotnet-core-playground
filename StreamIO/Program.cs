using System;
using System.Diagnostics;
using System.IO;

namespace StreamIO
{
    class Program
    {
        // To run this program, add a program with filename 'program.exe' in the root of this project and set it to be copied to output folder from Properties Window
        static void Main(string[] args)
        {
            $"[{this.GetType().Name}] " + "Started C# program");
            var programFileName = Path.Combine(Environment.CurrentDirectory, "program.exe");
            if(File.Exists(programFileName))
            {
                $"[{this.GetType().Name}] " + $"Executable file found at: {programFileName}");
                // Starting the process
                var process = new Process();
                process.StartInfo = new ProcessStartInfo
                {
                    FileName = programFileName,
                    UseShellExecute = false,
                    RedirectStandardInput = true,
                    RedirectStandardOutput = true
                };
                process.Start();
                // Reading from output stream
                var output = process.StandardOutput.ReadLine();
                $"[{this.GetType().Name}] " + output);
                // Writing to input stream
                process.StandardInput.Write(42);
                process.StandardInput.Write("\n");
                // Reading from output stream
                output = process.StandardOutput.ReadLine();
                $"[{this.GetType().Name}] " + output);
            } else
            {
                $"[{this.GetType().Name}] " + $"Executable file not found, searched at: ${programFileName}");
            }
            $"[{this.GetType().Name}] " + "Finished C# program");
        }
    }
}
