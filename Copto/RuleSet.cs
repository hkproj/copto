using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Copto
{
    public class RuleSet : IEnumerable<RuleInfo>
    {

        List<RuleInfo> Rules { get; set; } = new List<RuleInfo>();

        public RuleSet()
        {
            
        }

        public void Add(string rule, Delegate callback, int? index = null)
        {
            Rules.Add(new RuleInfo(rule, callback, index));
        }

        public void Add(string rule, Action callback, int? index = null)
        {
            // Call with argument but ignore it
            Rules.Add(new RuleInfo(rule, new Action<string>((s) => callback()), index));
        }

        public void Add(string rule, Action<string> callback, int? index = null)
        {
            Rules.Add(new RuleInfo(rule, callback, index));
        }

        public void Add(string rule, Action<int?> callback, int? index = null)
        {
            Rules.Add(new RuleInfo(rule, callback, index));
        }

        public void Add(string rule, Action<decimal?> callback, int? index = null)
        {
            Rules.Add(new RuleInfo(rule, callback, index));
        }

        public void Add(string rule, Action<float?> callback, int? index = null)
        {
            Rules.Add(new RuleInfo(rule, callback, index));
        }

        public void Add(string rule, Action<double?> callback, int? index = null)
        {
            Rules.Add(new RuleInfo(rule, callback, index));
        }

        public void Add(string rule, Action<DateTime?> callback, int? index = null)
        {
            Rules.Add(new RuleInfo(rule, callback, index));
        }

        public void Add(string rule, Action<bool?> callback, int? index = null)
        {
            Rules.Add(new RuleInfo(rule, callback, index));
        }

        public IEnumerator<RuleInfo> GetEnumerator()
        {
            return Rules.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

    }
}
