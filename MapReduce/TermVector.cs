using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MapReduce
{
    public class TermVector
    {
        public string HostName { get; set; }
        public Dictionary<string, double> Terms { get; set; } = new Dictionary<string, double>();
    }
}