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

            // Aseguramos que siempre se carguen las transacciones al aparecer
            if (BindingContext is MainViewModel vm)
            {
                await vm.LoadData();
            }
        }

        private async void OnAddTransactionClicked(object sender, EventArgs e)
        {
            // Navega a la página de registro
            await Shell.Current.GoToAsync(nameof(NewTransactionPage));
        }
    }
}