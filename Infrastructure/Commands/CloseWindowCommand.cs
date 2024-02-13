using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace WpfPaint.Infrastructure.Commands
{
    internal class CloseWindowCommand : BaseCommand
    {
        public bool DialogResult { get; set; } = false;
        public override bool CanExecute(object? parameter) => parameter is Window;

        public override void Execute(object? parameter)
        {
            if (parameter is null)
                throw new ArgumentNullException();
            var window = (Window)parameter;
            window.DialogResult = DialogResult;
            window.Close();
        }
    }
}
