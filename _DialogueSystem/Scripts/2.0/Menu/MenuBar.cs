using Godot;
using System;
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
        public event Action<MenuButton.MenuButtonItem> OnMenuItemPressed;

        private List<IMenuListener> _listeners = new();
        private Dictionary<PopupMenu, MenuButtonInitInfo> _buttons = new();
        private PopupMenu _currentMenu;

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
            button.VisibilityChanged += () => { Button_VisibilityChanged(button); };

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

        private void OnIDPressed(long id)
        {
            foreach (var listener in _listeners) {
                var menuItems = _buttons[_currentMenu];
                var pressedItemInfo = menuItems.MenuButton.MenuButtonItems.First(x => x.MenuButtonItem.ID == id);

                OnMenuItemPressed?.Invoke(pressedItemInfo.MenuButtonItem);
            }
        }

        private void Button_VisibilityChanged(PopupMenu menu)
        {
            _currentMenu = menu;
        }
    }
}
