using Godot;

namespace DialogueSystem.Menu
{
    public partial class NodeButtonSettings : ScriptableObject, IMenuButton
    {
        [Export] private string _title;
        [Export] private string _menuButtonPrefab;

        public MenuButtonInitInfo ButtonInfo {
            get => new()
            {
                MenuButton = new()
                {
                    Title = _title,
                    MenuButtonItems = new()
                    {
                         new() {
                            MenuButtonItem = new(){ Name = "Add Text Node", ID = 0, Shortcut = (Key)KeyModifierMask.MaskAlt | Key.Key1 },
                            SubMenuButtons = null
                        },
                    }
                },
                MenuButtonPrefab = _menuButtonPrefab
            };
        }
    }
}
