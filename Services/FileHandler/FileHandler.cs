using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WpfPaint.Services.UserDialogs;

namespace WpfPaint.Services.FileHandler
{
    class FileHandler : IFileHandler
    {
        private readonly IUserDialogs _dialogs;

        public string SuccessMessage { get ; set; }
        public string ErrorMessage { get; set; }
        public string AuthErrorMessage { get; set; }

        public async Task HandleFileAsync(Func<Task> task)
        {
            try
            {
                await task();
                _dialogs.ShowInfo(SuccessMessage);
            }
            catch(UnauthorizedAccessException ex)
            {
                var message = $"{AuthErrorMessage} {ex.HelpLink}";
                _dialogs.ShowError(message);
            }
            catch(Exception ex)
            {
                var message = $"{ErrorMessage} {ex.Message}";
                _dialogs.ShowError(message);
            }
        }
        public FileHandler(IUserDialogs dialogs)
        {
            _dialogs = dialogs;
            SuccessMessage = "Процесс успешно завершился";
            ErrorMessage = "Произошла непредвиденная ошибка";
            AuthErrorMessage = "Недостаточно прав для выполнения операции";
        }
    }
}
