using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Copto
{
    /// <summary>
    /// Represents a generic command-line argument
    /// </summary>
    public class Argument
    {

        public string Name { get; set; }

        public int Index { get; set; }

        public List<string> Values { get; set; } = new List<string>();

    }
}
