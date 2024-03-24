using DialogueSystem;
using Godot;

public partial class TextNode : GraphNode
{
    public string NodeTitle { get => _titleText.Text; set => _titleText.Text = value; }
    public string NodeContent { get => _contentText.Text; set => _contentText.Text = value; }

    private LineEdit _titleText;
    private TextEdit _contentText;

    public override void _Ready()
    {
        _titleText = this.GetChild<LineEdit>();
        _contentText = this.GetChild<TextEdit>();

        SetSlot(0, true, 0, new Color(1, 1, 1, 1), true, 0, new Color(1, 1, 1, 1));
    }
}
