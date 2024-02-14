using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace WpfPaint.Services.UserDialogs
{
    interface IUserDialogs
    {
        bool AskForFile(out string filename, string filter, bool isSaving, string? defaultExtension = null);
        bool Confirm(string message);
        void ShowError(string message);
        void ShowInfo(string message);
        bool ShowColorPickerDialog(out Color selectedColor);
    }
}
