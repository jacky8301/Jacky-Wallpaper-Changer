using System.Windows;
using Fluent;
using Jacky__Wallpaper_Changer;

namespace Jack__Wallpaper_Changer
{
    /// <summary>
    /// App.xaml 的交互逻辑
    /// </summary>
    public partial class App : Application
    {
        private void Application_DispatcherUnhandledException(object sender, System.Windows.Threading.DispatcherUnhandledExceptionEventArgs e)
        {
            LogHelper.WriteLog("catch a exception,error info:  ",  e.Exception);
        }
    }
}
