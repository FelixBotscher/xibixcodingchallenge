using System.Collections.Generic;

namespace hikingpath
{
    public class Map
    {
        //change to private
        public List<Node> nodes { get; set; }
        public List<Element> elements { get; set; }
        public List<Value> values { get; set; }
        public Map(List<Node> nodes, List<Element> elements, List<Value> values)
        {
            this.nodes = nodes;
            this.elements = elements;
            this.values = values;
        }

        public override string ToString()
        {
            string nodeString = "", elementString = "", valueString = "";//string.Join(",", Children.Select(x => x.Id));
            foreach (var node in nodes)
            {
                nodeString += "\t" + node.ToString() + "\n";
            }

            foreach (var element in elements)
            {
                elementString += "\t" + element.ToString() + "\n";
            }

            foreach (var value in values)
            {
                valueString += "\t" + value.ToString() + "\n";
            }
            return "{\n" + nodeString + "}\n{\n" + elementString + "}\n{\n" + valueString + "}\n";
        }
    }
}