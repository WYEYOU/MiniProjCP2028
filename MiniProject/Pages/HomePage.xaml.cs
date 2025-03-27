using Microsoft.Maui.Controls;
using MiniProject.ViewModel;

namespace MiniProject.Pages
{
    public partial class HomePage : ContentPage
    {
        private readonly HomeViewModel _viewModel;

        public HomePage()
        {
            InitializeComponent();
            _viewModel = new HomeViewModel(Navigation);
            BindingContext = _viewModel;
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();
            await _viewModel.LoadUserAsync(); // โหลด JSON เมื่อหน้าแสดง
        }
    }
}
