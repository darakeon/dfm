using System.IO;
using System.Linq;

namespace DFM.Console4Tests
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var path = @"Tasks.txt";

            var lines = File.ReadLines(path);

            var newLines = lines.Reverse().ToList();

            File.WriteAllLines(path, newLines);
        }
    }
}
