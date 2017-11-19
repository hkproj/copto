using NUnit.Framework;

namespace Copto.Tests
{
    [TestFixture()]
    public class OptionsTests
    {

        ParserOptions GetDefaultParserOptions()
        {
            return new ParserOptions()
            {
                CaseSensitive = false
            };
        }

        [Test]
        public void TestEmptyRules()
        {
            var options = Options.Parse(new string[]
            {
                "genreport",
                "/portfolio",
                "-stock",
                "--verbose",
            }, GetDefaultParserOptions());

            options.Apply(new RuleSet()
            {

            });

            Assert.IsTrue(true);
        }

        [Test]
        public void TestCaseSensitiveness()
        {
            var options = Options.Parse(new string[]
            {
                "gen-report",
                "/Portfolio",
                "-sTock",
                "--verbose",
            }, new ParserOptions()
            {
                CaseSensitive = true
            });

            string operation = null;
            bool? portfolio = null, stock = null, verbose = null;

            options.Apply(new RuleSet()
            {
                { "gen-report", () => operation = "gen-report" },
                { "portfolio", () => portfolio= true },
                { "stock", () => stock = true },
                { "verbose", (v) => verbose = v ?? true }
            });

            Assert.AreEqual("gen-report", operation, "operation");
            Assert.IsNull(portfolio, "portfolio should be null");
            Assert.IsNull(stock, "stock should be null");
            Assert.IsNotNull(verbose, "verbose should not be null");
            Assert.IsTrue(verbose.Value, "verbose");

            options.Apply(new RuleSet()
            {
                { "Portfolio", () => portfolio = true },
                { "sTock", () => stock = true }
            });

            Assert.IsNotNull(portfolio, "portfolio shoult not be null");
            Assert.IsTrue(portfolio.Value, "portfolio");
            Assert.IsNotNull(stock, "stock should not be null");
            Assert.IsTrue(stock.Value, "stock");
        }

        [Test]
        public void TestRulesWithoutValuesWithoutIndices()
        {
            var options = Options.Parse(new string[]
            {
                "gr",
                "/portfolio",
                "-stock",
                "--verbose",
            }, GetDefaultParserOptions());

            string operation = null;
            bool? portfolio = null;
            bool? stock = null;
            bool? verbose = null;

            options.Apply(new RuleSet()
            {
                { "gr", () => operation = "gr" },
                { "portfolio", () => portfolio = true },
                { "stock", () => stock = false },
                { "verbose", () => verbose = true }
            });

            Assert.AreEqual("gr", operation, "operation");
            Assert.IsTrue(portfolio.Value, "portfolio");
            Assert.IsFalse(stock.Value, "stock");
            Assert.IsTrue(verbose.Value, "verbose");
        }

        [Test]
        public void TestRulesWithoutValuesWithIndices()
        {
            var options = Options.Parse(new string[]
            {
                "gr",
                "/portfolio",
                "-stock",
                "--verbose",
            }, GetDefaultParserOptions());

            string operation = null;
            bool? portfolio = null;
            bool? stock = null;
            bool? verbose = null;

            options.Apply(new RuleSet()
            {
                { "gr", () => operation = "gr", 0 },
                { "portfolio", () => portfolio = true, 1 },
                { "stock", () => stock = true, 1 }, // Fail on purpose
                { "verbose", () => verbose = true, 3 }
            });

            Assert.AreEqual("gr", operation, "operation");
            Assert.IsTrue(portfolio.Value, "portfolio");
            Assert.IsNull(stock, "stock");
            Assert.IsTrue(verbose.Value, "verbose");

            options.Apply(new RuleSet()
            {
                { "stock", () => stock = true, 2 } // Now it must work!
            });

            Assert.IsTrue(stock.Value, "stock2");
        }

    }
}
