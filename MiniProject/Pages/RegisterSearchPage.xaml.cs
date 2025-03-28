using MiniProject.ViewModel;

namespace MiniProject.Pages;

public partial class RegisterSearchPage : ContentPage
{
	private readonly RegSearchViewModel _viewModel;
	public RegisterSearchPage()
	{
		InitializeComponent();
		_viewModel = new RegSearchViewModel();
		BindingContext = _viewModel;
	}

	protected override async void OnAppearing()
	{
		base.OnAppearing();
		await _viewModel.LoadUserAsync(); // โหลด JSON เมื่อหน้าแสดง
	}
}