using MvvmCross.Platforms.Wpf.Views;
using PortClosedEmailer.Core.ViewModels;
using System.Threading.Tasks;
using System.Windows.Input;

namespace WpfApp1.Views
{
    public partial class HomeView : MvxWpfView
    {
        public HomeView()
        {
            InitializeComponent();
            addHostBtn.Click += async (s, e) =>
            {
                await Task.Delay(500);
                newHostTxt.Focus();
            };
            newHostTxt.KeyUp += (s, e) =>
            {
                if (e.Key == Key.Enter)
                    ((HomeViewModel)DataContext).AddHost(newHostTxt.Text, true);
            };
        }
    }
}
