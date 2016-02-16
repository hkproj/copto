using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Copto.Tests
{
    [TestClass()]
    public class OptionsTests
    {

        [TestMethod()]
        [TestCategory("Rules")]
        public void TestEmptyRules()
        {
            var options = Options.Parse(new string[]
            {
                "genreport",
                "/portfolio",
                "-stock",
                "--verbose",
            });

            options.Apply(new RuleSet()
            {

            });

            Assert.IsTrue(true);
        }

        [TestMethod]
        [TestCategory("Rules")]
        public void TestRulesWithoutValuesWithoutIndices()
        {
            var options = Options.Parse(new string[]
            {
                "gr",
                "/portfolio",
                "-stock",
                "--verbose",
            });

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

        [TestMethod()]
        [TestCategory("Rules")]
        public void TestRulesWithoutValuesWithIndices()
        {
            var options = Options.Parse(new string[]
            {
                "gr",
                "/portfolio",
                "-stock",
                "--verbose",
            });

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
