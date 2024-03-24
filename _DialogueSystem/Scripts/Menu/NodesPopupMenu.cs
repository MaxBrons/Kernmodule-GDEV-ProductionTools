using Godot;
using System;

public partial class NodesPopupMenu : PopupMenuItem
{
    public enum NodeNames
    {
        TextNode = 0,
    }

    public event Action<NodeNames> OnMenuItemSelected;

    [Export] private PackedScene[] _nodes;

    public override void _Ready()
    {
        Clear();

        IdPressed += FilePopupMenu_IdPressed;

        base._Ready();
    }

    protected override void Dispose(bool disposing)
    {
        IdPressed -= FilePopupMenu_IdPressed;

        base.Dispose(disposing);
    }

    public void AddNodeToGraph(NodeNames node, Node graph)
    {
        switch (node) {
            case NodeNames.TextNode:
                graph.AddChild(_nodes[0].Instantiate());
                break;
            default:
                break;
        }
    }

    private void FilePopupMenu_IdPressed(long id)
    {
        if (id == 0)
            OnMenuItemSelected?.Invoke(NodeNames.TextNode);
    }

    protected override PopupItem[] GetMenuItems()
    {
        return new[]
        {
           new PopupItem("Add Text Node", 0, (Key)KeyModifierMask.MaskAlt | Key.Key1)
        };
    }
}
