using Godot;

namespace DialogueSystem.Graph
{
    public partial class Graph : GraphEdit
    {
        // Connection handling
        // Deconnection handling
        // Restricting amount of attachments on port
        public void AddNode(string path)
        {
            var node = GD.Load<PackedScene>(path);

            AddChild(node.Instantiate());
        }
    }
}
