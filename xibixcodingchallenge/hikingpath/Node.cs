namespace hikingpath
{
    public class Node
    {
        public int id { get; set; }
        public double x { get; set; }
        public double y { get; set; }
        public Node(int id, double x, double y)
        {
            this.id = id;
            this.x = x;
            this.y = y;
        }
        public bool ContainsCoordinate(Node node)
        {
            return node.x == x && node.y == y;
        }
        public override string ToString()
        {
            return "id: " + id + ", x: " + x + ", y: " + y;
        }
    }
}