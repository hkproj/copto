using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Copto
{
    public static class ExtensionMethods
    {

        public static bool StartsWith(this string str, params char[] prefixes)
        {
            foreach(var prefix in prefixes)
                if (str.StartsWith(prefix.ToString()))
                    return true;

            return false;
        }

        public static bool EndsWith(this string str, params char[] prefixes)
        {
            foreach (var prefix in prefixes)
                if (str.EndsWith(prefix.ToString()))
                    return true;

            return false;
        }

    }
}
