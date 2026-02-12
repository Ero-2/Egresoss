using Egresoss.ViewModels; // Asegúrate de que este namespace coincida con tu TransactionViewModel

namespace Egresoss
{
    public partial class NewTransactionPage : ContentPage
    {
        public NewTransactionPage(TransactionViewModel vm)
        {
            InitializeComponent();
            BindingContext = vm;
        }
    }
}