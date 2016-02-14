using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Copto.CommandLine
{
    class Program
    {
        static void Main(string[] args)
        {
            var options = Options.Parse(args);

            string operation = null;
            string output = null;
            string path = null;
            bool verbose = false;
            List<string> linkWith = new List<string>();

            options.Apply(new RuleSet()
            {
                { "genmatrix", (string v) => operation = "genmatrix", 0 },
                { "o|output", (value) => output = value },
                { "path", (value) => path = value },
                { "verbose|v", (v) => verbose = v ?? true },
                { "linkWith|link|l", (l) => linkWith.Add(l) }
            });

            Console.WriteLine("Operation: {0}", operation);
            Console.WriteLine("Output: {0}", output);
            Console.WriteLine("Path: {0}", path);
            Console.WriteLine("Verbose: {0}", verbose);
            foreach (var l in linkWith)
                Console.WriteLine("LinkWith: {0}", l);

            Console.ReadLine();

        }
    }
}
