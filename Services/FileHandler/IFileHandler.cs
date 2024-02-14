using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfPaint.Services.FileHandler
{
    interface IFileHandler
    {
        public string SuccessMessage { get; set; }
        public string ErrorMessage { get; set; }
        public string AuthErrorMessage { get; set; }
        public Task HandleFileAsync(Func<Task> task);
        
    }
}
