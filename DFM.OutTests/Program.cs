using System;
using System.IO;
using System.Text.RegularExpressions;

namespace DFM.OutTests
{
    class Program
    {
        static void Main(string[] args)
        {
            var path = @"K:\";

            var pattern = @"(.*)(\d{4})(\d{2})(\d{2})(.*)";
            var regex = new Regex(pattern);

            getFiles(path, regex);

            Console.WriteLine("FINISH!!!");
            Console.Read();

        }

        private static void getFiles(string path, Regex regex)
        {
            if (path.Contains(@"\.hg")
                    || path.Contains(@"\.svn")
                    || path.Contains(@"\bin")
                    || path.Contains(@"\obj")
                    || path.Contains(@"\_ReSharper")
                    || path.Contains(@"\PdbInfo"))
                return;

            var files = Directory.GetFiles(path);

            foreach(var file in files)
            {
                var match = regex.Match(file);

                if (match.Success)
                {
                    var groups = match.Groups;

                    var newName = groups[1].Value
                                  + groups[2].Value + "-"
                                  + groups[3].Value + "-"
                                  + groups[4].Value
                                  + groups[5].Value;

                    //File.Move(file, newName);

                    Console.WriteLine(file);
                    Console.WriteLine(newName);
                    Console.WriteLine();
                }

                var directories = Directory.GetDirectories(path);

                foreach (var dir in directories)
                {
                    getFiles(dir, regex);
                }

            }
        }
    }
}
