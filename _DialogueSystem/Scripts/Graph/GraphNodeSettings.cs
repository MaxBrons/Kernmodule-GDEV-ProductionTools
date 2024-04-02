using Godot;

namespace DialogueSystem.Graph
{
    [GlobalClass]
    public partial class GraphNodeSettings : Resource
    {
        [Export] public string NodeName;
        [Export] public string NodePrefabPath;
    }
}
