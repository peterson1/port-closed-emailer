using System;
using System.Threading.Tasks;

namespace WpfApp1.WpfTools
{
    public static class GlobalErrors
    {
        public static void HandleAll()
        {
            App.Current.DispatcherUnhandledException += (s, e) =>
            {
                e.Handled = true;
                OnError(e.Exception, "Application.Current");
            };

            TaskScheduler.UnobservedTaskException += (s, e) =>
            {
                e.SetObserved();
                OnError(e.Exception, "TaskScheduler");
            };

            AppDomain.CurrentDomain.UnhandledException += (s, e) =>
            {
                OnError(e.ExceptionObject as Exception, "AppDomain.CurrentDomain");
                // application terminates after above
            };
        }


        private static void OnError(Exception ex, string ctx)
        {
            Alert.Show(ex, ctx);
        }
    }
}
