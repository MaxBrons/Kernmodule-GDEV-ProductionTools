using Godot;
using System;

namespace DialogueSystem.Menu.Hooks
{
    public class DialogInfo
    {
        public string Title { get; set; }
        public FileDialog.FileModeEnum DialogMode { get; set; }
        public FileDialog.AccessEnum Access { get; set; } = FileDialog.AccessEnum.Filesystem;
        public string[] FileFilters { get; set; }
        public string CancelButtonText { get; set; } = "Cancel";
        public string ComfirmButtonText { get; set; } = "Save";
        public string RootFolder { get; set; }
        public bool ShowHiddenFiles { get; set; }
        public bool UseNativeDialog { get; set; } = true;
    }

    public delegate void DialogueConfirmedEvent(string path);

    public class FileDialogHook
    {
        private FileDialog _dialog;
        private string _buttonLabelToHookOn;
        private DialogInfo _dialogInfo;
        private DialogueConfirmedEvent _onDialogConfirm;
        private Action _onDialogCancel;

        public FileDialogHook(string buttonLabelToHookOn, FileDialog dialog, DialogInfo dialogInfo, DialogueConfirmedEvent onDialogConfirm, Action onDialogCancel)
        {
            _dialog = dialog;
            _dialogInfo = dialogInfo;
            _buttonLabelToHookOn = buttonLabelToHookOn;
            _onDialogConfirm = onDialogConfirm;
            _onDialogCancel = onDialogCancel;
        }

        public void OpenDialog(MenuButtonInfo.MenuButtonItem button)
        {
            if (button.Name != _buttonLabelToHookOn)
                return;

            _dialog.Dispose();
            _dialog = new()
            {
                Title = _dialogInfo.Title,
                FileMode = _dialogInfo.DialogMode,
                Access = _dialogInfo.Access,
                Filters = _dialogInfo.FileFilters,
                CancelButtonText = _dialogInfo.Title,
                OkButtonText = _dialogInfo.Title,
                RootSubfolder = _dialogInfo.RootFolder,
                ShowHiddenFiles = _dialogInfo.ShowHiddenFiles,
                UseNativeDialog = _dialogInfo.UseNativeDialog
            };

            if (_onDialogConfirm != null)
                _dialog.FileSelected += OnFileSelected;

            if (_onDialogCancel != null)
                _dialog.Canceled += _onDialogCancel;

            _dialog.Canceled += UnsubscribeEvents;

            _dialog.Show();
        }

        public void UnsubscribeEvents()
        {
            if (_onDialogConfirm != null)
                _dialog.FileSelected -= OnFileSelected;

            if (_onDialogCancel != null)
                _dialog.Canceled -= _onDialogCancel;

            _dialog.Canceled -= UnsubscribeEvents;
        }

        private void OnFileSelected(string path)
        {
            _onDialogConfirm?.Invoke(path);

            UnsubscribeEvents();
        }
    }
}
