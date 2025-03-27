using MiniProject.Pages;

namespace MiniProject;

public partial class AppShell : Shell
{
	public AppShell()
	{
		InitializeComponent();
		Routing.RegisterRoute(nameof(HomePage), typeof(HomePage));
		Routing.RegisterRoute(nameof(LoginPage), typeof(LoginPage));
		// ลงทะเบียน Route เพื่อให้สามารถใช้ GoToAsync ได้
		Routing.RegisterRoute("//LoginPage", typeof(LoginPage));
	}
}
