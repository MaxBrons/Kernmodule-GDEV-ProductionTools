using Godot;
using Godot.Collections;

namespace DialogueSystem.Graph
{
    [GlobalClass]
    public partial class GraphSettings : ScriptableObject
    {
        public string StartNodePrefab => _startNodePrefab;
        public Vector2 StartNodeOffset => _startNodeOffset;

        [Export] private string _startNodePrefab;
        [Export] private Vector2 _startNodeOffset = new(300, 300);
        [Export] private Array<GraphNodeSettings> _nodeTypes;

        public string GetNodePrefabPath(string nodeName)
        {
            var node = _nodeTypes.Find(x => x.NodeName == nodeName);

            if (node != null)
                return node.NodePrefabPath;

            GD.PrintErr("Could not find node prefab with name: " + nodeName);
            return string.Empty;
        }
    }
}
