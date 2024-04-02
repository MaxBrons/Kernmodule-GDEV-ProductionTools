using Godot;

namespace DialogueSystem.Graph
{
    public interface ISavable<T>
    {
        public T GetData();
    }

    public interface IGraphNodeData
    {
    }

    public interface IGraphNode
    {
        public void SetupNode(IGraphNodeData data, NodeSlotData slotData);
    }

    public struct NodeSlotData
    {
        public uint Index { get; set; } = 0;
        public bool LSlotEnabled { get; set; } = false;
        public uint LType { get; set; } = 0;
        public Color LSlotColor { get; set; } = new(0xffffffff);
        public bool RSlotEnabled { get; set; } = false;
        public uint RType { get; set; } = 0;
        public Color RSlotColor { get; set; } = new(0xffffffff);

        public NodeSlotData()
        {
        }
    }

    public struct Transform
    {
        public Vector2 Position { get; set; }
        public float Rotation { get; set; }
        public Vector2 Scale { get; set; }

        public Transform(Vector2 position, float rotation, Vector2 scale)
        {
            Position = position;
            Rotation = rotation;
            Scale = scale;
        }
    }
}
