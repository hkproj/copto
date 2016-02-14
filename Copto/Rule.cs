using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Copto
{
    public class RuleInfo
    {

        public RuleInfo(string rule, Delegate callback, int? index)
        {
            this.Rule = rule;
            this.Callback = callback;
            this.Index = index;
        }

        public string Rule { get; set; }

        public Delegate Callback { get; set; }

        public int? Index { get; set; }

    }
}
