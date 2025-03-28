using MiniProject.ViewModel;

namespace MiniProject.Pages;

public partial class RegisterPage : ContentPage
{
	private readonly RegViewModel _viewModel;
	public RegisterPage()
	{
		InitializeComponent();
		_viewModel = new RegViewModel();
		BindingContext = _viewModel;
	}

	protected override async void OnAppearing()
	{
		base.OnAppearing();
		await _viewModel.LoadUserAsync(); // โหลด JSON เมื่อหน้าแสดง
	}
}