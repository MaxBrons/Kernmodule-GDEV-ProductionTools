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
        [Export] private ScriptableObject[] _buttonInfo;

        public override void _Ready()
        {
            // Initialize the menu bar.
            foreach (IMenuButton item in _buttonInfo.Cast<IMenuButton>()) {
                if (item == null)
                    continue;

                _menuBar.AddButton(item.ButtonInfo);
            }

            _menuBar.OnMenuItemPressed += new AddNodeToGraphHook(_graph, "Add Text Node", "res://_DialogueSystem/Scenes/Nodes/TextNode.tscn").AddNodeToGraph;
        }
    }
}