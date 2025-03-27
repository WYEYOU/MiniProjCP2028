using MiniProject.ViewModel;

namespace MiniProject.Pages
{
	public partial class HomePage : ContentPage
	{
		public HomePage()
		{
			InitializeComponent();
			BindingContext = new HomeViewModel();
		}
	}
}

