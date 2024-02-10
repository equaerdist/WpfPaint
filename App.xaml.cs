using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using WpfPaint.Infrastructure.Behaviors;
using WpfPaint.Services.FileHandler;
using WpfPaint.Services.UserDialogs;
using WpfPaint.ViewModels;

namespace WpfPaint
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private static IHost? _host;
        public static IHost HostInstance
        {
            get
            {
                return _host ??= CreateHost();
            }
        }
        private static IHost CreateHost()
        {
            return Host.CreateDefaultBuilder(Environment.GetCommandLineArgs())
                .UseContentRoot(Environment.CurrentDirectory)
                .ConfigureAppConfiguration((context, cfg) => 
                {
                    cfg.AddJsonFile("appsettings.json", false);
                })
                .ConfigureServices((context, services) =>
                {
                    services.AddSingleton<MainViewModel>();
                    services.AddSingleton<DrawOnInkCanvas>();
                    services.AddSingleton<IUserDialogs, UserDialogs>();
                    services.AddSingleton<IFileHandler, FileHandler>();
                })
                .Build();
        }

    }
}
