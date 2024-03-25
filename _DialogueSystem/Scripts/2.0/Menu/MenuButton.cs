using Godot;
using System;
using System.Collections.Generic;

namespace DialogueSystem.Menu
{
    public class MenuButtonDictionarySet
    {
        public MenuButtonInfo.MenuButtonItem MenuButtonItem { get; set; }
        public MenuButtonDictionary SubMenuButtons { get; set; }
    }
    public class MenuButtonDictionary : List<MenuButtonDictionarySet>
    {
    }

    public struct MenuButtonInfo
    {
        public struct MenuButtonItem
        {
            public uint ID { get; set; }
            public string Name { get; set; }
            public Key Shortcut { get; set; }
        }

        public string Title { get; set; }
        public MenuButtonDictionary MenuButtonItems { get; set; }
    }

    public partial class MenuButton : PopupMenu
    {
        public event Action<MenuButton, long> OnMenuItemPressed;
        public List<MenuButtonInfo.MenuButtonItem> Items { get; } = new();

        public override void _Ready()
        {
            IdPressed += (id) => OnMenuItemPressed?.Invoke(this, id);
        }

        public static MenuButton operator +(MenuButton left, MenuButtonInfo.MenuButtonItem right)
        {
            left.Items.Add(right);
            return left;
        }

        public static MenuButton operator -(MenuButton left, MenuButtonInfo.MenuButtonItem right)
        {
            left.Items.Remove(right);
            return left;
        }
    }
}
