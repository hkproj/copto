using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Copto.Tests
{
    [TestClass]
    public class ParserTests
    {

        [TestMethod()]
        public void TestEmptyArguments()
        {
            var options = Options.Parse(new string[]
            {
            });

            Assert.IsNotNull(options.Arguments);
            Assert.AreEqual(options.Arguments.Count, 0);
        }

        [TestMethod]
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

            Assert.IsNotNull(options.Arguments, "Arguments should not be null");
            Assert.AreEqual(6, options.Arguments.Count);

            Assert.IsNotNull(options["genreport"], "There should be an argument called 'genreport'");
            Assert.IsNotNull(options["portfolio"], "There should be an argument called 'portfolio'");
            Assert.IsNotNull(options["stock"], "There should be an argument called 'stock'");
            Assert.IsNotNull(options["verbose"], "There should be an argument called 'verbose'");
            Assert.IsNotNull(options["dist-upgrade"], "There should be an argument called 'dist-upgrade'");
            Assert.IsNotNull(options["true/false"], "There should be an argument called 'true/false'");
        }

        [TestMethod()]
        public void TestArgumentsWithValue()
        {
            var options = Options.Parse(new string[]
            {
                "genreport:pdf",
                "/portfolio=all-inclusive",
                "-stock: / I can write thatever I want here! --like this or /this:ahahaha",
                "--verbose=true and false at the same time.",
            });

            Assert.IsNotNull(options.Arguments, "Arguments should not be null");
            Assert.AreEqual(4, options.Arguments.Count);

            Assert.IsNotNull(options["genreport"], "There should be an argument called 'genreport'");
            Assert.AreEqual(1, (options["genreport"]).Values.Count);
            Assert.AreEqual("pdf", (options["genreport"]).Values[0]);

            Assert.IsNotNull(options["portfolio"], "There should be an argument called 'portfolio'");
            Assert.AreEqual(1, (options["portfolio"]).Values.Count);
            Assert.AreEqual("all-inclusive", (options["portfolio"]).Values[0]);

            Assert.IsNotNull(options["stock"], "There should be an argument called 'stock'");
            Assert.AreEqual(1, (options["stock"]).Values.Count);
            Assert.AreEqual(" / I can write thatever I want here! --like this or /this:ahahaha", (options["stock"]).Values[0]);

            Assert.IsNotNull(options["verbose"], "There should be an argument called 'verbose'");
            Assert.AreEqual(1, (options["verbose"]).Values.Count);
            Assert.AreEqual("true and false at the same time.", (options["verbose"]).Values[0]);
        }

        [TestMethod()]
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

            Assert.IsNotNull(options.Arguments, "Arguments should not be null");
            Assert.AreEqual(2, options.Arguments.Count);

            Assert.IsNotNull(options["o"], "There should be an argument called 'o'");
            Assert.AreEqual(2, (options["o"]).Values.Count, "The argument 'o' should have 2 values");
            Assert.IsTrue((options["o"]).Values.Contains("report.pdf"));
            Assert.IsTrue((options["o"]).Values.Contains("report2.pdf"));

            Assert.IsNotNull(options["linkwith"], "There should be an argument called 'linkwith'");
            Assert.AreEqual(3, (options["linkwith"]).Values.Count, "The argument 'linkwith' should have 2 values");
            Assert.IsTrue((options["linkwith"]).Values.Contains("glibc"));
            Assert.IsTrue((options["linkwith"]).Values.Contains("ncurses"));
            Assert.IsTrue((options["linkwith"]).Values.Contains("boost"));
        }

        [TestMethod()]
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

            Assert.IsNotNull(options.Arguments, "Arguments should not be null");
            Assert.AreEqual(3, options.Arguments.Count);

            Assert.IsNotNull(options["o"], "There should be an argument called 'o'");
            Assert.AreEqual(2, (options["o"]).Values.Count, "The argument 'o' should have 2 values");
            Assert.IsTrue((options["o"]).Values.Contains("report.pdf"));
            Assert.IsTrue((options["o"]).Values.Contains("kappa.pdf"));

            Assert.IsNotNull(options["report.pdf"], "There should be an argument called 'report.pdf'");

            Assert.IsNotNull(options["verbose"], "There should be an argument called 'verbose'");
            Assert.AreEqual(1, (options["verbose"]).Values.Count);
            Assert.AreEqual("true", (options["verbose"]).Values[0]);
        }

        [TestMethod]
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

            Assert.IsNotNull(options.Arguments, "Arguments should not be null");
            Assert.AreEqual(16, options.Arguments.Count);

            Assert.IsNotNull(options["compile"]);

            Assert.IsNotNull(options["project"]);

            Assert.IsNotNull(options["name"], "There should be an argument called 'name'");
            Assert.AreEqual("test_project", (options["name"]).Values[0]);

            Assert.IsNotNull(options["working-dir"]);

            Assert.IsNotNull(options["home/projects/"]);

            Assert.IsNotNull(options["prefix"], "There should be an argument called 'prefix'");
            Assert.AreEqual("/opt/gcc43", (options["prefix"]).Values[0]);

            Assert.IsNotNull(options["program-suffix"], "There should be an argument called 'program-suffix'");
            Assert.AreEqual("43", (options["program-suffix"]).Values[0]);

            Assert.IsNotNull(options["enable-languages"], "There should be an argument called 'enable-languages'");
            Assert.AreEqual("c,c++", (options["enable-languages"]).Values[0]);

            Assert.IsNotNull(options["enabled-shared"], "There should be an argument called 'enabled-shared'");
            Assert.AreEqual("true OR FALSE doesn't matter, the program will read this.", (options["enabled-shared"]).Values[0]);

            Assert.IsNotNull(options["enabled-pthreads"]);

            Assert.IsNotNull(options["disable-checking"], "There should be an argument called 'disable-checking'");
            Assert.AreEqual("false", (options["disable-checking"]).Values[0]);

            Assert.IsNotNull(options["with-system-zlib"]);

            Assert.IsNotNull(options["enable-__cxa_atexit"]);

            Assert.IsNotNull(options["FALSE"]);

            Assert.IsNotNull(options["disable-libunwind-exceptions"]);

            Assert.IsNotNull(options["disable-multilib"]);
            Assert.AreEqual(3, (options["disable-multilib"]).Values.Count, "The argument 'disable-multilib' should have 3 values");
            Assert.IsTrue((options["disable-multilib"]).Values.Contains("64"));
            Assert.IsTrue((options["disable-multilib"]).Values.Contains("32"));
            Assert.IsTrue((options["disable-multilib"]).Values.Contains("96"));
        }

    }
}
