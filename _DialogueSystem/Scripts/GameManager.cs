using DialogueSystem.Graph;
using DialogueSystem.Menu;
using DialogueSystem.Menu.Hooks;
using DialogueSystem.Serialization;
using Godot;
using System.IO;
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

        private DialogInfo _exportDialogInfo = new()
        {
            Title = "Export a file",
            DialogMode = FileDialog.FileModeEnum.SaveFile,
            FileFilters = new string[] { "*json ; JSON" },
            ComfirmButtonText = "Export",
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
            var startNode = _graph.AddNode<StartNode>(_graphSettings.StartNodePrefab);

            startNode.SetupNode(new StartNodeData()
            {
                Title = "Start",
                PrefabPath = _graphSettings.StartNodePrefab,
                Position = _graphSettings.StartNodeOffset,
            }, new()
            {
                RSlotEnabled = true,
            });

            // Combine the menu bar events with their expected functionality.
            _menuBar.OnMenuItemPressed += new AddTextNodeToGraphHook("Add Text Node", _graph, _graphSettings.GetNodePrefabPath("Text Node")).AddNodeToGraph;

            _menuBar.OnMenuItemPressed += new FileDialogHook("Save", _dialog, _saveDialogInfo, (path) => {
                if (Path.GetExtension(path) != ".dsf")
                    path += ".dsf";

                _graph.MarkGraphForSaving<StartNodeData>();
                _graph.MarkGraphForSaving<TextNodeData>();
                _graph.MarkGraphConnectionsForSaving<GraphConnectionsData>();
                _graph.SaveGraphToJSON(path);
            }, null).OpenDialog;

            _menuBar.OnMenuItemPressed += new FileDialogHook("Open", _dialog, _openDialogInfo, (path) => {
                _graph.ClearConnections();
                _graph.RemoveChildren<GraphNode>(true);
                new StartNodeInitializer().InitializeData(_graph, _graph.LoadGraphFromJSON<StartNodeData>(path));
                new TextNodeInitializer().InitializeData(_graph, _graph.LoadGraphFromJSON<TextNodeData>(path));
                new GraphConnectionsInitializer().InitializeData(_graph, _graph.LoadGraphFromJSON<GraphConnectionsData>(path));
            }, null).OpenDialog;

            _menuBar.OnMenuItemPressed += new FileDialogHook("Export", _dialog, _exportDialogInfo, (path) => {
                if (Path.GetExtension(path) != ".json")
                    path += ".json";

                // TODO: implement exporting graph to JSON.
            }, null).OpenDialog;
        }
    }
}