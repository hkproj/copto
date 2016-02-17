using NUnit.Framework;
using System.Linq;

namespace Copto.Tests
{
    [TestFixture]
    public class ParserTests
    {

        void ValidateResult(Options options, int expectedArguments)
        {
            Assert.IsNotNull(options.Arguments, "Arguments should not be null");
            Assert.AreEqual(expectedArguments, options.Arguments.Count, "The result should have exactly {0} arguments", expectedArguments);
        }

        void ValidateArgument(Options option, string argumentName, int index, params string[] expectedValues)
        {
            var arg = option[argumentName];
            Assert.IsNotNull(arg, string.Format("Argument {0} should not be null", argumentName));
            Assert.AreEqual(arg.Index, index, string.Format("Argument {0} should have {1} as index", argumentName, index));
            Assert.AreEqual(expectedValues.Length, arg.Values.Count, string.Format("Argument {0} should have exactly {1} values", argumentName, expectedValues.Length));
            for (int i = 0; i < expectedValues.Length; i++)
            {
                string expected = expectedValues[i];
                Assert.IsTrue(arg.Values.Any(v => string.Compare(v, expected, !option.ParsingOptions.CaseSensitive) == 0), string.Format("Argument {0} should contain value {1}", argumentName, expected));
            }
        }

        [Test, TestCaseSource(typeof(Options), "Parser")]
        public void TestEmptyArguments()
        {
            var options = Options.Parse(new string[]
            {
            });

            ValidateResult(options, 0);
        }

        [Test, TestCaseSource(typeof(Options), "Parser")]
        public void TestArgumentsWithoutValue()
        {
            var options = Options.Parse(new string[]
            {
                "genreport",
                "/portfolio",
                "-stock",
                "--verbose",
                "--dist-upgrade",
                "--true/false"
            });

            ValidateResult(options, 6);

            ValidateArgument(options, "genreport", 0);
            ValidateArgument(options, "portfolio", 1);
            ValidateArgument(options, "stock", 2);
            ValidateArgument(options, "verbose", 3);
            ValidateArgument(options, "dist-upgrade", 4);
            ValidateArgument(options, "true/false", 5);
        }

        [Test, TestCaseSource(typeof(Options), "Parser")]
        public void TestArgumentsWithValue()
        {
            var options = Options.Parse(new string[]
            {
                "genreport:pdf",
                "/portfolio=all-inclusive",
                "-stock: / I can write thatever I want here! --like this or /this:ahahaha",
                "--verbose=true and false at the same time.",
            });

            ValidateResult(options, 4);

            ValidateArgument(options, "genreport", 0, "pdf");
            ValidateArgument(options, "portfolio", 1, "all-inclusive");
            ValidateArgument(options, "stock", 2, " / I can write thatever I want here! --like this or /this:ahahaha");
            ValidateArgument(options, "verbose", 3, "true and false at the same time.");
        }

        [Test, TestCaseSource(typeof(Options), "Parser")]
        public void TestArgumentsWithMultipleValues()
        {
            var options = Options.Parse(new string[]
            {
                "-o=report.pdf",
                "-o=report2.pdf",
                "/linkwith=glibc",
                "/linkwith:ncurses",
                "/linkwith:boost"
            });

            ValidateResult(options, 2);

            ValidateArgument(options, "o", 0, "report.pdf", "report2.pdf");
            ValidateArgument(options, "linkwith", 1, "glibc", "ncurses", "boost");
        }

        [Test, TestCaseSource(typeof(Options), "Parser")]
        public void TestArgumentsSeparatedFromValues()
        {
            var options = Options.Parse(new string[]
            {
                "-o",
                "report.pdf",
                "report.pdf",
                "-o",
                "kappa.pdf",
                "--verbose",
                "true"
            });

            ValidateResult(options, 3);

            ValidateArgument(options, "o", 0, "report.pdf", "kappa.pdf");
            ValidateArgument(options, "report.pdf", 1);
            ValidateArgument(options, "verbose", 2, "true");
        }

        [Test, TestCaseSource(typeof(Options), "Parser")]
        public void TestAllKindOfArguments()
        {
            var options = Options.Parse(new string[]
            {
                "compile",
                "project",
                "/name",
                "test_project",
                "--working-dir",
                "/home/projects/",
                "--prefix=/opt/gcc43",
                "--program-suffix=43",
                "--enable-languages:c,c++",
                "--enabled-shared",
                "true OR FALSE doesn't matter, the program will read this.",
                "--enabled-pthreads",
                "--disable-checking=false",
                "--with-system-zlib",
                "--enable-__cxa_atexit",
                "-FALSE",
                "--disable-libunwind-exceptions",
                "--disable-multilib",
                "64",
                "--disable-multilib=32",
                "--disable-multilib:96",
                });

            ValidateResult(options, 16);

            ValidateArgument(options, "compile", 0);
            ValidateArgument(options, "project", 1);
            ValidateArgument(options, "name", 2, "test_project");
            ValidateArgument(options, "working-dir", 3);
            ValidateArgument(options, "home/projects/", 4);
            ValidateArgument(options, "prefix", 5, "/opt/gcc43");
            ValidateArgument(options, "program-suffix", 6, "43");
            ValidateArgument(options, "enable-languages", 7, "c,c++");
            ValidateArgument(options, "enabled-shared", 8, "true OR FALSE doesn't matter, the program will read this.");
            ValidateArgument(options, "enabled-pthreads", 9);
            ValidateArgument(options, "disable-checking", 10, "false");
            ValidateArgument(options, "with-system-zlib", 11);
            ValidateArgument(options, "enable-__cxa_atexit", 12);
            ValidateArgument(options, "FALSE", 13);
            ValidateArgument(options, "disable-libunwind-exceptions", 14);
            ValidateArgument(options, "disable-multilib", 15, "64", "32", "96");
        }

    }
}
