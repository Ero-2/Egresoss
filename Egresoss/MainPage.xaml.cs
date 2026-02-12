using System.Diagnostics;
using Egresoss.ViewModels;

namespace Egresoss
{
    public partial class MainPage : ContentPage
    {
        public MainPage(MainViewModel vm)
        {
            InitializeComponent();
            BindingContext = vm;
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();

            if (BindingContext is MainViewModel vm)
            {
                await vm.LoadData();
                Debug.WriteLine($"[DEBUG] RecentTransactions.Count = {vm.RecentTransactions.Count}");
                for (int i = 0; i < Math.Min(5, vm.RecentTransactions.Count); i++)
                {
                    var t = vm.RecentTransactions[i];
                    Debug.WriteLine($"[DEBUG] Tx {i}: Category='{t.Category}', Amount={t.Amount}, Date={t.Date}, IsIncome={t.IsIncome}");
                }
            }
        }

        private async void OnAddTransactionClicked(object sender, EventArgs e)
        {
            await Shell.Current.GoToAsync(nameof(NewTransactionPage));
        }
    }
}