using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;

namespace DialogueSystem.Menu
{
    public struct MenuButtonInitInfo
    {
        public MenuButtonInfo MenuButton { get; set; }
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
        public void OnButtonPressed(MenuButtonInfo.MenuButtonItem info);
    }

    public partial class MenuBar : Godot.MenuBar
    {
        public event Action<MenuButtonInfo.MenuButtonItem> OnMenuItemPressed;

        private List<IMenuListener> _listeners = new();
        private Dictionary<MenuButton, MenuButtonInitInfo> _buttons = new();
        private MenuButton _currentMenu;

        public override void _Ready()
        {

        }

        public void SetButtons(params MenuButtonInitInfo[] buttonInfo)
        {
            this.RemoveChildren<MenuButton>(true);

            foreach (var info in buttonInfo)
                AddButton(info);
        }

        public void AddButton(MenuButtonInitInfo buttonInfo)
        {
            MenuButton button = Tools.Instantiate<MenuButton>(buttonInfo.MenuButtonPrefab) ?? new MenuButton();
            button.Name = buttonInfo.MenuButton.Title;

            foreach (var item in buttonInfo.MenuButton.MenuButtonItems) {
                button.AddItem(item.MenuButtonItem.Name, (int)item.MenuButtonItem.ID, item.MenuButtonItem.Shortcut);
                button += item.MenuButtonItem;
                // TODO: Implement submenu items
            }

            button.OnMenuItemPressed += Button_OnMenuItemPressed;
            button.VisibilityChanged += () => { Button_VisibilityChanged(button); };

            AddChild(button);

            _buttons.Add(button, buttonInfo);
        }


        public void RemoveButton(MenuButton button)
        {
            if (_buttons.ContainsKey(button))
                RemoveButton(_buttons[button]);
        }
        public void RemoveButton(MenuButtonInitInfo buttonInfo)
        {
            MenuButton button = this.GetChildren<MenuButton>().FirstOrDefault(x => x.Title == buttonInfo.MenuButton.Title);

            button.OnMenuItemPressed -= Button_OnMenuItemPressed;

            _buttons.Remove(button);

            button.QueueFree();
        }

        private void Button_OnMenuItemPressed(MenuButton button, long id)
        {
            OnMenuItemPressed?.Invoke(button.Items.First(x => x.ID == id));
        }

        private void Button_VisibilityChanged(MenuButton menu)
        {
            _currentMenu = menu;
        }
    }
}
