using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace WpfPaint.ViewModels
{
    class ViewModelLocator
    {
        public static MainViewModel MainViewModel => App.HostInstance.Services.GetRequiredService<MainViewModel>();
    }
}
