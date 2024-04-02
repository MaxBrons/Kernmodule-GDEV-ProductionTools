using DialogueSystem;
using DialogueSystem.Serialization;
using Godot;
using System;
using System.Collections.Generic;
using System.IO;

public partial class FilePopupMenu : PopupMenuItem
{
    public enum FileNames
    {
        Open = 0,
        Save = 1,
        Export = 2
    }

    public event Action<FileNames> OnMenuItemSelected;

    private FileDialog _currentDialog;
    private NodeGraph _currentGraph;

    public override void _Ready()
    {
        Clear();

        IdPressed += NodesPopupMenu_IdPressed;

        base._Ready();
    }

    protected override void Dispose(bool disposing)
    {
        IdPressed -= NodesPopupMenu_IdPressed;

        base.Dispose(disposing);
    }

    public void OpenFile(FileDialog dialogWindow, NodeGraph graph)
    {
        _currentDialog = dialogWindow;
        _currentGraph = graph;

        if (_currentDialog != null) {
            _currentDialog.FileSelected += OpenDialogWindow_FileSelected;
            _currentDialog.Canceled += OpenDialogWindow_Canceled;

            _currentDialog.Popup();
        }
    }

    public void SaveFile(FileDialog dialogWindow, NodeGraph graph)
    {
        _currentDialog = dialogWindow;
        _currentGraph = graph;

        if (_currentDialog != null) {
            _currentDialog.FileSelected += SaveDialogWindow_FileSelected;
            _currentDialog.Canceled += SaveDialogWindow_Canceled;

            _currentDialog.Popup();
        }
    }

    public void ExportFile(FileDialog dialogWindow, NodeGraph graph)
    {
        _currentDialog = dialogWindow;
        _currentGraph = graph;

        if (_currentDialog != null) {
            _currentDialog.FileSelected += ExportDialogWindow_FileSelected;
            _currentDialog.Canceled += ExportDialogWindow_Canceled;

            _currentDialog.Popup();
        }
    }

    private void OpenDialogWindow_FileSelected(string path)
    {
        OpenDialogWindow_Canceled();

        var items = SaveManager.ReadFromJSON<List<NodeGraph.NodeGraphItem>>(path);

        if (items.Count < 1 || _currentGraph == null)
            return;

        _currentGraph.ClearConnections();
        _currentGraph.RemoveChildren(true);

        List<NodeGraph.NodeGraphConnection> connections = new();

        for (int i = 0; i < items.Count; i++) {
            var item = items[i];

            if (item == null)
                continue;

            var node = GD.Load<PackedScene>(item.Asset).Instantiate();

            if (_currentGraph != null) {
                _currentGraph.AddChild(node);

                connections.AddRange(item.Connections);

                if (i == 0) {
                    EntryNode entryNode = (node as EntryNode);
                    entryNode.Name = item.Name;
                    entryNode.PositionOffset = item.Position;
                    entryNode.Size = item.Size;
                    continue;
                }

                TextNodeOld textNode = (node as TextNodeOld);
                textNode.Name = item.Name;
                textNode.PositionOffset = item.Position;
                textNode.Size = item.Size;
                textNode.NodeTitle = item.Title;
                textNode.NodeContent = item.Text;
            }
        }

        foreach (var connection in connections) {
            GD.Print("A: " + connection.FromNode + "\nB: " + connection.ToNode);
            _currentGraph?.ConnectNode(connection.FromNode, connection.FromPort, connection.ToNode, connection.ToPort);
        }
    }

    private void OpenDialogWindow_Canceled()
    {
        _currentDialog.FileSelected -= OpenDialogWindow_FileSelected;
        _currentDialog.Canceled -= OpenDialogWindow_Canceled;
    }

    private void SaveDialogWindow_FileSelected(string path)
    {
        SaveDialogWindow_Canceled();

        if (Path.GetExtension(path) != ".dsf")
            path += ".dsf";

        var nodes = _currentGraph?.GetGraphForSaving();

        if (nodes != null)
            SaveManager.SaveToJSON(path, nodes);
    }

    private void SaveDialogWindow_Canceled()
    {
        _currentDialog.FileSelected -= SaveDialogWindow_FileSelected;
        _currentDialog.Canceled -= SaveDialogWindow_Canceled;
    }

    private void ExportDialogWindow_FileSelected(string path)
    {
        ExportDialogWindow_Canceled();

        if (Path.GetExtension(path) != ".json")
            path += ".json";

        var nodes = _currentGraph?.GetGraphForSavingJSON();

        if (nodes != null)
            SaveManager.SaveToJSON(path, nodes);
    }

    private void ExportDialogWindow_Canceled()
    {
        _currentDialog.FileSelected -= ExportDialogWindow_FileSelected;
        _currentDialog.Canceled -= ExportDialogWindow_Canceled;
    }


    private void NodesPopupMenu_IdPressed(long id)
    {
        if (id == 0)
            OnMenuItemSelected?.Invoke(FileNames.Open);
        if (id == 1)
            OnMenuItemSelected?.Invoke(FileNames.Save);
        if (id == 2)
            OnMenuItemSelected?.Invoke(FileNames.Export);
    }

    protected override PopupItem[] GetMenuItems()
    {
        return new[]{
           new PopupItem("Open", 0, (Key)KeyModifierMask.MaskCtrl | Key.O),
           new PopupItem("Save", 1, (Key)KeyModifierMask.MaskCtrl | Key.S),
           new PopupItem("Export", 2, (Key)KeyModifierMask.MaskCtrl | Key.E)
        };
    }
}
