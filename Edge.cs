using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GreedyAlgs
{
    public class Edge : IComparable<Edge>
    {
        public string From { get; set; }
        public string To { get; set; }

        public int Weight { get; set; }

        public int CompareTo(Edge other)
        {
            return Weight.CompareTo(other.Weight);
        }
    }
}
