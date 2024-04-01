using Godot;

namespace DialogueSystem.Menu
{
    public partial class FileButtonSettings : ScriptableObject, IMenuButton
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
                            MenuButtonItem = new(){ Name = "Save", ID = 0, Shortcut = (Key)KeyModifierMask.MaskCtrl | Key.S },
                        },
                        new() {
                            MenuButtonItem = new(){ Name = "Load", ID = 1, Shortcut = (Key)KeyModifierMask.MaskCtrl | Key.O },
                        },
                        new() {
                            MenuButtonItem = new(){ Name = "Export", ID = 2, Shortcut = (Key)KeyModifierMask.MaskCtrl | Key.E },
                        }
                    }
                },
                MenuButtonPrefab = _menuButtonPrefab
            };
        }
    }
}
