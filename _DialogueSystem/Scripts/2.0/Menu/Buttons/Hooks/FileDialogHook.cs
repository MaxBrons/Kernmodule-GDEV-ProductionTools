using Godot;
using System;

namespace DialogueSystem.Menu.Hooks
{
    public struct DialogInfo
    {
        public string Title;
        public FileDialog.FileModeEnum DialogMode;
        public FileDialog.AccessEnum Access = FileDialog.AccessEnum.Filesystem;
        public string[] FileFilters;
        public string CancelButtonText = "Cancel";
        public string ComfirmButtonText;
        public string RootFolder;
        public bool ShowHiddenFiles;
        public bool UseNativeDialog = true;

        public DialogInfo(string title, FileDialog.FileModeEnum dialogMode, FileDialog.AccessEnum access, string[] fileFilter, string cancelButtonText, string comfirmButtonText, string rootFolder, bool showHiddenFiles, bool useNativeDialog)
        {
            Title = title;
            DialogMode = dialogMode;
            Access = access;
            FileFilters = fileFilter;
            CancelButtonText = cancelButtonText;
            ComfirmButtonText = comfirmButtonText;
            RootFolder = rootFolder;
            ShowHiddenFiles = showHiddenFiles;
            UseNativeDialog = useNativeDialog;
        }
    }

    public class FileDialogHook
    {
        private FileDialog _dialog;
        private string _buttonLabelToHookOn;
        private DialogInfo _dialogInfo;
        private Action _onDialogConfirm;
        private Action _onDialogCancel;

        public FileDialogHook(string buttonLabelToHookOn, FileDialog dialog, DialogInfo dialogInfo, Action onDialogConfirm, Action onDialogCancel)
        {
            _dialog = dialog;
            _dialogInfo = dialogInfo;
            _buttonLabelToHookOn = buttonLabelToHookOn;
            _onDialogConfirm = onDialogConfirm;
            _onDialogCancel = onDialogCancel;
        }

        public void OpenDialog(MenuButtonInfo.MenuButtonItem button)
        {
            if (button.Name == _buttonLabelToHookOn) {
                _dialog.Title = _dialogInfo.Title;
                _dialog.FileMode = _dialogInfo.DialogMode;
                _dialog.Access = _dialogInfo.Access;
                _dialog.Filters = _dialogInfo.FileFilters;
                _dialog.CancelButtonText = _dialogInfo.Title;
                _dialog.OkButtonText = _dialogInfo.Title;
                _dialog.RootSubfolder = _dialogInfo.RootFolder;
                _dialog.ShowHiddenFiles = _dialogInfo.ShowHiddenFiles;
                _dialog.UseNativeDialog = _dialogInfo.UseNativeDialog;

                _dialog.Confirmed += _onDialogConfirm;
                _dialog.Canceled += _onDialogCancel;
            }
        }

        public void UnsubscribeEvents()
        {
            _dialog.Confirmed -= _onDialogConfirm;
            _dialog.Canceled -= _onDialogCancel;
        }
    }
}
