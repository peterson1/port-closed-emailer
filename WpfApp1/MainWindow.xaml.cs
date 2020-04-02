using System;
using MvvmCross.Platforms.Wpf.Views;
using WpfApp1.WpfTools;

namespace WpfApp1
{
    public partial class MainWindow : MvxWindow
    {
        public MainWindow()
        {
            InitializeComponent();
        }


        protected override void OnInitialized(EventArgs e)
        {
            base.OnInitialized(e);
            this.Title += $"  {CurrentExe.GetVersion()}";
        }
    }
}
