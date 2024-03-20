using Godot;

public partial class MenuBarManager : MenuBar
{
    [Export] private FilePopupMenu _fileMenu;
    [Export] private NodesPopupMenu _nodesMenu;
    [Export] private FileDialog _openDialog;
    [Export] private FileDialog _saveDialog;
    [Export] private Node _graph;

    public override void _Ready()
    {
        _fileMenu.OnMenuItemSelected += FileMenu_OnMenuItemSelected;
        _nodesMenu.OnMenuItemSelected += NodesMenu_OnMenuItemSelected;
    }


    private void FileMenu_OnMenuItemSelected(FilePopupMenu.FileNames id)
    {
        switch (id) {
            case FilePopupMenu.FileNames.Open:
                _fileMenu.OpenFile(_openDialog);
                break;
            case FilePopupMenu.FileNames.Save:
                _fileMenu.SaveFile(_saveDialog);
                break;
            default:
                break;
        }
    }

    private void NodesMenu_OnMenuItemSelected(NodesPopupMenu.NodeNames id)
    {
        switch (id) {
            case NodesPopupMenu.NodeNames.TextNode:
                _nodesMenu.AddNodeToGraph(id, _graph);
                break;
            default:
                break;
        }
    }

    protected override void Dispose(bool disposing)
    {
        _fileMenu.OnMenuItemSelected -= FileMenu_OnMenuItemSelected;
        _nodesMenu.OnMenuItemSelected -= NodesMenu_OnMenuItemSelected;

        base.Dispose(disposing);
    }
}
