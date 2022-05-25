namespace hikingpath
{
    public class Value
    {
        public int element_id  { get; set; }
        public double value { get; set; }

        public Value()
        {
        }
        public Value(int elementId, double value)
        {
            element_id = elementId;
            this.value = value;
        }

        public override string ToString()
        {
            return "element_id: " + element_id + ", value: " + value;
        }
    }
}