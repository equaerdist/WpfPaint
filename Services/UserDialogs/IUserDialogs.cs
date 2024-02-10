using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfPaint.Services.UserDialogs
{
    interface IUserDialogs
    {
        bool AskForFile(out string filename, string filter, string? defaultExtension = null);
        bool Confirm(string message);
        void ShowError(string message);
        void ShowInfo(string message);
    }
}
