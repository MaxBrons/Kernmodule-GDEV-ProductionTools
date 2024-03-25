using Godot;

namespace DialogueSystem.Menu
{
    public partial class MenuBarManager : Godot.MenuBar
    {
        [Export] private FilePopupMenu _fileMenu;
        [Export] private NodesPopupMenu _nodesMenu;
        [Export] private FileDialog _openDialog;
        [Export] private FileDialog _saveDialog;
        [Export] private FileDialog _exportDialog;

        private NodeGraph _graph;

        public override void _Ready()
        {
            _graph = Owner.GetChild<NodeGraph>();

            _fileMenu.OnMenuItemSelected += FileMenu_OnMenuItemSelected;
            _nodesMenu.OnMenuItemSelected += NodesMenu_OnMenuItemSelected;
        }


        private void FileMenu_OnMenuItemSelected(FilePopupMenu.FileNames id)
        {
            switch (id) {
                case FilePopupMenu.FileNames.Open:
                    _fileMenu.OpenFile(_openDialog, _graph);
                    break;
                case FilePopupMenu.FileNames.Save:
                    _fileMenu.SaveFile(_saveDialog, _graph);
                    break;
                case FilePopupMenu.FileNames.Export:
                    _fileMenu.ExportFile(_exportDialog, _graph);
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
}
