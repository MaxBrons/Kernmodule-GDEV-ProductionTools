using Godot;
using System;
using System.IO;

public partial class FilePopupMenu : PopupMenu
{
    public enum FileNames
    {
        Open = 0,
        Save = 1
    }

    public event Action<FileNames> OnMenuItemSelected;

    private FileDialog _currentDialog;

    public override void _Ready()
    {
        Clear();

        IdPressed += NodesPopupMenu_IdPressed;

        AddItem("Open", 0, (Key)KeyModifierMask.MaskCtrl | Key.O);
        AddItem("Save", 0, (Key)KeyModifierMask.MaskCtrl | Key.S);
    }

    protected override void Dispose(bool disposing)
    {
        IdPressed -= NodesPopupMenu_IdPressed;

        base.Dispose(disposing);
    }

    public void OpenFile(FileDialog dialogWindow)
    {
        _currentDialog = dialogWindow;

        if (_currentDialog != null) {
            _currentDialog.FileSelected += OpenDialogWindow_FileSelected;
            _currentDialog.Canceled += OpenDialogWindow_Canceled;

            _currentDialog.Popup();
        }
    }

    public void SaveFile(FileDialog dialogWindow)
    {
        _currentDialog = dialogWindow;

        if (_currentDialog != null) {
            _currentDialog.FileSelected += SaveDialogWindow_FileSelected;
            _currentDialog.Canceled += SaveDialogWindow_Canceled;

            _currentDialog.Popup();
        }
    }

    private void OpenDialogWindow_FileSelected(string path)
    {
        _currentDialog.FileSelected -= OpenDialogWindow_FileSelected;
        _currentDialog.Canceled -= OpenDialogWindow_Canceled;

        GD.Print(File.ReadAllText(path));
    }

    private void OpenDialogWindow_Canceled()
    {
        _currentDialog.FileSelected -= OpenDialogWindow_FileSelected;
        _currentDialog.Canceled -= OpenDialogWindow_Canceled;
    }

    private void SaveDialogWindow_FileSelected(string path)
    {
        _currentDialog.FileSelected -= SaveDialogWindow_FileSelected;
        _currentDialog.Canceled -= SaveDialogWindow_Canceled;

        GD.Print(File.ReadAllText(path));
    }

    private void SaveDialogWindow_Canceled()
    {
        _currentDialog.FileSelected -= SaveDialogWindow_FileSelected;
        _currentDialog.Canceled -= SaveDialogWindow_Canceled;
    }


    private void NodesPopupMenu_IdPressed(long id)
    {
        if (id == 0)
            OnMenuItemSelected?.Invoke(FileNames.Open);
    }
}
