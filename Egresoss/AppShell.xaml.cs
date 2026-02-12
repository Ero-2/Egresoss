using Egresoss;

namespace Egresoss
{
    public partial class AppShell : Shell
    {
        public AppShell()
        {
            InitializeComponent();
            Routing.RegisterRoute(nameof(NewTransactionPage), typeof(NewTransactionPage));
        }
    }
}
