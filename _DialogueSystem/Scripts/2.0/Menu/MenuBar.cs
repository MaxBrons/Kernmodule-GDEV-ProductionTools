using Godot;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;

namespace DialogueSystem.Menu
{
    public struct MenuButtonInitInfo
    {
        public MenuButton MenuButton { get; set; }
        public string MenuButtonPrefab { get; set; }

        #region Operators
        public override bool Equals([NotNullWhen(true)] object obj) => base.Equals(obj);
        public override int GetHashCode() => base.GetHashCode();
        public static bool operator ==(MenuButtonInitInfo left, MenuButtonInitInfo right) => Equals(left, right);
        public static bool operator !=(MenuButtonInitInfo left, MenuButtonInitInfo right) => !Equals(left, right);
        #endregion
    }

    public interface IMenuListener
    {
        public void OnButtonPressed(MenuButton.MenuButtonItem info);
    }

    public partial class MenuBar : Godot.MenuBar
    {
        private List<IMenuListener> _listeners = new();
        private Dictionary<PopupMenu, MenuButtonInitInfo> _buttons = new();

        public override void _Ready()
        {

        }

        public void SetButtons(params MenuButtonInitInfo[] buttonInfo)
        {
            this.RemoveChildren<PopupMenu>(true);

            foreach (var info in buttonInfo)
                AddButton(info);
        }

        public void AddButton(MenuButtonInitInfo buttonInfo)
        {
            PopupMenu button = Tools.Instantiate<PopupMenu>(buttonInfo.MenuButtonPrefab) ?? new PopupMenu();
            button.Name = buttonInfo.MenuButton.Title;

            foreach (var item in buttonInfo.MenuButton.MenuButtonItems) {
                button.AddItem(item.MenuButtonItem.Name, (int)item.MenuButtonItem.ID, item.MenuButtonItem.Shortcut);

                // TODO: Implement submenu items
            }

            button.IdPressed += OnIDPressed;
            button.IdFocused += OnIDPressed;

            AddChild(button);

            _buttons.Add(button, buttonInfo);
        }

        public void RemoveButton(PopupMenu button)
        {
            if (_buttons.ContainsKey(button))
                RemoveButton(_buttons[button]);
        }
        public void RemoveButton(MenuButtonInitInfo buttonInfo)
        {
            PopupMenu button = this.GetChildren<PopupMenu>().FirstOrDefault(x => x.Title == buttonInfo.MenuButton.Title);

            button.IdPressed -= OnIDPressed;
            button.IdFocused -= OnIDPressed;

            _buttons.Remove(button);

            button.QueueFree();
        }

        public void AddListener(IMenuListener listener) => _listeners.Add(listener);
        public void RemoveListener(in IMenuListener listener) => _listeners.Remove(listener);

        private void OnIDPressed(long id)
        {
            //_listeners.ForEach(x => x.OnButtonPressed()
            GD.Print(id);
        }
    }
}
