using Godot;

public abstract partial class PopupMenuItem : PopupMenu
{
    public struct PopupItem
    {
        public string Name;
        public int ID = -1;
        public Key Shortcut = Key.None;

        public PopupItem(string name, int id, Key shortcut = Key.None)
        {
            Name = name;
            ID = id;
            Shortcut = shortcut;
        }
    }

    public override void _Ready()
    {
        foreach (PopupItem item in GetMenuItems()) {
            AddItem(item.Name, item.ID, item.Shortcut);
        }
    }

    protected abstract PopupItem[] GetMenuItems();
}
