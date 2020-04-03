using System.Windows.Controls;
using System.Windows.Input;

namespace WpfApp1.Views
{
    public partial class AddNewHostView : UserControl
    {
        public AddNewHostView()
        {
            InitializeComponent();
            addBtn.Click += (s, e) => dlgHost.IsOpen = false;
            hostTxt.KeyUp += (s, e) =>
            {
                if (e.Key == Key.Enter)
                    addBtn.Command.Execute(null);
                else if (e.Key == Key.Escape)
                    dlgHost.IsOpen = false;
            };
        }
    }
}
