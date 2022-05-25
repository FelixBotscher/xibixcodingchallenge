using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualBasic.CompilerServices;

namespace hikingpath
{
    public class Element
    {
        public int id { get; set; }
        public List<int> nodes { get; set; }
        
        public Element(int elementId, List<int> nodes)
        {
            id = elementId;
            this.nodes = nodes;
        }

        public override string ToString()
        {
            return "id: " + id + ", nodes: " + "["+ nodes[0] + ", "+ nodes[1] + ", " + nodes[2] + "]";
        }
    }
}