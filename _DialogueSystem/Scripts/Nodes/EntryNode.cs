using Godot;

public partial class EntryNode : GraphNode
{
    public void Init(string name, Vector2 position, Vector2 size)
    {
        Name = name;
        PositionOffset = position;
        Size = size;
    }

    public override void _Ready()
    {
        SetSlot(0, false, 0, new Color(1, 1, 1, 1), true, 0, new Color(1, 1, 1, 1));
    }
}
