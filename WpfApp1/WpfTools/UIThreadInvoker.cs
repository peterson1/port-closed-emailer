using System;
using System.Windows;

namespace WpfApp1.WpfTools
{
    public static class UIThread
    {
        public static void Run(Action action)
        {
            if (Application.Current == null)
                action.Invoke();
            else
                Application.Current.Dispatcher.Invoke(action);
        }
    }
}
