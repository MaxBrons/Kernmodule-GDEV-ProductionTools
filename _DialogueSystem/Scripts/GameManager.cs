using DialogueSystem.Graph;
using DialogueSystem.Menu;
using DialogueSystem.Menu.Hooks;
using Godot;
using System.Linq;

namespace DialogueSystem
{
    public partial class GameManager : Control
    {
        [Export] private Menu.MenuBar _menuBar;
        [Export] private Graph.Graph _graph;
        [Export] private FileDialog _dialog;
        [Export] private GraphSettings _graphSettings;
        [Export] private ScriptableObject[] _buttonInfo;

        private DialogInfo _saveDialogInfo = new()
        {
            Title = "Save a file",
            DialogMode = FileDialog.FileModeEnum.SaveFile,
            FileFilters = new string[] { "*dsf ; Dialogue System File" },
            ComfirmButtonText = "Save",
        };

        private DialogInfo _openDialogInfo = new()
        {
            Title = "Open a file",
            DialogMode = FileDialog.FileModeEnum.OpenFile,
            FileFilters = new string[] { "*dsf ; Dialogue System File" },
            ComfirmButtonText = "Open",
        };


        public override void _Ready()
        {
            // Initialize the menu bar.
            foreach (IMenuButton item in _buttonInfo.Cast<IMenuButton>()) {
                if (item == null)
                    continue;

                _menuBar.AddButton(item.ButtonInfo);
            }

            // Add start node.
            var startNode = _graph.AddNode(_graphSettings.StartNodePrefab) as StartNode;
            startNode.PositionOffset = _graphSettings.StartNodeOffset;
            startNode.SetupNode(default, new()
            {
                RSlotEnabled = true,
            });
            // Combine the menu bar events with their expected functionality.
            _menuBar.OnMenuItemPressed += new AddTextNodeToGraphHook("Add Text Node", _graph, _graphSettings.GetNodePrefabPath("Text Node")).AddNodeToGraph;

            _menuBar.OnMenuItemPressed += new FileDialogHook("Save", _dialog, _saveDialogInfo, () => {
                _graph.SaveGraphToJSON<TextNodeData>(_dialog.CurrentPath);
            }, null).OpenDialog;

            _menuBar.OnMenuItemPressed += new FileDialogHook("Open", _dialog, _openDialogInfo, () => {
                _graph.LoadNodesFromJSON<StartNodeData>(_dialog.CurrentPath);
                _graph.LoadNodesFromJSON<TextNodeData>(_dialog.CurrentPath);
            }, null).OpenDialog;
        }
    }
}