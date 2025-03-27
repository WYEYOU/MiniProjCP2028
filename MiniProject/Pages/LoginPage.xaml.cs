using Microsoft.Maui.Controls;
using MiniProject.ViewModel;

namespace MiniProject.Pages
{
    public partial class LoginPage : ContentPage
    {
        public LoginPage()
        {
            InitializeComponent();
            BindingContext = new LoginViewModel();
        }
    }
}
