using Godot;

public partial class TextNode : GraphNode
{
    private TextEdit _textBox;

    public override void _Ready()
    {
        _textBox = GetChild<TextEdit>(0);

        SetSlot(0, true, 1, new Color(1, 1, 1, 1), true, 0, new Color(1, 1, 1, 1));
    }
}
