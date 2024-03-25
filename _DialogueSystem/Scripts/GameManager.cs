using DialogueSystem.Menu;
using Godot;
using System.Linq;

namespace DialogueSystem
{
    public partial class GameManager : Control
    {
        [Export] private Menu.MenuBar _menuBar;
        [Export] private ScriptableObject[] _buttons;

        public override void _Ready()
        {
            foreach (IMenuButton item in _buttons.Cast<IMenuButton>()) {
                if (item == null)
                    continue;

                _menuBar.AddButton(item.ButtonInfo);
            }

        }
    }
}