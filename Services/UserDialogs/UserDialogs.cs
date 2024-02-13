using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using WpfPaint.ViewModels;
using WpfPaint.Views;

namespace WpfPaint.Services.UserDialogs
{
    class UserDialogs : IUserDialogs
    {
        string _caption = "Modal";
        public bool AskForFile(out string filename, string filter, bool isSaving, string? defaultExtension = null)
        {
            filename = Guid.NewGuid().ToString();
            var initialDirectory = Directory.GetLogicalDrives().First();
            if (isSaving)
            {
                var dialogs = new SaveFileDialog()
                {
                    Filter = filter,
                    DefaultExt = defaultExtension,
                    AddExtension = true
                };
                dialogs.InitialDirectory = initialDirectory;
                dialogs.FileName = filename;
                if (dialogs.ShowDialog() == true)
                {
                    filename = dialogs.FileName;
                    return true;
                }
                return false;
            }
            var dialog = new OpenFileDialog()
            { 
                Filter = filter,
                DefaultExt = defaultExtension,
                AddExtension = true,
            };
            dialog.InitialDirectory = initialDirectory;
            if(dialog.ShowDialog() == true)
            {
                filename = dialog.FileName;
                return true;
            }
            return false;
           
        }

        public bool Confirm(string message) 
            => MessageBox.Show(message, _caption, MessageBoxButton.OKCancel) == MessageBoxResult.OK;
        

        public void ShowError(string message) 
            => MessageBox.Show(message, _caption, MessageBoxButton.OK, MessageBoxImage.Error);
        

        public void ShowInfo(string message)
            => MessageBox.Show(message, _caption, MessageBoxButton.OK, MessageBoxImage.Information);
        public bool ShowColorPickerDialog(out Color selectedColor)
        {
            var picker = new ColorPicker();
            if(picker.ShowDialog() == true)
            {
                var viewModel = (ColorPickerViewModel)picker.DataContext;
                selectedColor = viewModel.CurrentColor;
                return true;
            }
            return false;
        }
    }
}
