using Godot;
using System.Collections.Generic;

namespace DialogueSystem.Menu
{
    public class MenuButtonDictionarySet
    {
        public MenuButton.MenuButtonItem MenuButtonItem { get; set; }
        public MenuButtonDictionary SubMenuButtons { get; set; }
    }
    public class MenuButtonDictionary : List<MenuButtonDictionarySet>
    {
    }

    public struct MenuButton
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
}
